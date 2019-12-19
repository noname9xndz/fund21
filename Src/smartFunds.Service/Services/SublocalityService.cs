using AutoMapper;
using smartFunds.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Data.UnitOfWork;

namespace smartFunds.Service.Services
{
    public interface ISublocalityService
    {
        Task<List<Sublocality>> GetSublocalityByListLocalityId(SublocalitySearch model);
    }
    public class SublocalityService : ISublocalityService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        #region Contructor

        public SublocalityService(IMapper mapper = null, IUnitOfWork unitOfWork = null)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Get all locality by list countrycode
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<Sublocality>> GetSublocalityByListLocalityId(SublocalitySearch model)
        {
            var sublocalities = await _unitOfWork.SublocalityRepository.FindByAsync(x => model.LocalityIds.Contains(x.LocalityId));
            if (sublocalities == null || !sublocalities.Any()) return null;

            var result = _mapper.Map<List<Sublocality>>(sublocalities);

            return result.OrderBy(x => x.Name).ToList();
        }

        #endregion
    }
}
