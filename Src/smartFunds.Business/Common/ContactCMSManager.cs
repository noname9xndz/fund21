using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using smartFunds.Common.Exceptions;

namespace smartFunds.Business.Common
{
    public interface IContactCMSManager
    {
        ContactCMS GetContactConfiguration();

        Task<ContactCMS> UpdateContactConfiguration(ContactCMS contactCMS);
        Task<ContactCMS> AddDefault(ContactCMS model);
    }

    public class ContactCMSManager : IContactCMSManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactCMSManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ContactCMS GetContactConfiguration()
        {
            return _unitOfWork.ContactCMSRepository.GetContactConfiguration();
        }

        public async Task<ContactCMS> AddDefault(ContactCMS model)
        {
            try
            {
                var savedModel = _unitOfWork.ContactCMSRepository.Add(model);
                await _unitOfWork.SaveChangesAsync();
                return savedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ContactCMS> UpdateContactConfiguration(ContactCMS contactCMS)
        {
            try
            {
                if (contactCMS == null) throw new InvalidParameterException();
                _unitOfWork.ContactCMSRepository.Update(contactCMS);
                await _unitOfWork.SaveChangesAsync();
                return contactCMS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
