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
    public interface IMaintainingFeeService
    {
        ListMaintainingFee GetConfiguration();
        List<MaintainingFeeModel> GetConfigurationByIds(int[] Ids);
        Task UpdateConfigurations(List<MaintainingFeeModel> fees);
        Task<MaintainingFeeModel> AddConfiguration(MaintainingFeeModel model);
        Task AddListFees(List<MaintainingFeeModel> list);
        void DeleteConfigurationByIds(int[] Ids);
        decimal GetPercentageByValue(decimal amount);
        Task<bool> ValidAmountMoney(MaintainingFeeModel currentFee);
    }
    public class MaintainingFeeService : IMaintainingFeeService
    {
        private readonly IMapper _mapper;
        private readonly IMaintainingFeeManager _maintainingFeeManager;

        public MaintainingFeeService(IMapper mapper, IMaintainingFeeManager maintainingFeeManager)
        {
            _mapper = mapper;
            _maintainingFeeManager = maintainingFeeManager;
        }

        public decimal GetPercentageByValue(decimal amount)
        {
            decimal result = _maintainingFeeManager.GetPercentageByValue(amount);
            return result;
        }

        public async Task<MaintainingFeeModel> AddConfiguration(MaintainingFeeModel maintainingFee)
        {
            try
            {
                MaintainingFee model = _mapper.Map<MaintainingFee>(maintainingFee);
                MaintainingFee addedModel = await _maintainingFeeManager.AddConfiguration(model);
                MaintainingFeeModel savedModel = _mapper.Map<MaintainingFeeModel>(addedModel);
                return savedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddListFees(List<MaintainingFeeModel> list)
        {
            try
            {
                List<MaintainingFee> mappedModels = _mapper.Map<List<MaintainingFee>>(list);
                await _maintainingFeeManager.AddListFees(mappedModels);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteConfigurationByIds(int[] Ids)
        {
            try
            {
                _maintainingFeeManager.DeleteConfigurationByIds(Ids);
            }
            catch( Exception ex)
            {
                throw ex;
            }
        }

        public ListMaintainingFee GetConfiguration()
        {
            try
            {
                List<MaintainingFee> modelDTO = _maintainingFeeManager.GetConfiguration();
                var list = _mapper.Map<List<MaintainingFeeModel>>(modelDTO);

                ListMaintainingFee item = new ListMaintainingFee();
                item.ListFees = list;
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MaintainingFeeModel> GetConfigurationByIds(int[] Ids)
        {
            try
            {
                var list = _maintainingFeeManager.GetConfigurationByIds(Ids);
                return _mapper.Map<List<MaintainingFeeModel>>(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateConfigurations(List<MaintainingFeeModel> fees)
        {
            try
            {
                List<MaintainingFee> list = _mapper.Map< List<MaintainingFeeModel> ,List<MaintainingFee>>(fees);
                await _maintainingFeeManager.UpdateFees(list);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ValidAmountMoney(MaintainingFeeModel currentFee)
        {
            try
            {
                var rangeMoneyDto = _mapper.Map<MaintainingFee>(currentFee);
                return await _maintainingFeeManager.ValidAmountMoney(rangeMoneyDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
