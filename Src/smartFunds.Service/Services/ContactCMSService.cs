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
    public interface IContactCMSService
    {
        ContactCMSModel GetContactConfiguration();
        Task<ContactCMS> UpdateContactCMS(ContactCMSModel contactCMSModel);
        Task<ContactCMSModel> AddDefault(ContactCMSModel ContactModel);
    }
    public class ContactCMSService : IContactCMSService
    {
        private readonly IMapper _mapper;
        private readonly IContactCMSManager _contactCMSManager;

        public ContactCMSService(IMapper mapper, IContactCMSManager contactCMSManager)
        {
            _mapper = mapper;
            _contactCMSManager = contactCMSManager;
        }

        public ContactCMSModel GetContactConfiguration()
        {
            try
            {
                var contactConfigurationDto = _contactCMSManager.GetContactConfiguration();
                return _mapper.Map<ContactCMSModel>(contactConfigurationDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ContactCMSModel> AddDefault(ContactCMSModel ContactModel)
        {
            try
            {
                ContactCMS model = _mapper.Map<ContactCMS>(ContactModel);
                ContactCMS addedModel = await _contactCMSManager.AddDefault(model);
                ContactCMSModel savedModel = _mapper.Map<ContactCMSModel>(addedModel);
                return savedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ContactCMS> UpdateContactCMS(ContactCMSModel contactCMSModel)
        {
            try
            {
                var contactCMSDto = _mapper.Map<ContactCMSModel, ContactCMS>(contactCMSModel);
                return await _contactCMSManager.UpdateContactConfiguration(contactCMSDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
