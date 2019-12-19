using AutoMapper;
using smartFunds.Business;
using smartFunds.Business.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IFundPurchaseFeeService
    {
        Task<FundPurchaseFeeModel> GetFundPurchaseFee(int? fundPurchaseFeeId);
        Task<FundPurchaseFeeModel> SaveFundPurchaseFee(FundPurchaseFeeModel fundPurchaseFee);
        Task UpdateFundPurchaseFee(FundPurchaseFeeModel fundPurchaseFee);
        Task<List<FundPurchaseFeeModel>> GetListFundPurchaseFee(int fundId);
        Task DeleteListFundPurchaseFee(List<int> fundPurchaseFeeIds);
        Task<decimal> GetFeeValue(int fundId, decimal amount);
    }
    public class FundPurchaseFeeService : IFundPurchaseFeeService
    {
        private readonly IMapper _mapper;
        private readonly IFundPurchaseFeeManager _fundPurchaseFeeManager;
        public FundPurchaseFeeService(IMapper mapper, IFundPurchaseFeeManager fundPurchaseFeeManager)
        {
            _mapper = mapper;
            _fundPurchaseFeeManager = fundPurchaseFeeManager;
        }
        public async Task<FundPurchaseFeeModel> GetFundPurchaseFee(int? fundPurchaseFeeId)
        {
            try
            {
                var FundPurchaseFee = await _fundPurchaseFeeManager.GetFundPurchaseFee(fundPurchaseFeeId);
                return _mapper.Map<FundPurchaseFee, FundPurchaseFeeModel>(FundPurchaseFee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FundPurchaseFeeModel> SaveFundPurchaseFee(FundPurchaseFeeModel fundPurchaseFee)
        {
            FundPurchaseFee newFundPurchaseFee = _mapper.Map<FundPurchaseFee>(fundPurchaseFee);
            FundPurchaseFee savedFundPurchaseFee = await _fundPurchaseFeeManager.SaveFundPurchaseFee(newFundPurchaseFee);
            FundPurchaseFeeModel savedFundPurchaseFeeModel = _mapper.Map<FundPurchaseFeeModel>(savedFundPurchaseFee);
            return savedFundPurchaseFeeModel;
        }

        public async Task UpdateFundPurchaseFee(FundPurchaseFeeModel fundPurchaseFee)
        {
            FundPurchaseFee newFundPurchaseFee = _mapper.Map<FundPurchaseFee>(fundPurchaseFee);
            await _fundPurchaseFeeManager.UpdateFundPurchaseFee(newFundPurchaseFee);
        }

        public async Task<List<FundPurchaseFeeModel>> GetListFundPurchaseFee(int fundId)
        {
            var list = await _fundPurchaseFeeManager.GetListFundPurchaseFee(fundId);
            return _mapper.Map<List<FundPurchaseFee>, List<FundPurchaseFeeModel>>(list);
        }

        public async Task DeleteListFundPurchaseFee(List<int> fundPurchaseFeeIds)
        {
            await _fundPurchaseFeeManager.DeleteListFundPurchaseFee(fundPurchaseFeeIds);
        }

        public async Task<decimal> GetFeeValue(int fundId, decimal amount)
        {
            return await _fundPurchaseFeeManager.GetFeeValue(fundId, amount);
        }
    }
}
