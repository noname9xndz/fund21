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
using smartFunds.Model.Client;
using static smartFunds.Common.Constants;
using smartFunds.Model.Admin;

namespace smartFunds.Service.Services
{
    public interface IUserService
    {
        Task UpdateUser(UserModel userModel);
        Task<LoginStatus> Login(string userName, string password, bool isRemember, bool lockoutOnFailure);
        Task<LoginStatus> LoginByEmailOrPhone(string emailOrPhone, string password, bool isRemember, bool lockoutOnFailure);
        Task<RegisterStatus> RegisterUser(UserModel userModel, string password, string roleName);
        Task<string> GenerateEmailConfirmationToken(UserModel userModel);
        Task<string> GeneratePhoneConfirmationToken(string userName, string phoneNumber);
        Task<string> GeneratePasswordResetToken(UserModel userModel);
        Task<bool> ConfirmEmail(string userId, string code);
        Task<bool> ConfirmPhone(string userName, string code, string phoneNumber);
        Task LogoutUser();
        Task<UserModel> GetUserById(string userId);
        Task<UserModel> GetUserByName(string userName);
        Task<UserModel> GetUserByEmailOrPhone(string emailOrPhone);
        Task<bool> IsInRole(UserModel userModel, string roleName, string roleNameSub = null);
        string GetRoleName(UserModel userModel);
        Task<bool> ResetPassword(string userName, string code, string newPassword);
        Task<bool> ResetPasswordByVerifyCode(string userName, string code, string phoneNumber, string newPassword);
        Task<bool> DeleteUserById(string userId);
        Task<bool> DeleteUserByIds(List<string> userIds);
        bool IsSignedIn(ClaimsPrincipal user);
        Task<bool> ChangePassword(string userName, string currentPassword, string newPassword);
        Task<List<UserModel>> GetUsersByRoles(int pageSize, int pageIndex, params string[] roles);
        Task<AdminUsersModel> GetAdminUsers(int pageSize, int pageIndex);
        Task<AdminUsersModel> GetAllAdminUsers();
        Task<List<UserModel>> GetAdminUsers();
        Task<UserModel> GetCurrentUser();
        Task<UserModel> GetUserRelateData();
        Task ConfirmKVRR(int kvrrId);
        Task<List<UserPorfolio>> GetUserPorfolio();
        Task<bool> Login(string userName);
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

        public async Task UpdateUser(UserModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            await _userManager.UpdateUser(user);
        }

        public async Task<LoginStatus> Login(string userName, string password, bool isRemember, bool lockoutOnFailure)
        {
            return await _userManager.LoginUser(userName, password, isRemember, lockoutOnFailure);
        }

        public async Task<LoginStatus> LoginByEmailOrPhone(string emailOrPhone, string password, bool isRemember, bool lockoutOnFailure)
        {
            return await _userManager.LoginUserByEmailOrPhone(emailOrPhone, password, isRemember, lockoutOnFailure);
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
            var user = await _userManager.GetUserById(userId);
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<UserModel> GetUserByName(string userName)
        {
            var user = await _userManager.GetUserByName(userName);
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<UserModel> GetUserByEmailOrPhone(string emailOrPhone)
        {
            var user = await _userManager.GetUserByEmailOrPhone(emailOrPhone);
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<bool> IsInRole(UserModel userModel, string roleName, string ruleNameSub = null)
        {
            User user = _mapper.Map<User>(userModel);
            if (ruleNameSub == null)
            {
                return await _userManager.IsInRole(user, roleName);
            }
            else
            {
                var _starusRuleName = await _userManager.IsInRole(user, roleName);
                var _starusRuleNameSub = await _userManager.IsInRole(user, ruleNameSub);
                return (_starusRuleName || _starusRuleNameSub) ? true : false;
            }
        }

        public async Task<bool> IsUnInRole(UserModel userModel, string roleName)
        {
            User user = _mapper.Map<User>(userModel);
            return !(await _userManager.IsInRole(user, roleName));
        }

        public string GetRoleName(UserModel userModel)
        {
            User user = _mapper.Map<User>(userModel);
            return _userManager.GetRoleName(user);
        }

        public async Task<bool> ResetPassword(string userName, string code, string newPassword)
        {
            return await _userManager.ResetPassword(userName, code, newPassword);
        }

        public async Task<bool> ResetPasswordByVerifyCode(string userName, string code, string phoneNumber, string newPassword)
        {
            return await _userManager.ResetPasswordByVerifyCode(userName, code, phoneNumber, newPassword);
        }

        public async Task<bool> DeleteUserById(string userId)
        {
            return await _userManager.DeleteUserById(userId);
        }

        public async Task<bool> DeleteUserByIds(List<string> userIds)
        {
            return await _userManager.DeleteUserByIds(userIds);
        }

        public bool IsSignedIn(ClaimsPrincipal user)
        {
            return _userManager.IsSignedIn(user);
        }

        public async Task<bool> ChangePassword(string userName, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePassword(userName, currentPassword, newPassword);
        }

        public async Task<List<UserModel>> GetUsersByRoles(int pageSize, int pageIndex, params string[] roles)
        {
            var listUser = await _userManager.GetUsersByRoles(pageSize, pageIndex, roles);
            var listUserModel = _mapper.Map<List<User>, List<UserModel>>(listUser);
            return listUserModel;
        }

        public async Task<AdminUsersModel> GetAdminUsers(int pageSize, int pageIndex)
        {
            var allUserAdmin = (await _userManager.GetAllUserByRoles(RoleName.Admin, RoleName.CustomerManager, RoleName.InvestmentManager, RoleName.Accountant));
            var listUserAdmin = (await GetUsersByRoles(pageSize, pageIndex, RoleName.Admin, RoleName.CustomerManager, RoleName.InvestmentManager, RoleName.Accountant));

            var adminUsersModel = new AdminUsersModel();
            adminUsersModel.ListUser = listUserAdmin;
            adminUsersModel.TotalCount = allUserAdmin.Count;

            return adminUsersModel;
        }

        public async Task<AdminUsersModel> GetAllAdminUsers()
        {
            var allUserAdmin = (await _userManager.GetAllUserByRoles(RoleName.Admin, RoleName.CustomerManager, RoleName.InvestmentManager, RoleName.Accountant));

            var adminUsersModel = new AdminUsersModel();
            adminUsersModel.ListUser = _mapper.Map<List<User>, List<UserModel>>(allUserAdmin);
            adminUsersModel.TotalCount = allUserAdmin.Count;

            return adminUsersModel;
        }

        public async Task<List<UserModel>> GetAdminUsers()
        {
            try
            {
                var listUsers = await _userManager.GetAllUserByRoles(RoleName.Admin);
                var listUserModels = _mapper.Map<List<User>, List<UserModel>>(listUsers);
                return listUserModels;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserModel> GetCurrentUser()
        {
            var user = await _userManager.GetCurrentUser();
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<UserModel> GetUserRelateData()
        {
            var user = await _userManager.GetUserRelateData();
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task ConfirmKVRR(int kvrrId)
        {
            try
            {
                await _userManager.ConfirmKVRR(kvrrId);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserPorfolio>> GetUserPorfolio()
        {
            var userPortfolios = await _userManager.GetUserPorfolio();
            return userPortfolios;
        }

        public async Task<bool> Login(string userName)
        {
            return await _userManager.Login(userName);
        }
    }
}