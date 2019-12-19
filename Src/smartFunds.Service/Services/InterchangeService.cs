using AutoMapper;
using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Data.UnitOfWork;
using smartFunds.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IInterchangeService
    {
        Task<MemberInterchangeData> GetMemberInterchangeData(User user);
        Task<Interchange> GetInterchangeByLocality(int localityId);
        Task AddOrUpdateInterchangeAsync(Interchange interchange);
    }
    public class InterchangeService : IInterchangeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #region Contructor

        public InterchangeService(IUnitOfWork unitOfWork = null, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Public Method
        /// <summary>
        ///  Add or update interchange
        /// </summary>
        /// <returns></returns>
        public async Task AddOrUpdateInterchangeAsync(Interchange interchange)
        {
            if (interchange == null || string.IsNullOrEmpty(interchange.EmailAddress))
            {
                throw new MissingParameterException();
            }

            interchange.EmailAddress = interchange.EmailAddress.Trim();

            await _unitOfWork.InterchangeRepository.AddOrUpdate(_mapper.Map<Data.Models.Interchange>(interchange, opt => opt.Items.Add("interchangeId", interchange.Id)));
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        ///  Get information about countries and localities which user can be accessed
        /// </summary>
        /// <returns></returns>
        public async Task<MemberInterchangeData> GetMemberInterchangeData(User user)
        {
            if (user.Roles == null || !user.Roles.Any())
                throw new NotFoundException("Roles not found");

            var member = await _unitOfWork.MemberRepository.GetAsync(x => x.Id == user.MemberId);
            if (member == null)
                throw new NotFoundException("Member not found");

            var result = new MemberInterchangeData
            {
                CountryCode = member.CountryCode,
                MemberId = member.Id,
                LocalityId = member.LocalityId ?? 0,
                RegionId = member.RegionId ?? 0,
                IsCoodinatorRole = user.IsInRole(RoleType.smartFunds_CO_ORDINATOR)
            };

            // Filter country base on user's role. 
            // should have region and localities available
            var countries = await _unitOfWork.CountryRepository.GetMemberCountriesAsync(member, user.IsInRole(RoleType.ADMIN), user.IsInRole(RoleType.smartFunds_ADMIN));
            
            result.Countries = _mapper.Map<List<Country>>(countries);

            // Ordering data
            foreach (var country in result.Countries)
            {
                foreach (var locality in country.Localities)
                {
                    locality.Sublocalities = locality.Sublocalities.OrderBy(x => x.Name).ToList();
                }

                country.Localities = country.Localities.OrderBy(x => x.Name).ToList();
            }

            return result;
        }

        /// <summary>
        /// Get interchange by localityId
        /// </summary>
        /// <param name="localityId"></param>
        /// <returns></returns>
        public async Task<Interchange> GetInterchangeByLocality(int localityId)
        {
            // Get interchange information
            var interchange = await _unitOfWork.InterchangeRepository.GetInterchangeByLocality(localityId);

            if (interchange == null) return null;
            var result = _mapper.Map<Interchange>(interchange);

            return result;
        }

        #endregion
    }
}
