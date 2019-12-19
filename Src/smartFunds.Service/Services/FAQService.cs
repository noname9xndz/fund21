using AutoMapper;
using smartFunds.Business.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using smartFunds.Common;
using Microsoft.AspNetCore.Http;

namespace smartFunds.Service.Services
{
    public interface IFAQService
    {
        Task<FAQModel> GetFAQ (int? faqId);
        Task<FAQModel> SaveFAQ (FAQModel faqModel);
        Task UpdateFAQ (FAQModel faqModel);
        Task DeleteFAQ (int? faqId);
        Task DeleteFAQs(int[] faqIds);
        Task<List<FAQModel>> GetFAQsByCategory(FAQCategory category);
        Task<List<FAQModel>> GetFAQsByCategory(FAQCategory category, string searchValue);
        Task<List<FAQModel>> GetFAQs();
        Task<List<FAQModel>> GetAllFAQs();
        Task<bool> ImportFAQs(IFormFile file);
        Task<byte[]> ExportFAQs();
        byte[] ExportExampleFile();
        FAQCategory IsMapData(string text);
    }
    public class FAQService : IFAQService
    {
        private readonly IMapper _mapper;
        private readonly IFAQManager _faqManager;
        public FAQService(IMapper mapper, IFAQManager faqManager)
        {
            _mapper = mapper;
            _faqManager = faqManager;
        }        

        public async Task<FAQModel> GetFAQ(int? faqId)
        {            
            try
            {
                FAQ faq = await _faqManager.GetFAQ(faqId);
                FAQModel faqModel = _mapper.Map<FAQModel>(faq);
                return faqModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        public async Task<List<FAQModel>> GetFAQsByCategory(FAQCategory category)
        {
            try
            {
                List<FAQ> faqs = await _faqManager.GetFAQsByCategory(category);
                List<FAQModel> faqsModels = _mapper.Map<List<FAQ>, List<FAQModel>>(faqs);
                return faqsModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FAQModel>> GetFAQsByCategory(FAQCategory category, string searchValue)
        {
            try
            {
                var faqs = await _faqManager.GetFAQsByCategory(category, searchValue);
                List<FAQModel> faqsModels = _mapper.Map<List<FAQModel>>(faqs);
                return faqsModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FAQModel> SaveFAQ(FAQModel faqModel)
        {
            try
            {
                FAQ faq = _mapper.Map<FAQ>(faqModel);
                FAQ savedFAQ = await _faqManager.SaveFAQ(faq);
                FAQModel savedFAQModel = _mapper.Map<FAQModel>(savedFAQ);
                return savedFAQModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public async Task UpdateFAQ(FAQModel faqModel)
        {
            try
            {
                FAQ faq = _mapper.Map<FAQ>(faqModel);
                await _faqManager.UpdateFAQ(faq);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteFAQ(int? faqId)
        {
            try
            {
                await _faqManager.DeleteFAQ(faqId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteFAQs(int[] faqIds)
        {
            try
            {
                await _faqManager.DeleteFAQs(faqIds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FAQModel>> GetFAQs()
        {
            try
            {
                List<FAQ> faqs = await _faqManager.GetFAQs();
                List<FAQModel> faqsModels = _mapper.Map<List<FAQ>, List<FAQModel>>(faqs);
                return faqsModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FAQModel>> GetAllFAQs()
        {
            try
            {
                var faqs = await _faqManager.GetAllFAQs();
                List<FAQModel> faqsModels = _mapper.Map<List<FAQModel>>(faqs);
                return faqsModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ImportFAQs(IFormFile file)
        {
            try
            {
               bool result = await _faqManager.ImportFAQs(file);
                if (!result)
                    return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<byte[]> ExportFAQs()
        {
            try
            {
                return await _faqManager.ExportFAQs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FAQCategory IsMapData(string text)
        {
           return _faqManager.IsMapData(text);
        }

        public byte[] ExportExampleFile()
        {
            try
            {
                return _faqManager.ExportExampleFile();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
