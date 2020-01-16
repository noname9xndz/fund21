using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using smartFunds.Common.Exceptions;
using static smartFunds.Common.Constants;

namespace smartFunds.Business.Common
{
    public interface IUserManager
    {
        Task UpdateUser(User user);
        Task<RegisterStatus> RegisterUser(User user, string password, string roleName);
        Task<string> GenerateEmailConfirmationToken(User user);
        Task<string> GeneratePhoneConfirmationToken(string userName, string phoneNumber);
        Task<string> GeneratePasswordResetToken(User user);
        Task<LoginStatus> LoginUser(string userName, string password, bool isRemember, bool lockoutOnFailure);
        Task<LoginStatus> LoginUserByEmailOrPhone(string emailOrPhone, string password, bool isRemember, bool lockoutOnFailure);
        Task<bool> ConfirmEmail(string userId, string code);
        Task<bool> ConfirmPhone(string userName, string code, string phoneNumber);
        Task<User> GetUserById(string userId);
        Task<User> GetUserByName(string userName);
        Task<User> GetUserByEmailOrPhone(string emailOrPhone);
        Task LogoutUser();
        Task<bool> IsInRole(User user, string roleName);
        string GetRoleName(User user);
        Task<bool> ResetPassword(string userName, string code, string newPassword);
        Task<bool> ResetPasswordByVerifyCode(string userName, string code, string phoneNumber, string newPassword);
        string CurrentUser();
        Task<User> GetCurrentUser();
        Task<bool> DeleteUserById(string userId);
        Task<bool> DeleteUserByIds(List<string> userIds);
        bool IsSignedIn(ClaimsPrincipal user);
        Task<bool> ChangePassword(string userName, string currentPassword, string newPassword);
        Task<List<User>> GetAllUserByRoles(params string[] roles);
        Task<List<User>> GetUsersByRoles(int pageSize, int pageIndex, params string[] roles);
        Task<List<UserPorfolio>> GetUserPorfolio(string userId = null);
        Task<User> GetUserRelateData();
        Task ConfirmKVRR(int kvrrId);
        Task<bool> Login(string userName);
        Task UpdateSecurityStamp(string userName);
    }

    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _appUserManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManager(IUnitOfWork unitOfWork, UserManager<User> appUserManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _appUserManager = appUserManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentUser()
        {
            return _httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                ? _httpContextAccessor.HttpContext.User.Identity.Name
                : "Unknown";
        }

        public async Task<User> GetCurrentUser()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return await GetUserByName(_httpContextAccessor.HttpContext.User.Identity.Name);
            }

            return null;
        }

        public async Task UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ApplicationException("Update User: user is null");
            }

            try
            {
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<LoginStatus> LoginUser(string userName, string password, bool isRemember, bool lockoutOnFailure)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return LoginStatus.None;
            }
            var user = await _appUserManager.FindByNameAsync(userName);

            if (user != null && !user.IsDeleted && (await _appUserManager.IsEmailConfirmedAsync(user) || await _appUserManager.IsPhoneNumberConfirmedAsync(user)))
            {
                var result = await _signInManager.PasswordSignInAsync(userName, password, isRemember, lockoutOnFailure);
                if (result.Succeeded)
                {
                    user.LastLogin = DateTime.Now;
                    await _appUserManager.UpdateAsync(user);
                    return LoginStatus.Succeeded;
                }
                if (result.RequiresTwoFactor)
                {
                    return LoginStatus.RequiresTwoFactor;
                }
                if (result.IsLockedOut)
                {
                    return LoginStatus.IsLockedOut;
                }
            }

            return LoginStatus.None;
        }

        public async Task<LoginStatus> LoginUserByEmailOrPhone(string emailOrPhone, string password, bool isRemember, bool lockoutOnFailure)
        {
            if (string.IsNullOrWhiteSpace(emailOrPhone) || string.IsNullOrWhiteSpace(password))
            {
                return LoginStatus.None;
            }
            var user = await GetUserByEmailOrPhone(emailOrPhone);

            if (user != null && ((emailOrPhone.IsEmail() && await _appUserManager.IsEmailConfirmedAsync(user)) || (emailOrPhone.IsPhoneNumber() && await _appUserManager.IsPhoneNumberConfirmedAsync(user))))
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, password, isRemember, lockoutOnFailure);
                if (result.Succeeded)
                {
                    user.LastLogin = DateTime.Now;
                    await _appUserManager.UpdateAsync(user);
                    return LoginStatus.Succeeded;
                }
                if (result.RequiresTwoFactor)
                {
                    return LoginStatus.RequiresTwoFactor;
                }
                if (result.IsLockedOut)
                {
                    return LoginStatus.IsLockedOut;
                }
            }
            else
            {
                return LoginStatus.NotVerify;
            }

            return LoginStatus.None;
        }

        public async Task LogoutUser()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterStatus> RegisterUser(User user, string password, string roleName)
        {
            if (user == null || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(roleName))
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
                    await _appUserManager.RemoveFromRolesAsync(deletedUser, _appUserManager.GetRolesAsync(deletedUser).Result);
                    var deleteUser = await _appUserManager.DeleteAsync(deletedUser);
                    if (!deleteUser.Succeeded)
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
                    return RegisterStatus.NotVerify;
                }
            }

            user = _unitOfWork.UserRepository.InitBasicInfo(user);

            var result = await _appUserManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var addToCustomerRole = await _appUserManager.AddToRoleAsync(user, roleName);
                return RegisterStatus.Succeeded;
            }
            return RegisterStatus.None;
        }

        public async Task<string> GenerateEmailConfirmationToken(User user)
        {
            if (user == null)
            {
                return string.Empty;
            }
            return await _appUserManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePhoneConfirmationToken(string userName, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return string.Empty;
            }
            var user = await _appUserManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new ApplicationException(string.Format("User {0} not found", userName));
            }
            return await _appUserManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        }

        public async Task<string> GeneratePasswordResetToken(User user)
        {
            if (user == null)
            {
                return string.Empty;
            }
            return await _appUserManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            var user = await _appUserManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await _appUserManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                user.LastLogin = DateTime.Now;
                await _appUserManager.UpdateAsync(user);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ConfirmPhone(string userName, string code, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }

            var user = await _appUserManager.FindByNameAsync(userName);
            if (user == null || user.IsDeleted)
            {
                throw new ApplicationException($"Unable to load user with name '{userName}'.");
            }

            //var result = await _appUserManager.VerifyChangePhoneNumberTokenAsync(user, code, phoneNumber);
            var result = true;
            if (result)
            {
                user.PhoneNumberConfirmed = true;
                var updateUser = await _appUserManager.UpdateAsync(user);
                if (updateUser.Succeeded)
                {
                    user.LastLogin = DateTime.Now;
                    await _appUserManager.UpdateAsync(user);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return true;
                }
                else
                {
                    throw new ApplicationException("Update phone number confirm error: " + updateUser.Errors.ToString());
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Login(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            var user = await _appUserManager.FindByNameAsync(userName);
            if (user == null || user.IsDeleted)
            {
                throw new ApplicationException($"Unable to load user with name '{userName}'.");
            }

            user.LastLogin = DateTime.Now;
            await _appUserManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return true;
        }

        public async Task UpdateSecurityStamp(string userName)
        {
            var user = await _appUserManager.FindByNameAsync(userName);
            await _appUserManager.UpdateSecurityStampAsync(user);
        }

        public async Task<bool> ResetPassword(string userName, string code, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(newPassword))
            {
                throw new ApplicationException($"Unable to load user with name '{userName}'.");
            }

            var user = await _appUserManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with name '{userName}'.");
            }

            var result = await _appUserManager.ResetPasswordAsync(user, code, newPassword);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordByVerifyCode(string userName, string code, string phoneNumber, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(newPassword))
            {
                return false;
            }

            var user = await _appUserManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with name '{userName}'.");
            }

            var result = await _appUserManager.VerifyChangePhoneNumberTokenAsync(user, code, phoneNumber);
            if (result)
            {
                var removePassword = await _appUserManager.RemovePasswordAsync(user);
                if (removePassword.Succeeded)
                {
                    var addPassword = await _appUserManager.AddPasswordAsync(user, newPassword);
                    if (addPassword.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<User> GetUserById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            var user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId && !u.IsDeleted, "KVRR");

            return user;
        }

        public async Task<User> GetUserByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            var user = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userName && !u.IsDeleted, "KVRR");

            return user;
        }

        public async Task<User> GetUserByEmailOrPhone(string emailOrPhone)
        {
            if (string.IsNullOrWhiteSpace(emailOrPhone) || (!emailOrPhone.IsEmail() && !emailOrPhone.IsPhoneNumber()))
            {
                return null;
            }

            if (emailOrPhone.IsEmail())
            {
                var user = await _unitOfWork.UserRepository.GetAsync(u => u.Email == emailOrPhone && !u.IsDeleted, "KVRR");

                return user;
            }
            else
            {
                var user = await _unitOfWork.UserRepository.GetAsync(u => u.PhoneNumber != null && u.PhoneNumber == emailOrPhone, "KVRR");
                if (user != null && user.IsDeleted)
                {
                    return null;
                }
                return user;
            }

        }

        public async Task<bool> IsInRole(User user, string roleName)
        {
            if (user == null || string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }

            return await _appUserManager.IsInRoleAsync(user, roleName);
        }

        public string GetRoleName(User user)
        {
            if (user != null)
            {
                if (IsInRole(user, RoleName.Admin).Result)
                {
                    return Model.Resources.Common.Admin;
                }
                if (IsInRole(user, RoleName.CustomerManager).Result)
                {
                    return Model.Resources.Common.CustomerService;
                }
                if (IsInRole(user, RoleName.InvestmentManager).Result)
                {
                    return Model.Resources.Common.BackOfficer;
                }
                if (IsInRole(user, RoleName.Accountant).Result)
                {
                    return Model.Resources.Common.Accountant;
                }
            }

            return string.Empty;
        }

        public bool IsSignedIn(ClaimsPrincipal user)
        {
            return _signInManager.IsSignedIn(user);
        }

        public async Task<bool> DeleteUserById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            var user = await _appUserManager.FindByIdAsync(userId);
            if (user != null)
            {
                //await _appUserManager.RemoveFromRolesAsync(user, _appUserManager.GetRolesAsync(user).Result);
                _unitOfWork.UserRepository.Delete(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteUserByIds(List<string> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return false;
            }

            var listUser = await _unitOfWork.UserRepository.FindByAsync(u => !u.IsDeleted && userIds.Contains(u.Id));

            if (listUser != null && listUser.Any())
            {
                _unitOfWork.UserRepository.BulkDelete(listUser.ToList());
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ChangePassword(string userName, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                return false;
            }

            var user = await _appUserManager.FindByNameAsync(userName);
            if (user != null)
            {
                var changePassword = await _appUserManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (changePassword.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<User>> GetAllUserByRoles(params string[] roles)
        {
            var users = new List<User>();
            foreach (var role in roles)
            {
                var us = (await _appUserManager.GetUsersInRoleAsync(role)).ToList();
                users = users.Union(us).ToList();
            }

            return users.OrderBy(h => h.FullName).ToList();

        }

        public async Task<List<User>> GetUsersByRoles(int pageSize, int pageIndex, params string[] roles)
        {
            if (pageSize < 0 || pageIndex < 1)
            {
                return null;
            }

            var listUser = new List<User>();
            if (roles != null && roles.Any())
            {
                listUser = (await _unitOfWork.UserRepository.FindByAsync(u => !u.IsDeleted && IsInRoles(u, roles))).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return listUser;
        }

        public async Task<User> GetUserRelateData()
        {
            if (string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext.User.Identity.Name))
            {
                return null;
            }

            var user = await _unitOfWork.UserRepository.GetAsync(
                x => x.UserName.Equals(_httpContextAccessor.HttpContext.User.Identity.Name), "KVRR");

            if (user != null && user.IsDeleted)
            {
                return null;
            }

            return user;
        }

        public async Task ConfirmKVRR(int kvrrId)
        {
            try
            {
                if (kvrrId <= 0)
                {
                    throw new InvalidParameterException();
                }

                var user = await GetCurrentUser();
                var kvrr = await _unitOfWork.KVRRRepository.GetAsync(x => x.Id == kvrrId);
                user.KVRR = kvrr;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<UserPorfolio>> GetUserPorfolio(string userId = null)
        {
            if(string.IsNullOrWhiteSpace(userId))
            {
                var currentUser = await GetCurrentUser();
                if (currentUser != null)
                {
                    var userPorfolios = (await _unitOfWork.UserFundRepository.GetAllAsync("User,Fund")).Where(i => i.UserId == currentUser.Id)
                        .Select(i => new UserPorfolio()
                        {
                            CertificateValue = i.Fund.NAV,
                            OldCertificateValue = i.Fund.NAVOld,
                            CurrentAccountAmount = i.User.CurrentAccountAmount,
                            FundName = i.Fund.Title,
                            FundCode = i.Fund.Code,
                            NoOfCertificates = i.NoOfCertificates,
                            Status = i.EditStatus
                        }).OrderBy(i => i.FundCode).ToList();

                    return userPorfolios;
                }
            }
            else
            {
                var userPorfolios = (await _unitOfWork.UserFundRepository.GetAllAsync("User,Fund")).Where(i => i.UserId == userId)
                        .Select(i => new UserPorfolio()
                        {
                            CertificateValue = i.Fund.NAV,
                            OldCertificateValue = i.Fund.NAVOld,
                            CurrentAccountAmount = i.User.CurrentAccountAmount,
                            FundName = i.Fund.Title,
                            FundCode = i.Fund.Code,
                            NoOfCertificates = i.NoOfCertificates,
                            Status = i.EditStatus
                        }).OrderBy(i => i.FundCode).ToList();

                return userPorfolios;
            }

            return null;
        }

        private bool IsInRoles(User user, params string[] roles)
        {
            foreach (var role in roles)
            {
                if (IsInRole(user, role).Result)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
