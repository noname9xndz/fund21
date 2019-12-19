using AutoMapper;
using smartFunds.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Data.UnitOfWork;

namespace smartFunds.Service.Services
{
    public interface ILocalityService
    {
        Task<List<Locality>> GetLocalityByCountryCode(string countryCode);
        Task<List<Locality>> GetLocalityByListCountryCode(LocalitySearch model);
    }
    public class LocalityService : ILocalityService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        #region Contructor

        public LocalityService(IMapper mapper = null, IUnitOfWork unitOfWork = null)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Get all locality information by country code
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public async Task<List<Locality>> GetLocalityByCountryCode(string countryCode)
        {
            var localities = (await _unitOfWork.LocalityRepository.FindByAsync(x => x.CountryCode == countryCode, "Sublocalities"))?.ToList();
            if (localities == null || !localities.Any()) return new List<Locality>();

            var result = _mapper.Map<List<Locality>>(localities);

            foreach (var locality in result)
            {
                locality.Sublocalities = locality.Sublocalities.OrderBy(x => x.Name).ToList();
            }

            result = result.OrderBy(x => x.Name).ToList();
            return result;
        }

        /// <summary>
        /// Get all locality by list countrycode
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<Locality>> GetLocalityByListCountryCode(LocalitySearch model)
        {
            var localities = await _unitOfWork.LocalityRepository.FindByAsync(x => model.CountryCodes.Contains(x.CountryCode));
            if (localities == null || !localities.Any()) return null;

            var result = _mapper.Map<List<Locality>>(localities);

            return result.OrderBy(x => x.Name).ToList();
        }

        #endregion
    }
}
