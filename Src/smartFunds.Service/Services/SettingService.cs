using AutoMapper;
using smartFunds.Common.Exceptions;
using smartFunds.Data.UnitOfWork;
using smartFunds.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface ISettingService
    {
        Task<List<Setting>> GetAll();
        Task<Setting> Save(Setting setting);
        Task<Setting> Get(string key);
    }
    public class SettingService : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SettingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Setting>> GetAll()
        {
            var settings = await _unitOfWork.SettingRepository.GetAllAsync();
            if (settings == null)
            {
                throw new NotFoundException();
            }
          
            return _mapper.Map<List<Setting>>(settings);
        }

        public async Task<Setting> Get(string key)
        {
            var setting = await _unitOfWork.SettingRepository.GetAsync(x => x.Key == key);
            if (setting == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<Setting>(setting);
        }

        public async Task<Setting> Save(Setting setting)
        {
            var existing = await _unitOfWork.SettingRepository.GetAsync(x => x.Key == setting.Key);
            if (existing != null)
            {
                throw new DuplicateRecordException();
            }
            
            var dbSetting = _mapper.Map<Data.Models.Setting>(setting);
            var savedSetting =  _unitOfWork.SettingRepository.Add(dbSetting);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Setting>(savedSetting);
        }
    }
}
