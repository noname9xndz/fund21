using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Business.Common
{
    public interface IFAQManager
    {
        Task<FAQ> GetFAQ(int? faqId);
        Task<List<FAQ>> GetFAQs(int? pageSize, int? pageIndex);
        Task<FAQ> SaveFAQ(FAQ faq);
        void UpdateFAQ(FAQ faq);
        Task<bool> DeleteFAQ(int? faqId);
    }
    public class FAQManager : IFAQManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;

        public FAQManager(IUnitOfWork unitOfWork, IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<FAQ> GetFAQ(int? faqId)
        {
            if (faqId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var faq = await _unitOfWork.FAQRepository.GetAsync(m => m.Id == faqId);
                if (faq != null)
                {
                    return faq;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<FAQ>> GetFAQs(int? pageSize, int? pageIndex)
        {
            //if (pageSize == null)
            //{
            //    pageSize = 10;
            //}
            //if (pageIndex == null)
            //{
            //    pageIndex = 1;
            //}
            //var faqs = _unitOfWork.FAQRepository.GetAllAsync("FAQ");
            //var result = faqs.Result.;
            //return result;
            throw new NotImplementedException();
        }

        public async Task<FAQ> SaveFAQ(FAQ faq)
        {
            try
            {
                faq.DateLastUpdated = DateTime.Now;
                faq.LastUpdatedBy = _userManager.CurrentUser();
                var savedFAQ = _unitOfWork.FAQRepository.Add(faq);
                await _unitOfWork.SaveChangesAsync();
                return savedFAQ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateFAQ(FAQ faq)
        {
            try
            {
                faq.DateLastUpdated = DateTime.Now;
                faq.LastUpdatedBy = _userManager.CurrentUser();
                _unitOfWork.FAQRepository.Update(faq);
                _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> DeleteFAQ(int? faqId)
        {
            throw new NotImplementedException();
        }
    }
}
