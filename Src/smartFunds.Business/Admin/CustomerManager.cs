using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Business.Admin
{
    public interface ICustomerManager
    {
        Task<List<User>> GetAllCustomer(SearchCustomer searchCustomer = null);
        Task<List<User>> GetListCustomer(int pageSize, int pageIndex, SearchCustomer searchCustomer = null);
        Task<User> GetCustomerById(string customerId);
        Task<RegisterStatus> AddCustomer(User user, string password);
        Task<bool> UpdateCustomer(User customer);
        Task<bool> DeleteCustomerById(string customerId);
        Task<bool> DeleteCustomerByIds(List<string> customerIds);
        Task<byte[]> ExportCustomer(SearchCustomer searchCustomer = null);
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

        public async Task<RegisterStatus> AddCustomer(User user, string password)
        {
            if (user == null || string.IsNullOrWhiteSpace(password))
            {
                throw new ApplicationException("Can't register user with name " + user.UserName);
            }
            var existUser = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == user.UserName
                                                                 || (u.Email != null && u.Email == user.Email)
                                                                 || (u.PhoneNumber != null && u.PhoneNumber == user.PhoneNumber));
            if (existUser != null)
            {
                if (existUser.IsDeleted)
                {
                    var deletedUser = await _appUserManager.FindByIdAsync(existUser.Id);
                    await _appUserManager.RemoveFromRoleAsync(deletedUser, RoleName.Customer);
                    var deleteCustomer = await _appUserManager.DeleteAsync(deletedUser);
                    if (!deleteCustomer.Succeeded)
                    {
                        return RegisterStatus.None;
                    }
                }
                else if (existUser.EmailConfirmed || existUser.PhoneNumberConfirmed)
                {
                    return RegisterStatus.ExistUser;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(existUser.Email))
                    {
                        existUser.EmailConfirmed = true;
                    }
                    if (!string.IsNullOrWhiteSpace(existUser.PhoneNumber))
                    {
                        existUser.PhoneNumberConfirmed = true;
                    }

                    var updateUser = await _appUserManager.UpdateAsync(existUser);

                    if (updateUser.Succeeded)
                    {
                        var removePassword = await _appUserManager.RemovePasswordAsync(existUser);
                        if (removePassword.Succeeded)
                        {
                            var addPassword = await _appUserManager.AddPasswordAsync(existUser, password);
                            if (addPassword.Succeeded)
                            {
                                return RegisterStatus.Succeeded;
                            }
                        }
                    }
                }
            }

            user = _unitOfWork.UserRepository.InitBasicInfo(user);

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                user.EmailConfirmed = true;
            }

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                user.PhoneNumberConfirmed = true;
            }

            var result = await _appUserManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var addToCustomerRole = await _appUserManager.AddToRoleAsync(user, RoleName.Customer);
                return RegisterStatus.Succeeded;
            }
            return RegisterStatus.None;
        }

        public async Task<bool> UpdateCustomer(User customer)
        {
            if (customer == null)
            {
                throw new ApplicationException("Update Customer: customer is null");
            }

            try
            {
                _unitOfWork.UserRepository.Update(customer);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<User> GetCustomerById(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return null;
            }

            return await _appUserManager.FindByIdAsync(customerId);
        }

        public async Task<List<User>> GetAllCustomer(SearchCustomer searchCustomer = null)
        {
            var predicate = SetPredicate(searchCustomer);
            var listCustomer = (await _appUserManager.GetUsersInRoleAsync(RoleName.Customer)).Where(u => !u.IsDeleted).Where(predicate).ToList();
            //Get all KVRR
            var kvrr = _unitOfWork.KVRRRepository.GetAllKVRR();
            foreach (var customer in listCustomer)
            {
                customer.KVRR = kvrr.FirstOrDefault(h => h.Id == customer.KVRRId);
            }
            //var listCustomer = (await _unitOfWork.UserRepository.FindByAsync(u => !u.IsDeleted && u., "KVRR"))
            //                    .Where(predicate).OrderByDescending(u => u.Created)
            //                    .ToList().Where(u=> _appUserManager.IsInRoleAsync(u, RoleName.Customer).Result).ToList();
            return listCustomer.ToList();
        }

        public async Task<List<User>> GetListCustomer(int pageSize, int pageIndex, SearchCustomer searchCustomer = null)
        {
            if (pageSize < 1 || pageIndex < 1)
            {
                return null;
            }

            var predicate = SetPredicate(searchCustomer);

            var listCustomer = (await _unitOfWork.UserRepository.FindByAsync(u => !u.IsDeleted && _appUserManager.IsInRoleAsync(u, RoleName.Customer).Result))
                                .Where(predicate).OrderByDescending(u => u.Created)
                                .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return listCustomer;
        }

        public async Task<byte[]> ExportCustomer(SearchCustomer searchCustomer = null)
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.FullName,
                    Model.Resources.Common.PhoneNumber,
                    Model.Resources.Common.InitialInvestmentAmount,
                    Model.Resources.Common.CurrentAccountAmount,
                    Model.Resources.Common.AccountCreated,
                    Model.Resources.Common.ActiveStatus
                };

                var predicate = SetPredicate(searchCustomer);

                var listCustomer = (await _unitOfWork.UserRepository.FindByAsync(u => !u.IsDeleted && _appUserManager.IsInRoleAsync(u, RoleName.Customer).Result))
                                    .Where(predicate).OrderByDescending(u => u.Created)
                                    .ToList();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Customers");
                    using (var cells = worksheet.Cells[1, 1, 1, 6])
                    {
                        cells.Style.Font.Bold = true;
                    }

                    for (var i = 0; i < comlumHeadrs.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    }

                    var j = 2;
                    foreach (var customer in listCustomer)
                    {
                        worksheet.Cells["A" + j].Value = customer.FullName;
                        worksheet.Cells["B" + j].Value = customer.PhoneNumber;
                        worksheet.Cells["C" + j].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["C" + j].Value = customer.InitialInvestmentAmount;
                        worksheet.Cells["D" + j].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["D" + j].Value = customer.CurrentAccountAmount;
                        worksheet.Cells["E" + j].Value = customer.Created.ToString("dd/MM/yyyy");
                        worksheet.Cells["F" + j].Value = (DateTime.Now - customer.LastLogin).Days < 365 ? Model.Resources.Common.Active : Model.Resources.Common.Inactive;
                        j++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export Customer: " + ex.Message);
            }

        }

        private ExpressionStarter<User> SetPredicate(SearchCustomer searchCustomer)
        {
            var predicate = PredicateBuilder.New<User>(true);

            if (searchCustomer != null)
            {
                if (!string.IsNullOrWhiteSpace(searchCustomer.FullName))
                {
                    var fullName = searchCustomer.FullName.Trim().ToLower();
                    predicate = predicate.And(u => !string.IsNullOrWhiteSpace(u.FullName) && u.FullName.ToLower().Contains(fullName));
                }
                if (!string.IsNullOrWhiteSpace(searchCustomer.PhoneNumber))
                {
                    var phone = searchCustomer.PhoneNumber.Trim().ToLower();
                    predicate = predicate.And(u => !string.IsNullOrWhiteSpace(u.PhoneNumber) && u.PhoneNumber.Contains(phone));
                }
                if (!string.IsNullOrWhiteSpace(searchCustomer.Email))
                {
                    var email = searchCustomer.Email.Trim().ToLower();
                    predicate = predicate.And(u => !string.IsNullOrWhiteSpace(u.Email) && u.Email.ToLower().Contains(email));
                }
                var created = new DateTime();
                if (!string.IsNullOrWhiteSpace(searchCustomer.CreatedDate) && DateTime.TryParseExact(searchCustomer.CreatedDate, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out created))
                {
                    predicate = predicate.And(u => u.Created.Date == created);
                }
                if (searchCustomer.ActiveStatus == ActiveStatus.Active)
                {
                    predicate = predicate.And(u => (DateTime.Now - u.LastLogin).Days < 365);
                }
                if (searchCustomer.ActiveStatus == ActiveStatus.Inactive)
                {
                    predicate = predicate.And(u => (DateTime.Now - u.LastLogin).Days >= 365);
                }
            }

            return predicate;
        }

        public async Task<bool> DeleteCustomerById(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return false;
            }

            var customer = await GetCustomerById(customerId);
            if (customer != null && _appUserManager.IsInRoleAsync(customer, RoleName.Customer).Result)
            {
                _unitOfWork.UserRepository.Delete(customer);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteCustomerByIds(List<string> customerIds)
        {
            if (customerIds == null || !customerIds.Any())
            {
                return false;
            }

            var listCustomer = await _unitOfWork.UserRepository.FindByAsync(u => !u.IsDeleted && customerIds.Contains(u.Id));
            
            if (listCustomer != null && listCustomer.Any())
            {
                _unitOfWork.UserRepository.BulkDelete(listCustomer.ToList());
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
