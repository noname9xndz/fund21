using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Business.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;

namespace smartFunds.Business
{
    /*
     * Creator by PhuongNC
     */
    public interface ICustomerLevelManager
    {
        IEnumerable<CustomerLevel> GetCustomerLevels(int pageSize, int pageIndex);
        CustomerLevel GetCustomerLevelById(int id);
        IEnumerable<CustomerLevel> GetAllCustomerLevel();
        Task<CustomerLevel> Save(CustomerLevel customerLevel);
        Task<CustomerLevel> Update(CustomerLevel customerLevel);
        Task DeleteCustomerLevels(int[] customerLevelIds);
        Task<bool> IsCheckSumInvest(CustomerLevel customerLevel);
        Task<bool> IsDuplicateName(string newValue, int Id);
    }

    public class CustomerLevelManager : ICustomerLevelManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;

        public CustomerLevelManager(IUnitOfWork unitOfWork, IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<bool> IsCheckSumInvest(CustomerLevel customerLevel)
        {
            if (customerLevel == null) throw new InvalidParameterException();

            if (customerLevel.MaxMoney == 0) customerLevel.MaxMoney = smartFunds.Common.Constants.MaxDecimal.MaxMoney;
            if (customerLevel.IDCustomerLevel != 0)
            {
                var customerLevelEdited = await _unitOfWork.CustomerLevelRepository.GetAsync(m => m.IDCustomerLevel == customerLevel.IDCustomerLevel);

                if (customerLevel.MinMoney == customerLevelEdited.MinMoney && customerLevel.MaxMoney == customerLevelEdited.MaxMoney) return true;

                var isValueEditExisted = await _unitOfWork.CustomerLevelRepository.FindByAsync(x => x.MinMoney <= customerLevel.MinMoney && x.MaxMoney >= customerLevel.MinMoney && customerLevelEdited.IDCustomerLevel != x.IDCustomerLevel && x.IsDeleted == false);

                if (isValueEditExisted != null && isValueEditExisted.Any())  return false;

                isValueEditExisted = await _unitOfWork.CustomerLevelRepository.FindByAsync(x => x.MinMoney <= customerLevel.MaxMoney && x.MaxMoney >= customerLevel.MaxMoney && customerLevelEdited.IDCustomerLevel != x.IDCustomerLevel && x.IsDeleted == false);

                if (isValueEditExisted != null && isValueEditExisted.Any())  return false;
            }
            else
            {
                var isValueExisted = await _unitOfWork.CustomerLevelRepository.FindByAsync(x => x.MinMoney <= customerLevel.MinMoney && x.MaxMoney >= customerLevel.MinMoney && x.IsDeleted == false);

                if (isValueExisted != null && isValueExisted.Any()) return false;

                isValueExisted = await _unitOfWork.CustomerLevelRepository.FindByAsync(x => x.MinMoney <= customerLevel.MaxMoney && x.MaxMoney >= customerLevel.MaxMoney && x.IsDeleted == false);

                if (isValueExisted != null && isValueExisted.Any())  return false;
            }
            return true;
        }

        public async Task DeleteCustomerLevels(int[] customerLevelIds)
        {
            if (customerLevelIds == null || !customerLevelIds.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                var funds = _unitOfWork.CustomerLevelRepository.GetCustomerLevelsByIds(customerLevelIds);
                if (funds != null && funds.Any())
                {
                    _unitOfWork.CustomerLevelRepository.BulkDelete(funds.ToList());
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CustomerLevel> GetAllCustomerLevel()
        {
            return _unitOfWork.CustomerLevelRepository.GetAllAsync().Result;
        }

        public CustomerLevel GetCustomerLevelById(int id)
        {
            var customLever = _unitOfWork.CustomerLevelRepository.GetAsync(m => m.IDCustomerLevel == id);
            return customLever.Result;// _unitOfWork.CustomerLevelRepository.GetCustomerLevelById(id);
        }

        public IEnumerable<CustomerLevel> GetCustomerLevels(int pageSize, int pageIndex)
        {
            if (pageSize < 0 || pageIndex < 0) throw new InvalidParameterException();
            if (pageSize == 0 && pageIndex == 0) return _unitOfWork.CustomerLevelRepository.GetAllCustomerLevel()?.ToList();

            return _unitOfWork.CustomerLevelRepository.GetAllCustomerLevel()?.Take(pageSize).Skip((pageIndex - 1) * pageSize).ToList();
        }

        public async Task<CustomerLevel> Save(CustomerLevel customerLevel)
        {
            try
            {
                if (customerLevel == null) throw new InvalidParameterException();

                customerLevel.DateLastUpdated = DateTime.Now;
                customerLevel.LastUpdatedBy = _userManager.CurrentUser();

                customerLevel = _unitOfWork.CustomerLevelRepository.Add(customerLevel);

                await _unitOfWork.SaveChangesAsync();
                return customerLevel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CustomerLevel> Update(CustomerLevel customerLevel)
        {
            try
            {
                if (customerLevel == null) throw new InvalidParameterException();

               _unitOfWork.CustomerLevelRepository.Update(customerLevel);
                await _unitOfWork.SaveChangesAsync();

                return customerLevel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsDuplicateName(string newValue, int Id)
        {
            if (string.IsNullOrEmpty(newValue)) return false;

            if(Id != 0)
            {
                var customerLeverChecked = await _unitOfWork.CustomerLevelRepository.GetAsync(m => m.IDCustomerLevel == Id);
                if (customerLeverChecked.NameCustomerLevel.ToLower().Equals(newValue.ToLower())) return true;
            }
            var isValueExisted = await _unitOfWork.CustomerLevelRepository.FindByAsync(x => x.NameCustomerLevel.ToLower().Equals(newValue.ToLower()));
            if (isValueExisted != null && isValueExisted.Any())
                return false;

            return true;
        }
    }
}
