using AutoMapper;
using smartFunds.Business.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IIntroducingSettingService
    {
        GenericIntroducingSettingModel GetSetting();

        GenericIntroducingSetting UpdateIntroducingSetting(GenericIntroducingSettingModel GenericIntroducingSetting,int isDeleteMobile,int isDeleteDesktop);

        Task AddDefault(GenericIntroducingSettingModel GenericIntroducingSetting);
    }
    public class IntroducingSettingService : IIntroducingSettingService
    {
        private readonly IMapper _mapper;
        private readonly IIntroducingSettingManager _genericIntroducingSettingManager;

        public IntroducingSettingService(IMapper mapper, IIntroducingSettingManager GenericIntroducingSettingManager)
        {
            _mapper = mapper;
            _genericIntroducingSettingManager = GenericIntroducingSettingManager;
        }

        public async Task AddDefault(GenericIntroducingSettingModel genericIntroducingSettingModel)
        {
            try
            {
                var genericIntroducingSettingDto = _mapper.Map<GenericIntroducingSetting>(genericIntroducingSettingModel);
                await _genericIntroducingSettingManager.AddDefault(genericIntroducingSettingDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GenericIntroducingSettingModel GetSetting()
        {
            try
            {
                var introducingSettingDto = _genericIntroducingSettingManager.GetSetting();
                return _mapper.Map<GenericIntroducingSettingModel>(introducingSettingDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GenericIntroducingSetting UpdateIntroducingSetting(GenericIntroducingSettingModel GenericIntroducingSettingModel, int isDeleteMobile, int isDeleteDesktop)
        {
            try
            {
                var GenericIntroducingSettingDto = _mapper.Map<GenericIntroducingSetting>(GenericIntroducingSettingModel);
                return _genericIntroducingSettingManager.UpdateIntroducingSetting(GenericIntroducingSettingDto, isDeleteMobile, isDeleteDesktop);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
