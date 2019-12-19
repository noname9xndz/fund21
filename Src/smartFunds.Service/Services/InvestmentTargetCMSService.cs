using AutoMapper;
using smartFunds.Business.Common;
using smartFunds.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IInvestmentTargetCMSService
    {
        List<InvestmentTargetSettingModel> GetAll();
        Task<InvestmentTargetSettingModel> GetInvestmentModel(int? modelId);
        Task<IEnumerable<InvestmentTargetSettingModel>> UpdateSettings(int[] modelIds);
        Task<InvestmentTargetSettingModel> AddConfiguration(InvestmentTargetSettingModel model);
        Task UpdateConfigurations(List<InvestmentTargetSettingModel> InvestmentTargetSettings);
        Task AddListConfigurations(List<InvestmentTargetSettingModel> list);
        Task<List<InvestmentTargetSettingModel>> GetListInvestmentTargetSetting();
        Task<decimal> GetInvestmentTargetSettingValue(int portfolioId, Duration duration);
        Task<int> GetId(decimal value, int portfolioId, Duration duration);
    }
    public class InvestmentTargetCMSService : IInvestmentTargetCMSService
    {
        private readonly IMapper _mapper;
        private readonly IInvestmentTargetCMSManager _investmentTargetCMSManager;

        public InvestmentTargetCMSService(IMapper mapper, IInvestmentTargetCMSManager investmentModel)
        {
            _mapper = mapper;
            _investmentTargetCMSManager = investmentModel;
        }

        public async Task<InvestmentTargetSettingModel> AddConfiguration(InvestmentTargetSettingModel item)
        {
            try
            {
                InvestmentTargetSetting model = _mapper.Map<InvestmentTargetSetting>(item);
                InvestmentTargetSetting addedModel = await _investmentTargetCMSManager.AddConfiguration(model);
                InvestmentTargetSettingModel savedModel = _mapper.Map<InvestmentTargetSettingModel>(addedModel);
                return savedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddListConfigurations(List<InvestmentTargetSettingModel> list)
        {
            try
            {
                List<InvestmentTargetSetting> mappedModels = _mapper.Map<List<InvestmentTargetSetting>>(list);
                await _investmentTargetCMSManager.AddListConfigurations(mappedModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InvestmentTargetSettingModel> GetAll()
        {
            try
            {
                var modelDto = _investmentTargetCMSManager.GetAll();
                return _mapper.Map<List<InvestmentTargetSettingModel>>(modelDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<InvestmentTargetSettingModel> GetInvestmentModel(int? modelId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetId(decimal value, int portfolioId, Duration duration)
        {
            return await _investmentTargetCMSManager.GetId(value, portfolioId, duration);
        }

        public async Task<decimal> GetInvestmentTargetSettingValue(int portfolioId, Duration duration)
        {
            return await _investmentTargetCMSManager.GetInvestmentTargetSettingValue(portfolioId, duration);
        }

        public async Task<List<InvestmentTargetSettingModel>> GetListInvestmentTargetSetting()
        {
            try
            {
                var list = await _investmentTargetCMSManager.GetListInvestmentTargetSetting();
                return _mapper.Map<List<InvestmentTargetSettingModel>>(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateConfigurations(List<InvestmentTargetSettingModel> models)
        {
            try
            {
                List<InvestmentTargetSetting> list = _mapper.Map<List<InvestmentTargetSettingModel>, List<InvestmentTargetSetting>>(models);
                //foreach (var itm in list)
                //{
                //    itm.Id = 0;
                //}
                await _investmentTargetCMSManager.UpdateConfigurations(list);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<InvestmentTargetSettingModel>> UpdateSettings(int[] modelIds)
        {
            try
            {
                IEnumerable<InvestmentTargetSetting> investmentTargetSetting = await _investmentTargetCMSManager.UpdateSettings(modelIds);
                IEnumerable<InvestmentTargetSettingModel> investmentTargetSettingModels = _mapper.Map<IEnumerable<InvestmentTargetSetting>, IEnumerable<InvestmentTargetSettingModel>>(investmentTargetSetting);
                return investmentTargetSettingModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
