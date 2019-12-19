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
    public interface IFundSellFeeService
    {
        Task<FundSellFeeModel> GetFundSellFee(int? fundSellFeeId);
        Task<FundSellFeeModel> SaveFundSellFee(FundSellFeeModel fundSellFee);
        Task UpdateFundSellFee(FundSellFeeModel fundSellFee);
        Task<List<FundSellFeeModel>> GetListFundSellFee(int fundId);
        Task DeleteListFundSellFee(List<int> fundSellFeeIds);
    }
    public class FundSellFeeService : IFundSellFeeService
    {
        private readonly IMapper _mapper;
        private readonly IFundSellFeeManager _fundSellFeeManager;
        public FundSellFeeService(IMapper mapper, IFundSellFeeManager fundSellFeeManager)
        {
            _mapper = mapper;
            _fundSellFeeManager = fundSellFeeManager;
        }
        public async Task<FundSellFeeModel> GetFundSellFee(int? fundSellFeeId)
        {
            try
            {
                var FundSellFee = await _fundSellFeeManager.GetFundSellFee(fundSellFeeId);
                return _mapper.Map<FundSellFee, FundSellFeeModel>(FundSellFee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FundSellFeeModel> SaveFundSellFee(FundSellFeeModel fundSellFee)
        {
            FundSellFee newFundSellFee = _mapper.Map<FundSellFee>(fundSellFee);
            FundSellFee savedFundSellFee = await _fundSellFeeManager.SaveFundSellFee(newFundSellFee);
            FundSellFeeModel savedFundSellFeeModel = _mapper.Map<FundSellFeeModel>(savedFundSellFee);
            return savedFundSellFeeModel;
        }

        public async Task UpdateFundSellFee(FundSellFeeModel fundSellFee)
        {
            FundSellFee newFundSellFee = _mapper.Map<FundSellFee>(fundSellFee);
            await _fundSellFeeManager.UpdateFundSellFee(newFundSellFee);
        }

        public async Task<List<FundSellFeeModel>> GetListFundSellFee(int fundId)
        {
            var list = await _fundSellFeeManager.GetListFundSellFee(fundId);
            return _mapper.Map<List<FundSellFee>, List<FundSellFeeModel>>(list);
        }

        public async Task DeleteListFundSellFee(List<int> fundSellFeeIds)
        {
            await _fundSellFeeManager.DeleteListFundSellFee(fundSellFeeIds);
        }
    }
}
