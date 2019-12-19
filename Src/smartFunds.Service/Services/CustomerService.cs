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

namespace smartFunds.Service.Services
{
    public interface ICustomerService
    {
        Task<CustomersModel> GetListCustomer(int pageSize, int pageIndex);
        Task<UserModel> GetCustomerById(string customerId);
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
        
        public async Task<UserModel> GetCustomerById(string customerId)
        {
            var user = await _customerManager.GetCustomerById(customerId);
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<CustomersModel> GetListCustomer(int pageSize, int pageIndex)
        {
            var model = new CustomersModel();
            model.Customers = (await _customerManager.GetListCustomer(pageSize, pageIndex)).Select(u => _mapper.Map<UserModel>(u)).ToList();
            return model;
        }
    }
}