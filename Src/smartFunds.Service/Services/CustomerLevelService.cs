using AutoMapper;
using smartFunds.Business;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface ICustomerLevelService
    {
        IEnumerable<CustomerLevelModel> GetCustomerLevels(int pageSize, int pageIndex);
        CustomerLevelModel GetCustomerLevelById(int id);
        IEnumerable<CustomerLevelModel> GetAllCustomerLevel();
        Task<CustomerLevel> Save(CustomerLevelModel customerLevel);
        Task<CustomerLevel> Update(CustomerLevelModel customerLevel);
        Task DeleteCustomerLevels(int[] customerLevelIds);
        Task<bool> IsCheckSumInvest(CustomerLevelModel customerLevel);
        Task<bool> IsDuplicateName(string Name, int Id);
    }

    public class CustomerLevelService : ICustomerLevelService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerLevelManager _customerLevelManager;
        public CustomerLevelService(IMapper mapper, ICustomerLevelManager customerLevelManager)
        {
            _mapper = mapper;
            _customerLevelManager = customerLevelManager;
        }

        public async Task<bool> IsCheckSumInvest(CustomerLevelModel customerLevel)
        {
            try
            {
                var customerLevelDto = _mapper.Map<CustomerLevelModel, CustomerLevel>(customerLevel);
                return await _customerLevelManager.IsCheckSumInvest(customerLevelDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteCustomerLevels(int[] customerLevelIds)
        {
            try
            {
                await _customerLevelManager.DeleteCustomerLevels(customerLevelIds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CustomerLevelModel> GetAllCustomerLevel()
        {
            var datas = _customerLevelManager.GetAllCustomerLevel();
            if (datas == null) return null;
            return _mapper.Map<IEnumerable<CustomerLevel>, IEnumerable<CustomerLevelModel>>(datas);
        }

        public CustomerLevelModel GetCustomerLevelById(int id)
        {
            try
            {
                var customerLevelDto = _customerLevelManager.GetCustomerLevelById(id);
                return _mapper.Map<CustomerLevel, CustomerLevelModel>(customerLevelDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CustomerLevelModel> GetCustomerLevels(int pageSize, int pageIndex)
        {
            try
            {
                var customerLevelDto = _customerLevelManager.GetCustomerLevels(pageSize, pageIndex);
                if (customerLevelDto == null) return null;
                return _mapper.Map<IEnumerable<CustomerLevel>, IEnumerable<CustomerLevelModel>>(customerLevelDto);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<CustomerLevel> Save(CustomerLevelModel customerLevel)
        {
            try
            {
                var customerLevelDto = _mapper.Map<CustomerLevelModel, CustomerLevel>(customerLevel);
                return await _customerLevelManager.Save(customerLevelDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CustomerLevel> Update(CustomerLevelModel customerLevel)
        {
            try
            {
                var customerLevelDto = _mapper.Map<CustomerLevelModel, CustomerLevel>(customerLevel);
                return await _customerLevelManager.Update(customerLevelDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsDuplicateName(string Name, int Id)
        {
            try
            {
                return await _customerLevelManager.IsDuplicateName(Name, Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
