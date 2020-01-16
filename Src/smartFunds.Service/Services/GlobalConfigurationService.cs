
using System;
using System.Threading.Tasks;
using AutoMapper;
using smartFunds.Business.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;

namespace smartFunds.Service.Services
{
    public interface IGlobalConfigurationService
    {
        Task<GlobalConfigurationModel> GetConfig(string name);
        Task<GlobalConfigurationModel> SaveConfig(GlobalConfigurationModel config);
        Task UpdateConfig(GlobalConfigurationModel config);
        Task<string> GetValueConfig(string name);
        Task SetValueConfig(string name, string value);
    }
    public class GlobalConfigurationService : IGlobalConfigurationService
    {
        private readonly IMapper _mapper;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        public GlobalConfigurationService(IMapper mapper, IGlobalConfigurationManager globalConfiguration)
        {
            _mapper = mapper;
            _globalConfigurationManager = globalConfiguration;
        }

        public async Task<GlobalConfigurationModel> GetConfig(string name)
        {
            try
            {
                var config = await _globalConfigurationManager.GetConfig(name);
               var model =  _mapper.Map<GlobalConfiguration, GlobalConfigurationModel>(config);
               return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GlobalConfigurationModel> SaveConfig(GlobalConfigurationModel config)
        {
            GlobalConfiguration newConfig = _mapper.Map<GlobalConfiguration>(config);
            GlobalConfiguration savedConfig = await _globalConfigurationManager.SaveConfig(newConfig);
            GlobalConfigurationModel savedConfigModel = _mapper.Map<GlobalConfigurationModel>(savedConfig);
            return savedConfigModel;
        }

        public async Task UpdateConfig(GlobalConfigurationModel config)
        {
            var newConfig = _mapper.Map<GlobalConfiguration>(config);
            await _globalConfigurationManager.UpdateConfig(newConfig);
        }

        public async Task<string> GetValueConfig(string name)
        {
            var model = await _globalConfigurationManager.GetValueConfig(name);
            var config = _mapper.Map<GlobalConfiguration, GlobalConfigurationModel>(model);
            return config.Value;

        }

        public async Task SetValueConfig(string name, string value)
        {
            await _globalConfigurationManager.SetValueConfig(name, value);
        }
    }
}
