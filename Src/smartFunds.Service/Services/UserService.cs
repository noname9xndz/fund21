using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using smartFunds.Model.Common;
using System.Threading.Tasks;
using AutoMapper;
using smartFunds.Business.Common;
using smartFunds.Data.Models;
using Microsoft.AspNetCore.Identity;
using smartFunds.Common;
using System.Collections.Generic;

namespace smartFunds.Service.Services
{
    public interface IUserService
    {
        Task<LoginStatus> Login(string userName, string password, bool isRemember, bool lockoutOnFailure);
        Task<RegisterStatus> RegisterUser(UserModel userModel, string password, string roleName);
        Task<string> GenerateEmailConfirmationToken(UserModel userModel);
        Task<string> GeneratePhoneConfirmationToken(string userName, string phoneNumber);
        Task<string> GeneratePasswordResetToken(UserModel userModel);
        Task<bool> ConfirmEmail(string userId, string code);
        Task<bool> ConfirmPhone(string userName, string code, string phoneNumber);
        Task LogoutUser();
        Task<UserModel> GetUserById(string userId);
        Task<UserModel> GetUserByName(string userName);
        Task<bool> IsInRole(UserModel userModel, string roleName);
        Task<bool> ResetPassword(string userName, string code, string newPassword);
        Task<bool> ResetPasswordByVerifyCode(string userName, string code, string phoneNumber, string newPassword);
    }
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;

        public UserService(IMapper mapper, IUserManager userManager)
        {
            _mapper = mapper;
            _userManager = userManager;

        }

        public async Task<LoginStatus> Login(string userName, string password, bool isRemember, bool lockoutOnFailure)
        {
            return await _userManager.LoginUser(userName, password, isRemember, lockoutOnFailure);
        }

        public async Task<RegisterStatus> RegisterUser(UserModel userModel, string password, string roleName)
        {
            User user = _mapper.Map<User>(userModel);
            return await _userManager.RegisterUser(user, password, roleName);
        }

        public async Task<string> GenerateEmailConfirmationToken(UserModel userModel)
        {
            User user = _mapper.Map<User>(userModel);
            return await _userManager.GenerateEmailConfirmationToken(user);
        }

        public async Task<string> GeneratePhoneConfirmationToken(string userName, string phoneNumber)
        {
            return await _userManager.GeneratePhoneConfirmationToken(userName, phoneNumber);
        }

        public async Task<string> GeneratePasswordResetToken(UserModel userModel)
        {
            User user = _mapper.Map<User>(userModel);
            return await _userManager.GeneratePasswordResetToken(user);
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            return await _userManager.ConfirmEmail(userId, code);
        }

        public async Task<bool> ConfirmPhone(string userName, string code, string phoneNumber)
        {
            return await _userManager.ConfirmPhone(userName, code, phoneNumber);
        }

        public async Task LogoutUser()
        {
            await _userManager.LogoutUser();
        }

        public async Task<UserModel> GetUserById(string userId)
        {
            var user =  await _userManager.GetUserById(userId);
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<UserModel> GetUserByName(string userName)
        {
            var user = await _userManager.GetUserByName(userName);
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<bool> IsInRole(UserModel userModel, string roleName)
        {
            User user = _mapper.Map<User>(userModel);
            return await _userManager.IsInRole(user, roleName);
        }

        public async Task<bool> ResetPassword(string userName, string code, string newPassword)
        {
            return await _userManager.ResetPassword(userName, code, newPassword);
        }

        public async Task<bool> ResetPasswordByVerifyCode(string userName, string code, string phoneNumber, string newPassword)
        {
            return await _userManager.ResetPasswordByVerifyCode(userName, code, phoneNumber, newPassword);
        }
    }
}