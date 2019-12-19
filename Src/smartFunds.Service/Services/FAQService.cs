using AutoMapper;
using smartFunds.Business;
using smartFunds.Business.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IFAQService
    {
        Task<FAQModel> GetFAQ(int? faqId);
        Task<FAQModel> SaveFAQ(FAQModel faqModel);
        void UpdateFAQ(FAQModel faqModel);
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

        public void UpdateFAQ(FAQModel faqModel)
        {
            try
            {
                FAQ faq = _mapper.Map<FAQ>(faqModel);
                _faqManager.UpdateFAQ(faq);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
