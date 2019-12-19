using AutoMapper;
using smartFunds.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Data.UnitOfWork;

namespace smartFunds.Service.Services
{
    public interface IRegionService
    {
        Task<List<Region>> GetAllRegion();
    }
    public class RegionService : IRegionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        #region Contructor

        public RegionService(IMapper mapper = null, IUnitOfWork unitOfWork = null)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Get all region
        /// </summary>
        /// <returns></returns>
        public async Task<List<Region>> GetAllRegion()
        {
            var regions = await _unitOfWork.RegionRepository.GetAllAsync("Countries");
            if (regions == null || !regions.Any()) return null;

            var result = _mapper.Map<List<Region>>(regions);

            foreach (var region in result)
            {
                region.Countries = region.Countries.OrderBy(x => x.Name).ToList();
            }

            return result.OrderBy(x => x.Name).ToList();
        }

        #endregion
    }
}
