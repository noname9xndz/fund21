using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using smartFunds.Model.Common;
using System.Threading.Tasks;
using AutoMapper;
using smartFunds.Business.Admin;
using smartFunds.Data.Models;
using Microsoft.AspNetCore.Identity;
using smartFunds.Common;
using System.Collections.Generic;
using smartFunds.Model.Admin;

namespace smartFunds.Service.Services
{
    public interface ICustomerService
    {
        Task<CustomersModel> GetListCustomer(int pageSize, int pageIndex, SearchCustomer searchCustomer = null);
        Task<CustomersModel> GetAllCustomer(SearchCustomer searchCustomer = null);
        Task<UserModel> GetCustomerById(string customerId);
        Task<RegisterStatus> AddCustomer(UserModel userModel, string password);
        Task<bool> UpdateCustomer(UserModel customer);
        Task<bool> DeleteCustomerById(string customerId);
        Task<bool> DeleteCustomerByIds(List<string> customerIds);
        Task<byte[]> ExportCustomer(SearchCustomer searchCustomer = null);
    }
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerManager _customerManager;

        public CustomerService(IMapper mapper, ICustomerManager customerManager)
        {
            _mapper = mapper;
            _customerManager = customerManager;

        }

        public async Task<RegisterStatus> AddCustomer(UserModel userModel, string password)
        {
            var user = _mapper.Map<User>(userModel);
            return await _customerManager.AddCustomer(user, password);
        }

        public async Task<bool> UpdateCustomer(UserModel customer)
        {
            var user = _mapper.Map<User>(customer);
            return await _customerManager.UpdateCustomer(user);
        }

        public async Task<UserModel> GetCustomerById(string customerId)
        {
            var user = await _customerManager.GetCustomerById(customerId);
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<bool> DeleteCustomerById(string customerId)
        {
            return await _customerManager.DeleteCustomerById(customerId);
        }

        public async Task<bool> DeleteCustomerByIds(List<string> customerIds)
        {
            return await _customerManager.DeleteCustomerByIds(customerIds);
        }

        public async Task<CustomersModel> GetListCustomer(int pageSize, int pageIndex, SearchCustomer searchCustomer = null)
        {
            var model = new CustomersModel();
            var listUser = (await _customerManager.GetListCustomer(pageSize, pageIndex, searchCustomer)).ToList();
            var listUserModel = _mapper.Map<List<User>, List<UserModel>>(listUser);
            model.Customers = listUserModel;
            model.TotalCount = (await _customerManager.GetAllCustomer(searchCustomer)).Count;
            return model;
        }

        public async Task<CustomersModel> GetAllCustomer(SearchCustomer searchCustomer = null)
        {
            var model = new CustomersModel();
            var allUserModel = _mapper.Map<List<User>, List<UserModel>>(await _customerManager.GetAllCustomer(searchCustomer));
            model.Customers = allUserModel;
            model.TotalCount = allUserModel.Count;
            return model;
        }

        public async Task<byte[]> ExportCustomer(SearchCustomer searchCustomer = null)
        {
            return await _customerManager.ExportCustomer(searchCustomer);
        }
    }
}