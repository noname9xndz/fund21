using AutoMapper;
using smartFunds.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Data.UnitOfWork;

namespace smartFunds.Service.Services
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllAdditionalCountry(string countryCode);
    }
    public class CountryService : ICountryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        #region Contructor

        public CountryService(IMapper mapper = null, IUnitOfWork unitOfWork = null)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Get additional country in the same region with current country
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public async Task<List<Country>> GetAllAdditionalCountry(string countryCode)
        {
            var additionalCountryCodes = await _unitOfWork.CountryRepository.GetAdditionalCountry(countryCode);

            var result = _mapper.Map<List<Country>>(additionalCountryCodes);

            // Ordering data
            foreach (var country in result)
            {
                foreach (var locality in country.Localities)
                {
                    locality.Sublocalities = locality.Sublocalities.OrderBy(x => x.Name).ToList();
                }

                country.Localities = country.Localities.OrderBy(x => x.Name).ToList();
            }

            return result.OrderBy(x => x.Name).ToList();
        }

        #endregion
    }
}
