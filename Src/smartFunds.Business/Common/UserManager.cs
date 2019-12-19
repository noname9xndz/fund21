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

namespace smartFunds.Business.Common
{
    public interface IUserManager
    {        
        Task<RegisterStatus> RegisterUser(User user, string password, string roleName);
        Task<string> GenerateEmailConfirmationToken(User user);
        Task<string> GeneratePhoneConfirmationToken(string userName, string phoneNumber);
        Task<string> GeneratePasswordResetToken(User user);
        Task<LoginStatus> LoginUser(string userName, string password, bool isRemember, bool lockoutOnFailure);
        Task<bool> ConfirmEmail(string userId, string code);
        Task<bool> ConfirmPhone(string userName, string code, string phoneNumber);
        Task<User> GetUserById(string userId);
        Task<User> GetUserByName(string userName);
        Task LogoutUser();
        Task<bool> IsInRole(User user, string roleName);
        Task<bool> ResetPassword(string userName, string code, string newPassword);
        Task<bool> ResetPasswordByVerifyCode(string userName, string code, string phoneNumber, string newPassword);
        string CurrentUser();
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

        public async Task<LoginStatus> LoginUser(string userName, string password, bool isRemember, bool lockoutOnFailure)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return LoginStatus.None;
            }
            var user = await GetUserByName(userName);

            if (user != null && (await _appUserManager.IsEmailConfirmedAsync(user) || await _appUserManager.IsPhoneNumberConfirmedAsync(user)))
            {
                var result = await _signInManager.PasswordSignInAsync(userName, password, isRemember, lockoutOnFailure);
                if (result.Succeeded)
                {
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

            return LoginStatus.Error;
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
                if (existUser.EmailConfirmed || existUser.PhoneNumberConfirmed)
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
            var user = await GetUserByName(userName);
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
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with name '{userName}'.");
            }

            var result = await _appUserManager.VerifyChangePhoneNumberTokenAsync(user, code, phoneNumber);
            if (result)
            {
                user.PhoneNumberConfirmed = true;
                var updateUser = await _appUserManager.UpdateAsync(user);
                if (updateUser.Succeeded)
                {
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

            return await _appUserManager.FindByIdAsync(userId);
        }

        public async Task<User> GetUserByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            return await _appUserManager.FindByNameAsync(userName);
        }

        public async Task<bool> IsInRole(User user, string roleName)
        {
            if (user == null || string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }
            
            return await _appUserManager.IsInRoleAsync(user, roleName);
        }
    }
}
