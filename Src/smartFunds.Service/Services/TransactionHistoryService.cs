using AutoMapper;
using smartFunds.Business;
using smartFunds.Business.Common;
using smartFunds.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface ITransactionHistoryService
    {
        Task<TransactionHistoryModel> GetTransactionHistory(int? id);
        Task<List<TransactionHistoryModel>> GetAllTransactionHistory(string userId, SearchTransactionHistory searchTransactionHistory = null);
        Task<ListTransactionHistoryModel> GetListTransactionHistory(int pageSize, int pageIndex, string userId = null, SearchTransactionHistory searchTransactionHistory = null);
        Task<ListTransactionHistoryModel> GetListTransactionHistoryForTask(int pageSize, int pageIndex, SearchTransactionHistory searchTransactionHistory = null);
        Task<int> GetCountTransactionHistory();
        Task<TransactionHistoryModel> AddTransactionHistory(TransactionHistoryModel transactionHistoryModel);
        Task UpdateTransactionHistory(TransactionHistoryModel transactionHistoryModel);
        Task UpdateStatusTransactionHistory(int id, TransactionStatus status);
        Task<byte[]> ExportTransactionHistory(SearchTransactionHistory searchTransactionHistory = null);
        Task<byte[]> ExportDealCustom(SearchTransactionHistory searchTransactionHistory = null);
        Task<PropertyFluctuations> GetPropertyFluctuations(string userId, DateTime dateFrom, DateTime dateTo);
        Task<StatisticModel> GetStatisticAsync(SearchTransactionHistory searchTransactionHistory = null);
    }

    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionHistoryManager _transactionHistoryManager;
        public TransactionHistoryService(IMapper mapper, ITransactionHistoryManager transactionHistoryManager)
        {
            _mapper = mapper;
            _transactionHistoryManager = transactionHistoryManager;
        }

        public async Task<TransactionHistoryModel> GetTransactionHistory(int? id)
        {
            var transactionHistory = await _transactionHistoryManager.GetTransactionHistory(id);
            var transactionHistoryModel = _mapper.Map<TransactionHistoryModel>(transactionHistory);
            return transactionHistoryModel;
        }

        public async Task<ListTransactionHistoryModel> GetListTransactionHistory(int pageSize, int pageIndex, string userId = null, SearchTransactionHistory searchTransactionHistory = null)
        {
            var listTransactionHistoryModel = new ListTransactionHistoryModel();
            var listTransactionHistory = await _transactionHistoryManager.GetListTransactionHistory(pageSize, pageIndex, userId, searchTransactionHistory);
            var allTransactionHistory = await _transactionHistoryManager.GetTotalTransactionHistory(userId, searchTransactionHistory);
            listTransactionHistoryModel.ListTransactionHistory = _mapper.Map<List<TransactionHistory>, List<TransactionHistoryModel>>(listTransactionHistory);
            listTransactionHistoryModel.TotalCount = allTransactionHistory;
            return listTransactionHistoryModel;
        }

        public async Task<List<TransactionHistoryModel>> GetAllTransactionHistory(string userId, SearchTransactionHistory searchTransactionHistory = null)
        {
            var allTransactionHistory = await _transactionHistoryManager.GetAllTransactionHistory(userId, searchTransactionHistory);
            return _mapper.Map<List<TransactionHistory>, List<TransactionHistoryModel>>(allTransactionHistory);
        }

        public async Task<int> GetCountTransactionHistory()
        {
            var allTransactionHistory = await _transactionHistoryManager.GetTotalTransactionHistory();
            
            return allTransactionHistory;
        }

        public async Task<ListTransactionHistoryModel> GetListTransactionHistoryForTask(int pageSize, int pageIndex, SearchTransactionHistory searchTransactionHistory = null)
        {   // for accountant
            var listTransactionHistoryModel = new ListTransactionHistoryModel();
            var listTransactionHistory = await _transactionHistoryManager.GetListTransactionHistoryForTask(pageSize, pageIndex, searchTransactionHistory);

            listTransactionHistoryModel.ListTransactionHistory = _mapper.Map<List<TransactionHistory>, List<TransactionHistoryModel>>(listTransactionHistory);
            listTransactionHistoryModel.TotalCount = _transactionHistoryManager.GetListTransactionHistoryForTask(searchTransactionHistory).Result.Count;
            return listTransactionHistoryModel;
        }

        public async Task UpdateStatusTransactionHistory(int id, TransactionStatus status)
        {   // for accountant
            status = TransactionStatus.Success;
            await _transactionHistoryManager.UpdateStatusTransactionHistory(id, status);
        }

        public async Task<TransactionHistoryModel> AddTransactionHistory(TransactionHistoryModel transactionHistoryModel)
        {
            var newTransactionHistory = _mapper.Map<TransactionHistory>(transactionHistoryModel);
            var addTransactionHistory = await _transactionHistoryManager.AddTransactionHistory(newTransactionHistory);
            return _mapper.Map<TransactionHistoryModel>(addTransactionHistory);
        }

        public async Task UpdateTransactionHistory(TransactionHistoryModel transactionHistoryModel)
        {
            var transactionHistory = _mapper.Map<TransactionHistory>(transactionHistoryModel);
            await _transactionHistoryManager.UpdateTransactionHistory(transactionHistory);
        }

        public async Task<byte[]> ExportTransactionHistory(SearchTransactionHistory searchTransactionHistory = null)
        {
            return await _transactionHistoryManager.ExportTransactionHistory(searchTransactionHistory);
        }

        public async Task<byte[]> ExportDealCustom(SearchTransactionHistory searchTransactionHistory = null)
        {
            return await _transactionHistoryManager.ExportDealCustom(searchTransactionHistory);
        }

        public async Task<PropertyFluctuations> GetPropertyFluctuations(string userId, DateTime dateFrom, DateTime dateTo)
        {
            return await _transactionHistoryManager.GetPropertyFluctuations(userId, dateFrom, dateTo);
        }

        public async Task<StatisticModel> GetStatisticAsync(SearchTransactionHistory searchTransactionHistory = null)
        {
            return await _transactionHistoryManager.GetStatisticAsync(searchTransactionHistory);
        }
    }
}
