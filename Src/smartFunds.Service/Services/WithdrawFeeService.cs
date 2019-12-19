using AutoMapper;
using smartFunds.Business.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IWithdrawFeeService
    {
        Task<ListWithdrawFee> GetConfiguration();
        Task<WithdrawFeeModel> GetQuickWithdrawalFeeItem();
        List<WithdrawFeeModel> GetConfigurationByIds(int[] Ids);
        Task UpdateConfigurations(List<WithdrawFeeModel> fees);
        Task<WithdrawFeeModel> AddConfiguration(WithdrawFeeModel model);
        Task AddListFees(List<WithdrawFeeModel> list);
        Task<decimal> GetQuickWithdrawalFee();
        Task<decimal> GetFeeAmount(decimal withdrawalAmount, bool noUpdateDB = false);
        Task<bool> IsValidMonth(WithdrawFeeModel currentFee);
        void DeleteConfigurationByIds(int[] Ids);
    }
    public class WithdrawFeeService : IWithdrawFeeService
    {
        private readonly IMapper _mapper;
        private readonly IWithdrawFeeManager _withdrawFeeManager;

        public WithdrawFeeService(IMapper mapper, IWithdrawFeeManager withdrawFeeManager)
        {
            _mapper = mapper;
            _withdrawFeeManager = withdrawFeeManager;
        }

        public async Task<WithdrawFeeModel> AddConfiguration(WithdrawFeeModel withdrawFee)
        {
            try
            {
                var model = _mapper.Map<WithdrawalFee>(withdrawFee);
                var addedModel = await _withdrawFeeManager.AddConfiguration(model);
                var savedModel = _mapper.Map<WithdrawFeeModel>(addedModel);
                return savedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddListFees(List<WithdrawFeeModel> list)
        {
            try
            {
                List<WithdrawalFee> mappedModels = _mapper.Map<List<WithdrawalFee>>(list);
                await _withdrawFeeManager.AddListFees(mappedModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<decimal> GetQuickWithdrawalFee()
        {
            return await _withdrawFeeManager.GetQuickWithdrawalFee();
        }

        public async Task<decimal> GetFeeAmount(decimal withdrawalAmount, bool noUpdateDB = false)
        {
            return await _withdrawFeeManager.GetFeeAmount(withdrawalAmount, noUpdateDB);
        }

        public async Task<ListWithdrawFee> GetConfiguration()
        {
            try
            {
                List<WithdrawalFee> modelDTO = _withdrawFeeManager.GetConfiguration();
                var list = _mapper.Map<List<WithdrawFeeModel>>(modelDTO);

                ListWithdrawFee item = new ListWithdrawFee();
                item.ListFees = list;
                item.QuickWithdrawFee = _mapper.Map<WithdrawFeeModel>(await _withdrawFeeManager.GetQuickWithdrawalFeeItem());
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WithdrawFeeModel> GetQuickWithdrawalFeeItem()
        {
            return _mapper.Map<WithdrawFeeModel>(await _withdrawFeeManager.GetQuickWithdrawalFeeItem());
        }

        public List<WithdrawFeeModel> GetConfigurationByIds(int[] Ids)
        {
            try
            {
                var list = _withdrawFeeManager.GetConfigurationByIds(Ids);
                return _mapper.Map<List<WithdrawFeeModel>>(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateConfigurations(List<WithdrawFeeModel> fees)
        {
            try
            {
                List<WithdrawalFee> list = _mapper.Map<List<WithdrawFeeModel>, List<WithdrawalFee>>(fees);
                //foreach (var itm in list)
                //{
                //    itm.Id = 0;
                //}
                await _withdrawFeeManager.UpdateFees(list);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> IsValidMonth(WithdrawFeeModel currentFee)
        {
            try
            {
                var rangeMonthDto = _mapper.Map<WithdrawalFee>(currentFee);
                return await _withdrawFeeManager.ValidRangeMonth(rangeMonthDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteConfigurationByIds(int[] Ids)
        {
            try
            {
                _withdrawFeeManager.DeleteConfigurationByIds(Ids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
