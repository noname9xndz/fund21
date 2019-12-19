using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using smartFunds.Model.Common;
using LinqKit;
using OfficeOpenXml;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using System.Globalization;

namespace smartFunds.Business.Common
{
    public interface ITransactionHistoryManager
    {
        Task<TransactionHistory> GetTransactionHistory(int? id);
        Task<List<TransactionHistory>> GetListTransactionHistory(int pageSize, int pageIndex, string userId = null, SearchTransactionHistory searchTransactionHistory = null);
        Task<List<TransactionHistory>> GetListTransactionHistoryForTask(int pageSize, int pageIndex, SearchTransactionHistory searchTransactionHistory = null);
        Task<List<TransactionHistory>> GetListTransactionHistoryForTask(SearchTransactionHistory searchTransactionHistory = null);
        Task UpdateStatusTransactionHistory(int id, TransactionStatus status);
        Task<List<TransactionHistory>> GetAllTransactionHistory(string userId = null, SearchTransactionHistory searchTransactionHistory = null);
        Task<TransactionHistory> AddTransactionHistory(TransactionHistory transactionHistory, string userName = null);
        Task UpdateTransactionHistory(TransactionHistory transactionHistory);
        Task<byte[]> ExportTransactionHistory(SearchTransactionHistory searchTransactionHistory = null);
        Task<byte[]> ExportDealCustom(SearchTransactionHistory searchTransactionHistory = null);
        Task<PropertyFluctuations> GetPropertyFluctuations(string userId, DateTime dateFrom, DateTime dateTo);
        Task<StatisticModel> GetStatisticAsync(SearchTransactionHistory searchTransactionHistory = null);
    }
    public class TransactionHistoryManager : ITransactionHistoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;

        public TransactionHistoryManager(IUnitOfWork unitOfWork, IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<TransactionHistory> GetTransactionHistory(int? id)
        {
            if (id == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var transactionHistory = await _unitOfWork.TransactionHistoryRepository.GetAsync(m => m.Id == id, "User");
                if (transactionHistory != null)
                {
                    return transactionHistory;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionHistory>> GetListTransactionHistory(int pageSize, int pageIndex, string userId = null, SearchTransactionHistory searchTransactionHistory = null)
        {
            if (pageSize < 1 || pageIndex < 1)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var predicate = SetPredicate(searchTransactionHistory);

                var listTransactionHistory = new List<TransactionHistory>();

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    listTransactionHistory = (await _unitOfWork.TransactionHistoryRepository.FindByAsync(i => i.User.Id == userId && i.TransactionType != TransactionType.None, "User"))
                                    .Where(predicate).OrderByDescending(i => i.TransactionDate)
                                    .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    listTransactionHistory = (await _unitOfWork.TransactionHistoryRepository.GetAllAsync("User")).Where(i => i.TransactionType != TransactionType.None)
                                    .Where(predicate).OrderByDescending(i => i.TransactionDate)
                                    .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                return listTransactionHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionHistory>> GetListTransactionHistoryForTask(int pageSize, int pageIndex, SearchTransactionHistory searchTransactionHistory = null)
        {   // for accountant
            if (pageSize < 1 || pageIndex < 1)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var predicate = SetPredicateTask(searchTransactionHistory);
                var listTransactionHistory = new List<TransactionHistory>();

                listTransactionHistory = (await _unitOfWork.TransactionHistoryRepository.GetAllAsync("User")).Where(i => i.TransactionType == TransactionType.Withdrawal && i.Status == TransactionStatus.Processing)
                                .OrderBy(i => i.TransactionDate)
                                .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                //return listTransactionHistory.GroupBy(x => x.UserId, (k, g) => g.Aggregate((a, x) => (x.TotalWithdrawal > a.TotalWithdrawal) ? x : a)).Where(predicate).ToList();
                return listTransactionHistory.Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionHistory>> GetListTransactionHistoryForTask(SearchTransactionHistory searchTransactionHistory = null)
        {   // for accountant
            try
            {
                var predicate = SetPredicateTask(searchTransactionHistory);
                var listTransactionHistory = new List<TransactionHistory>();

                listTransactionHistory = (await _unitOfWork.TransactionHistoryRepository.GetAllAsync("User")).Where(i => i.TransactionType == TransactionType.Withdrawal && i.Status == TransactionStatus.Processing)
                                .OrderBy(i => i.TransactionDate).ToList();

                return listTransactionHistory.GroupBy(x => x.UserId, (k, g) => g.Aggregate((a, x) => (x.TotalWithdrawal > a.TotalWithdrawal) ? x : a)).Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateStatusTransactionHistory(int id, TransactionStatus status)
        {   // for accountant
            try
            {
                var listTransactionHistory = await _unitOfWork.TransactionHistoryRepository.FindByAsync(m => m.Id == id && m.Status == TransactionStatus.Processing && m.TransactionType == TransactionType.Withdrawal);
                foreach (var itm in listTransactionHistory)
                {
                    itm.Status = status;
                    itm.TransactionDate = DateTime.Now;
                }
                _unitOfWork.TransactionHistoryRepository.BulkUpdate(listTransactionHistory);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionHistory>> GetAllTransactionHistory(string userId = null, SearchTransactionHistory searchTransactionHistory = null)
        {
            try
            {
                var predicate = SetPredicate(searchTransactionHistory);

                var listTransactionHistory = new List<TransactionHistory>();

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    listTransactionHistory = (await _unitOfWork.TransactionHistoryRepository.FindByAsync(i => i.User.Id == userId && i.TransactionType != TransactionType.None, "User"))
                                    .Where(predicate).OrderByDescending(i => i.TransactionDate)
                                    .ToList();
                }
                else
                {
                    listTransactionHistory = (await _unitOfWork.TransactionHistoryRepository.GetAllAsync("User")).Where(i => i.TransactionType != TransactionType.None)
                                    .Where(predicate).OrderByDescending(i => i.TransactionDate)
                                    .ToList();
                }

                return listTransactionHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<byte[]> ExportTransactionHistory(SearchTransactionHistory searchTransactionHistory = null)
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.CustomerName,
                    Model.Resources.Common.PhoneNumber,
                    Model.Resources.Common.Type,
                    Model.Resources.Common.Amount,
                    Model.Resources.Common.Status,
                    Model.Resources.Common.TimeAction
                };

                var predicate = SetPredicate(searchTransactionHistory);

                var listTransactionHistory = (await _unitOfWork.TransactionHistoryRepository.GetAllAsync("User")).Where(i => i.TransactionType != TransactionType.None)
                                    .Where(predicate).OrderByDescending(i => i.TransactionDate).ToList();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Transaction History");
                    using (var cells = worksheet.Cells[1, 1, 1, 6])
                    {
                        cells.Style.Font.Bold = true;
                    }

                    for (var i = 0; i < comlumHeadrs.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    }

                    var j = 2;
                    foreach (var transactionHistory in listTransactionHistory)
                    {
                        worksheet.Cells["A" + j].Value = transactionHistory.User.FullName;
                        worksheet.Cells["B" + j].Value = transactionHistory.User.PhoneNumber;
                        worksheet.Cells["C" + j].Value = transactionHistory.TransactionType.GetDisplayName();
                        worksheet.Cells["D" + j].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["D" + j].Value = transactionHistory.Amount;
                        worksheet.Cells["E" + j].Value = transactionHistory.Status.GetDisplayName();
                        worksheet.Cells["F" + j].Value = transactionHistory.TransactionDate.ToString("dd/MM/yyyy HH:mm:ss");
                        j++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export Transaction History: " + ex.Message);
            }

        }

        public async Task<byte[]> ExportDealCustom(SearchTransactionHistory searchTransactionHistory = null)
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.STT,
                    Model.Resources.Common.Deal,
                    Model.Resources.Common.FullName,
                    Model.Resources.Common.PhoneNumber,
                    Model.Resources.Common.Amount
                };
                var listExport = await GetListTransactionHistoryForTask(searchTransactionHistory);

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Deal Custom");
                    using (var cells = worksheet.Cells[1, 1, 1, 5])
                    {
                        cells.Style.Font.Bold = true;
                    }

                    for (var i = 0; i < comlumHeadrs.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    }

                    var j = 2;
                    foreach (var transactionHistory in listExport)
                    {
                        worksheet.Cells["A" + j].Value = j - 1;
                        worksheet.Cells["B" + j].Value = Model.Resources.Common.TransfersCustomMoney;
                        worksheet.Cells["C" + j].Value = transactionHistory.User.FullName;
                        worksheet.Cells["D" + j].Value = transactionHistory.User.PhoneNumber;
                        worksheet.Cells["E" + j].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["E" + j].Value = transactionHistory.TotalWithdrawal;
                        j++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export Deal Custom: " + ex.Message);
            }

        }

        private ExpressionStarter<TransactionHistory> SetPredicateTask(SearchTransactionHistory searchTransactionHistory)
        {
            var predicate = PredicateBuilder.New<TransactionHistory>(true);

            if (searchTransactionHistory != null)
            {
                if (!string.IsNullOrWhiteSpace(searchTransactionHistory.CustomerName))
                {
                    var customerName = searchTransactionHistory.CustomerName.Trim().ToLower();
                    predicate = predicate.And(i => i.User != null && !string.IsNullOrWhiteSpace(i.User.FullName) && (i.User.FullName.ToLower().Contains(customerName) || i.User.PhoneNumber.Contains(customerName)));
                }
                if (!string.IsNullOrWhiteSpace(searchTransactionHistory.AmountFrom) && Decimal.TryParse(searchTransactionHistory.AmountFrom, out decimal amountFrom))
                {
                    predicate = predicate.And(i => Decimal.Round(i.TotalWithdrawal) >= Decimal.Round(amountFrom));
                }
                if (!string.IsNullOrWhiteSpace(searchTransactionHistory.AmountTo) && Decimal.TryParse(searchTransactionHistory.AmountTo, out decimal amountTo))
                {
                    predicate = predicate.And(i => Decimal.Round(i.TotalWithdrawal) <= Decimal.Round(amountTo));
                }
            }

            return predicate;
        }


        private ExpressionStarter<TransactionHistory> SetPredicate(SearchTransactionHistory searchTransactionHistory)
        {
            var predicate = PredicateBuilder.New<TransactionHistory>(true);

            if (searchTransactionHistory != null)
            {
                if (!string.IsNullOrWhiteSpace(searchTransactionHistory.CustomerName))
                {
                    var customerName = searchTransactionHistory.CustomerName.Trim().ToLower();
                    predicate = predicate.And(i => i.User != null && !string.IsNullOrWhiteSpace(i.User.FullName) && i.User.FullName.ToLower().Contains(customerName));
                }
                if (!string.IsNullOrWhiteSpace(searchTransactionHistory.PhoneNumber))
                {
                    var phone = searchTransactionHistory.PhoneNumber.Trim().ToLower();
                    predicate = predicate.And(i => i.User != null && !string.IsNullOrWhiteSpace(i.User.PhoneNumber) && i.User.PhoneNumber.Contains(phone));
                }
                if (!string.IsNullOrWhiteSpace(searchTransactionHistory.EmailAddress))
                {
                    var email = searchTransactionHistory.EmailAddress.Trim().ToLower();
                    predicate = predicate.And(i => i.User != null && !string.IsNullOrWhiteSpace(i.User.Email) && i.User.Email.ToLower().Contains(email));
                }
                if (searchTransactionHistory.TransactionType != TransactionType.None)
                {
                    predicate = predicate.And(i => i.TransactionType == searchTransactionHistory.TransactionType);
                }
                if (searchTransactionHistory.Status != TransactionStatus.None)
                {
                    predicate = predicate.And(i => i.Status == searchTransactionHistory.Status);
                }
                if (!string.IsNullOrWhiteSpace(searchTransactionHistory.AmountFrom) && Decimal.TryParse(searchTransactionHistory.AmountFrom, out decimal amountFrom) && amountFrom > 0)
                {
                    predicate = predicate.And(i => Decimal.Round(i.Amount) >= Decimal.Round(amountFrom));
                }
                if (!string.IsNullOrWhiteSpace(searchTransactionHistory.AmountTo) && Decimal.TryParse(searchTransactionHistory.AmountTo, out decimal amountTo) && amountTo > 0)
                {
                    predicate = predicate.And(i => Decimal.Round(i.Amount) <= Decimal.Round(amountTo));
                }
                var transactionDateFrom = new DateTime();
                if (!string.IsNullOrWhiteSpace(searchTransactionHistory.TransactionDateFrom) && DateTime.TryParseExact(searchTransactionHistory.TransactionDateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out transactionDateFrom))
                {
                    predicate = predicate.And(i => i.TransactionDate.Date >= transactionDateFrom);
                }
                var transactionDateTo = new DateTime();
                if (!string.IsNullOrWhiteSpace(searchTransactionHistory.TransactionDateTo) && DateTime.TryParseExact(searchTransactionHistory.TransactionDateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out transactionDateTo))
                {
                    predicate = predicate.And(i => i.TransactionDate.Date <= transactionDateTo);
                }
            }

            return predicate;
        }

        public async Task<TransactionHistory> AddTransactionHistory(TransactionHistory transactionHistory, string userName = null)
        {
            try
            {
                var currentUser = string.IsNullOrWhiteSpace(userName)? await _userManager.GetCurrentUser() : await _userManager.GetUserByName(userName);
                transactionHistory.UserId = currentUser.Id;
                transactionHistory.CurrentAccountAmount = currentUser.CurrentAccountAmount;
                var savedTransactionHistory = _unitOfWork.TransactionHistoryRepository.Add(transactionHistory);
                await _unitOfWork.SaveChangesAsync();
                return savedTransactionHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateTransactionHistory(TransactionHistory transactionHistory)
        {
            try
            {
                _unitOfWork.TransactionHistoryRepository.Update(transactionHistory);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PropertyFluctuations> GetPropertyFluctuations(string userId, DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom > dateTo || string.IsNullOrWhiteSpace(userId))
            {
                throw new InvalidParameterException();
            }
            try
            {
                var propertyFluctuations = new PropertyFluctuations();

                var listProperty = (await _unitOfWork.TransactionHistoryRepository.FindByAsync(i => i.User.Id == userId && i.TransactionDate.Date >= dateFrom.Date && i.TransactionDate.Date <= dateTo.Date, "User"))
                                            .OrderByDescending(i => i.TransactionDate)
                                            .GroupBy(i => i.TransactionDate.Date)
                                            .Select(g => new PropertyFluctuationItem()
                                            {
                                                Date = g.Key,
                                                Amount = g.First().CurrentAccountAmount
                                            })
                                            .ToList();
                if (listProperty.Any())
                {
                    //var beginDate = listProperty.Last();
                    var endDate = listProperty.First();
                    //for (DateTime date = dateFrom; date < beginDate.Date; date = date.AddDays(1))
                    //{
                    //    var propertyFluctuationItem = new PropertyFluctuationItem()
                    //    {
                    //        Date = date,
                    //        Amount = 0
                    //    };
                    //    listProperty.Add(propertyFluctuationItem);
                    //}
                    if (endDate.Date < dateTo)
                    {
                        var propertyFluctuationItem = new PropertyFluctuationItem()
                        {
                            Date = dateTo,
                            Amount = endDate.Amount
                        };
                        listProperty.Add(propertyFluctuationItem);
                    }
                }

                propertyFluctuations.ListProperty = listProperty.OrderBy(i => i.Date).ToList();

                return propertyFluctuations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StatisticModel> GetStatisticAsync(SearchTransactionHistory searchTransactionHistory = null)
        {
            var result = new StatisticModel();

            var predicate = SetPredicate(searchTransactionHistory);

            var listTransactionHistoryAccountFee = (await _unitOfWork.TransactionHistoryRepository.GetAllAsync("User")).Where(i => i.TransactionType == TransactionType.AccountFee)?.Where(predicate)?.ToList();
            if(listTransactionHistoryAccountFee != null)
            {
                result.TotalAccountFee = listTransactionHistoryAccountFee.Sum(i => i.Amount);
            }

            var listTransactionHistoryWithdrawalFee = (await _unitOfWork.TransactionHistoryRepository.GetAllAsync("User")).Where(i => i.TransactionType == TransactionType.WithdrawalFee)?.Where(predicate)?.ToList();
            if (listTransactionHistoryAccountFee != null)
            {
                result.TotalWithdrawalFee = listTransactionHistoryWithdrawalFee.Sum(i => i.Amount);
            }

            var listTransactionHistoryQuickWithdrawal = (await _unitOfWork.TransactionHistoryRepository.GetAllAsync("User")).Where(i => i.TransactionType == TransactionType.Withdrawal && i.WithdrawalType != null && i.WithdrawalType == WithdrawalType.Quick)?.Where(predicate)?.ToList();
            if (listTransactionHistoryAccountFee != null)
            {
                result.TotalQuickWithdrawal = listTransactionHistoryQuickWithdrawal.Sum(i => i.Amount);
            }

            return result;
        }
    }
}
