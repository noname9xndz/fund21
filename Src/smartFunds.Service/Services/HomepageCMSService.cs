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
    public interface IHomepageCMSService
    {
        List<HomepageCMSModel> GetAll();
        Task UpdateHomepageConfiguration(HomepageCMSModel homepageCMS, int category = 0, string typeUpload = "", int Id = 0);
        Task Delete(int id);
        Task AddImageConfiguration(HomepageCMSModel homepageCMS, string typeUpload = "");

    }
    public class HomepageCMSService : IHomepageCMSService
    {
        private readonly IMapper _mapper;
        private readonly IHomepageCMSManager _homepageCMSManager;

        public HomepageCMSService(IMapper mapper, IHomepageCMSManager homepageCMSManager)
        {
            _mapper = mapper;
            _homepageCMSManager = homepageCMSManager;
        }

        public async Task AddImageConfiguration(HomepageCMSModel homepageCMS, string typeUpload = "")
        {
            try
            {
                HomepageCMS model = _mapper.Map<HomepageCMS>(homepageCMS);
                await _homepageCMSManager.AddImageConfiguration(model, typeUpload);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await _homepageCMSManager.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<HomepageCMSModel> GetAll()
        {
            try
            {
                var homepageCMSDto = _homepageCMSManager.GetAll();
                return _mapper.Map<List<HomepageCMSModel>>(homepageCMSDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateHomepageConfiguration(HomepageCMSModel homepageCMS, int category = 0 ,string typeUpload = "", int Id = 0)
        {
            try
            {
                var homepageCMSDto = _mapper.Map<HomepageCMSModel, HomepageCMS>(homepageCMS);
                await _homepageCMSManager.UpdateHomepageConfiguration(homepageCMSDto, category, typeUpload, Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
