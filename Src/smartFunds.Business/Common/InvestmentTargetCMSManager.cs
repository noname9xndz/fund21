using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Business.Common
{
    public interface IInvestmentTargetCMSManager
    {
        Task<InvestmentTargetSetting> GetInvestmentModel(int? modelId);
        List<InvestmentTargetSetting> GetAll();
        Task<IEnumerable<InvestmentTargetSetting>> UpdateSettings(int[] modelIds);
        Task<InvestmentTargetSetting> AddConfiguration(InvestmentTargetSetting model);
        Task UpdateConfigurations(List<InvestmentTargetSetting> InvestmentTargetSettings);
        Task AddListConfigurations(List<InvestmentTargetSetting> list);
        Task<List<InvestmentTargetSetting>> GetListInvestmentTargetSetting();
        Task<decimal> GetInvestmentTargetSettingValue(int portfolioId, Duration duration);
        Task<int> GetId(decimal value, int portfolioId, Duration duration);
    }
    public class InvestmentTargetCMSManager : IInvestmentTargetCMSManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPortfolioManager _portfolioManager;
        

        public InvestmentTargetCMSManager (IUnitOfWork unitOfWork, IPortfolioManager portfolioManager)
        {
            _unitOfWork = unitOfWork;
            _portfolioManager = portfolioManager;
        }

        public async Task<List<InvestmentTargetSetting>> GetListInvestmentTargetSetting()
        {
            var allPortfolio = await _portfolioManager.GetAllPortfolio();
            var listInvestmentTargetSetting = new List<InvestmentTargetSetting>();
            foreach (var portfolio in allPortfolio)
            {
                foreach (Duration duration in Enum.GetValues(typeof(Duration)))
                {
                    var investmentTargetSetting = await _unitOfWork.InvestmentTargetCMSRepository.GetAsync(i => i.PortfolioId == portfolio.Id && i.Duration == duration, "Portfolio");
                    if(investmentTargetSetting != null)
                    {
                        listInvestmentTargetSetting.Add(investmentTargetSetting);
                    }
                    else
                    {
                        var newInvestmentTargetSetting = new InvestmentTargetSetting()
                        {
                            Duration = duration,
                            PortfolioId = portfolio.Id,
                            Value = 0
                        };
                        var investmentTargetSettingAdded = _unitOfWork.InvestmentTargetCMSRepository.Add(newInvestmentTargetSetting);
                        await _unitOfWork.SaveChangesAsync();
                        listInvestmentTargetSetting.Add(investmentTargetSettingAdded);
                    }
                }
            }
            return listInvestmentTargetSetting;
        }

        public async Task<decimal> GetInvestmentTargetSettingValue(int portfolioId, Duration duration)
        {
            var investmentTargetSetting = await _unitOfWork.InvestmentTargetCMSRepository.GetAsync(x => x.PortfolioId == portfolioId && x.Duration == duration);
            if(investmentTargetSetting != null)
            {
                return investmentTargetSetting.Value;
            }
            return 0;
        }

        public async Task<InvestmentTargetSetting> AddConfiguration(InvestmentTargetSetting model)
        {
            try
            {
                var savedModel = _unitOfWork.InvestmentTargetCMSRepository.Add(model);
                await _unitOfWork.SaveChangesAsync();
                return savedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddListConfigurations(List<InvestmentTargetSetting> list)
        {
            try
            {
                _unitOfWork.InvestmentTargetCMSRepository.BulkInsert(list);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InvestmentTargetSetting> GetAll()
        {
            try
            {
                var list = _unitOfWork.InvestmentTargetCMSRepository.GetAll();
                return list?.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<InvestmentTargetSetting> GetInvestmentModel(int? modelId)
        {
            if (modelId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var investmentModel = await _unitOfWork.InvestmentTargetCMSRepository.GetAsync(m => m.Id == modelId);
                if (investmentModel != null)
                {
                    return investmentModel;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateConfigurations(List<InvestmentTargetSetting> InvestmentTargetSettings)
        {
            try
            {

                var existedList = _unitOfWork.InvestmentTargetCMSRepository
                                .GetInvestCMSModels(InvestmentTargetSettings
                                .Select(h => h.Id).ToArray()).ToList();
                foreach( var item in existedList)
                {
                    foreach( var data in InvestmentTargetSettings)
                    {
                        if(data.Id == item.Id)
                        {
                            item.Value = data.Value;
                        }
                    }
                }
                
                _unitOfWork.InvestmentTargetCMSRepository.BulkUpdate(existedList);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<InvestmentTargetSetting>> UpdateSettings(int[] modelIds)
        {
            if (modelIds == null || !modelIds.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                var models = _unitOfWork.InvestmentTargetCMSRepository.GetInvestCMSModels(modelIds);
                if (models != null && models.Any())
                {
                    _unitOfWork.InvestmentTargetCMSRepository.BulkUpdate(models.ToList());
                    await _unitOfWork.SaveChangesAsync();
                }
                return models;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetId(decimal value, int portfolioId, Duration duration)
        {
            var data = await _unitOfWork.InvestmentTargetCMSRepository.GetAsync(x => x.Value == value && x.PortfolioId == portfolioId && x.Duration == duration);
            if (data != null)
            {
                return data.Id;
            }
            return 0;
        }
    }
}
