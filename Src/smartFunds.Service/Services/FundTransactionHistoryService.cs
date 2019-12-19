using AutoMapper;
using smartFunds.Business;
using smartFunds.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IFundTransactionHistoryService
    {
        Task<FundTransactionHistoryModel> AddFundTransactionHistory(FundTransactionHistoryModel fundTransactionHistoryModel);
        Task UpdateFundTransactionHistory(FundTransactionHistoryModel fundTransactionHistoryModel);
        Task<List<FundTransactionHistoryModel>> GetFundTransactionHistoryByFundId(int fundId, smartFunds.Common.EditStatus? taskStatus = null);
        Task<List<FundTransactionHistoryModel>> GetFundTransactionHistoryByFundId(int fundId, int pageIndex, int pageSize, smartFunds.Common.EditStatus? status = null);
        Task<ListFundTransactionHistoryModel> GetsFundTransactionHistory(int pageIndex, int pageSize, SearchBalanceFund searchBalanceFund = null);
        Task<List<FundTransactionHistoryModel>> GetAllFundTransactionHistory();
        Task<List<FundTransactionHistoryModel>> GetListBalanceFund(smartFunds.Common.EditStatus status);
        Task<FundTransactionHistoryModel> GetListBalanceFund(int fundId, smartFunds.Common.EditStatus status);
        Task<byte[]> ExportBalanceFund(smartFunds.Common.EditStatus status);
        Task<byte[]> ExportDealFund(SearchBalanceFund searchBalanceFund = null);
        Task<int> GetCountUserFund(int fundId);
        Task<decimal> GetTotalAmountInvestFund(int fundId);
        Task<InvestmentFunds> GetInvestmentFunds();
        Task Investment(decimal amount, string customerUserName = null);
        Task Withdrawal(string userName, decimal amount, decimal fee, WithdrawalType? type);
        Task ChangeKVRR(int newKVRRId);
        Task ApproveBalanceFund(int fundId);
        Task<bool> ApproveFundPercent(int portfolioId);
    }
    public class FundTransactionHistoryService : IFundTransactionHistoryService
    {
        private readonly IMapper _mapper;
        private readonly IFundTransactionHistoryManager _fundTransactionHistoryManager;
        public FundTransactionHistoryService(IMapper mapper, IFundTransactionHistoryManager fundTransactionHistoryManager)
        {
            _mapper = mapper;
            _fundTransactionHistoryManager = fundTransactionHistoryManager;
        }

        public async Task<FundTransactionHistoryModel> AddFundTransactionHistory(FundTransactionHistoryModel fundTransactionHistoryModel)
        {
            var fundTransactionHistory = _mapper.Map<FundTransactionHistory>(fundTransactionHistoryModel);
            var fundTransactionHistoryAdded = await _fundTransactionHistoryManager.AddFundTransactionHistory(fundTransactionHistory);
            return _mapper.Map<FundTransactionHistoryModel>(fundTransactionHistoryAdded);
        }

        public async Task UpdateFundTransactionHistory(FundTransactionHistoryModel fundTransactionHistoryModel)
        {
            var fundTransactionHistory = _mapper.Map<FundTransactionHistory>(fundTransactionHistoryModel);
            await _fundTransactionHistoryManager.UpdateFundTransactionHistory(fundTransactionHistory);
        }

        public async Task<List<FundTransactionHistoryModel>> GetFundTransactionHistoryByFundId(int fundId, smartFunds.Common.EditStatus? status = null)
        {
            var listFundTransactionHistory = await _fundTransactionHistoryManager.GetFundTransactionHistoryByFundId(fundId, status);
            return _mapper.Map<List<FundTransactionHistory>, List<FundTransactionHistoryModel>>(listFundTransactionHistory);
        }

        public async Task<List<FundTransactionHistoryModel>> GetFundTransactionHistoryByFundId(int fundId, int pageIndex, int pageSize, smartFunds.Common.EditStatus? status = null)
        {
            var listFundTransactionHistory = await _fundTransactionHistoryManager.GetFundTransactionHistoryByFundId(fundId, pageIndex, pageSize, status);
            return _mapper.Map<List<FundTransactionHistory>, List<FundTransactionHistoryModel>>(listFundTransactionHistory);
        }

        public async Task<ListFundTransactionHistoryModel> GetsFundTransactionHistory(int pageIndex, int pageSize, SearchBalanceFund searchBalanceFund = null)
        {
            var listFundTransactionHistoryModel = new ListFundTransactionHistoryModel();
            var listTransactionHistory = await _fundTransactionHistoryManager.GetsFundTransactionHistory(pageSize,pageIndex, searchBalanceFund);
            listFundTransactionHistoryModel.ListFundTransactionHistory = _mapper.Map<List<FundTransactionHistory>, List<FundTransactionHistoryModel>>(listTransactionHistory);
            listFundTransactionHistoryModel.TotalCount = _fundTransactionHistoryManager.GetsFundTransactionHistory(searchBalanceFund).Result.Count();
            return listFundTransactionHistoryModel;
        }

        public async Task<List<FundTransactionHistoryModel>> GetAllFundTransactionHistory()
        {
            var allFundTransactionHistory = await _fundTransactionHistoryManager.GetAllFundTransactionHistory();
            return _mapper.Map<List<FundTransactionHistory>, List<FundTransactionHistoryModel>>(allFundTransactionHistory);
        }

        public async Task<List<FundTransactionHistoryModel>> GetListBalanceFund(smartFunds.Common.EditStatus status)
        {
            var allFundTransactionHistory = await _fundTransactionHistoryManager.GetListBalanceFund(status);
            return _mapper.Map<List<FundTransactionHistory>, List<FundTransactionHistoryModel>>(allFundTransactionHistory);
        }

        public async Task<FundTransactionHistoryModel> GetListBalanceFund(int fundId, smartFunds.Common.EditStatus status)
        {
            var fundTransactionHistory = await _fundTransactionHistoryManager.GetBalanceFund(fundId, status);
            return _mapper.Map<FundTransactionHistory, FundTransactionHistoryModel>(fundTransactionHistory);
        }

        public async Task<byte[]> ExportBalanceFund(smartFunds.Common.EditStatus status)
        {
            return await _fundTransactionHistoryManager.ExportBalanceFund(status);
        }

        public async Task<byte[]> ExportDealFund(SearchBalanceFund searchBalanceFund = null)
        {
            return await _fundTransactionHistoryManager.ExportDealFund(searchBalanceFund);
        }

        public async Task<int> GetCountUserFund(int fundId)
        {
            return await _fundTransactionHistoryManager.GetCountUserFund(fundId);
        }

        public async Task<decimal> GetTotalAmountInvestFund(int fundId)
        {
            return await _fundTransactionHistoryManager.GetTotalAmountInvestFund(fundId);
        }

        public async Task<InvestmentFunds> GetInvestmentFunds()
        {
            return await _fundTransactionHistoryManager.GetInvestmentFunds();
        }

        public async Task Investment(decimal amount, string customerUserName = null)
        {
            await _fundTransactionHistoryManager.Investment(amount, customerUserName);
        }

        public async Task Withdrawal(string userName, decimal amount, decimal fee, WithdrawalType? type)
        {
            await _fundTransactionHistoryManager.Withdrawal(userName, amount, fee, type);
        }

        public async Task ChangeKVRR(int newKVRRId)
        {
            await _fundTransactionHistoryManager.ChangeKVRR(newKVRRId);
        }

        public async Task ApproveBalanceFund(int fundId)
        {
            await _fundTransactionHistoryManager.ApproveBalanceFund(fundId);
        }

        public async Task<bool> ApproveFundPercent(int portfolioId)
        {
          return  await _fundTransactionHistoryManager.ApproveFundPercent(portfolioId);
        }
    }
}
