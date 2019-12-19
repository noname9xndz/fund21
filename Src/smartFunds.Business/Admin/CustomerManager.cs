using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Business.Admin
{
    public interface ICustomerManager
    {        
        Task<List<User>> GetListCustomer(int pageSize, int pageIndex);
        Task<User> GetCustomerById(string customerId);
    }

    public class CustomerManager : ICustomerManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _appUserManager;

        public CustomerManager(IUnitOfWork unitOfWork, UserManager<User> appUserManager)
        {
            _unitOfWork = unitOfWork;
            _appUserManager = appUserManager;
        }

        public async Task<User> GetCustomerById(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return null;
            }

            return await _appUserManager.FindByIdAsync(customerId);
        }

        public async Task<List<User>> GetListCustomer(int pageSize, int pageIndex)
        {
            if(pageSize < 0 || pageIndex < 1)
            {
                return null;
            }

            var listCustomer = (await _unitOfWork.UserRepository.FindByAsync(u => _appUserManager.IsInRoleAsync(u, RoleName.Customer).Result))
                                .Take(pageSize).Skip((pageIndex - 1)*pageSize).ToList();
            return listCustomer;
        }
    }
}
