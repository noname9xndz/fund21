using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using smartFunds.Business.Common;
using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using smartFunds.Infrastructure.Services;
using System.Globalization;
using static smartFunds.Common.Constants;
using smartFunds.Business.Admin;
using Hangfire;

namespace smartFunds.Business
{
    public interface IFundTransactionHistoryManager
    {
        Task<FundTransactionHistory> AddFundTransactionHistory(FundTransactionHistory fundTransactionHistory);
        Task UpdateFundTransactionHistory(FundTransactionHistory fundTransactionHistory);
        Task<List<FundTransactionHistory>> GetFundTransactionHistoryByFundId(int fundId, smartFunds.Common.EditStatus? status = null);
        Task<List<FundTransactionHistory>> GetsFundTransactionHistory(int pageSize, int pageIndex, SearchBalanceFund searchBalanceFund = null);
        Task<List<FundTransactionHistory>> GetsFundTransactionHistory(SearchBalanceFund searchBalanceFund = null);
        Task<List<FundTransactionHistory>> GetFundTransactionHistoryByFundId(int fundId, int pageIndex, int pageSize, smartFunds.Common.EditStatus? status = null);
        Task<List<FundTransactionHistory>> GetAllFundTransactionHistory();
        Task<IQueryable<FundTransactionHistory>> GetListBalanceFund(smartFunds.Common.EditStatus status);
        Task<FundTransactionHistory> GetBalanceFund(int fundId, smartFunds.Common.EditStatus status);
        Task<byte[]> ExportBalanceFund(smartFunds.Common.EditStatus status);
        Task<byte[]> ExportDealFund(SearchBalanceFund searchBalanceFund = null);
        Task<int> GetCountUserFund(int fundId);
        Task<decimal> GetTotalAmountInvestFund(int fundId);
        Task<InvestmentFunds> GetInvestmentFunds();
        Task Investment(decimal amount, string customerUserName = null,  string objectId = null);
        Task Withdrawal(string userName, decimal amount, decimal fee, WithdrawalType? type, bool withdrawalAll = false, string objectId = null);
        Task ChangeKVRR(int newKVRRId);
        Task ApproveBalanceFund(int fundId);
        Task StartBalancing(int fundId);
        Task<bool> ApproveFundPercent(int portfolioId);
        Task WithdrawRollback(decimal amount, string customerUserName);
    }
    public class FundTransactionHistoryManager : IFundTransactionHistoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;
        private readonly ICustomerManager _customerManager;
        private readonly IFundManager _fundManager;
        private readonly ITransactionHistoryManager _transactionHistoryManager;
        private readonly IMaintainingFeeManager _maintainingFeeManager;
        private readonly IFundPurchaseFeeManager _fundPurchaseFeeManager;
        private readonly IFundSellFeeManager _fundSellFeeManager;
        private NLog.Logger _logger;

        public FundTransactionHistoryManager(IUnitOfWork unitOfWork, IUserManager userManager, IFundManager fundManager, ITransactionHistoryManager transactionHistoryManager, ICustomerManager customerManager, IMaintainingFeeManager maintainingFeeManager, IFundPurchaseFeeManager fundPurchaseFeeManager, IFundSellFeeManager fundSellFeeManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _fundManager = fundManager;
            _transactionHistoryManager = transactionHistoryManager;
            _customerManager = customerManager;
            _maintainingFeeManager = maintainingFeeManager;
            _fundPurchaseFeeManager = fundPurchaseFeeManager;
            _fundSellFeeManager = fundSellFeeManager;
            _logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        }

        public async Task<FundTransactionHistory> AddFundTransactionHistory(FundTransactionHistory fundTransactionHistory)
        {
            try
            {
                if (fundTransactionHistory == null) throw new InvalidParameterException();

                fundTransactionHistory.LastUpdatedBy = _userManager.CurrentUser();
                fundTransactionHistory.LastUpdatedDate = DateTime.Now;
                var fundTransactionHistoryAdded = _unitOfWork.FundTransactionHistoryRepository.Add(fundTransactionHistory);

                await _unitOfWork.SaveChangesAsync();
                return fundTransactionHistoryAdded;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateFundTransactionHistory(FundTransactionHistory fundTransactionHistory)
        {
            try
            {
                if (fundTransactionHistory == null) throw new InvalidParameterException();

                fundTransactionHistory.LastUpdatedBy = _userManager.CurrentUser();
                fundTransactionHistory.LastUpdatedDate = DateTime.Now;
                _unitOfWork.FundTransactionHistoryRepository.Update(fundTransactionHistory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FundTransactionHistory>> GetFundTransactionHistoryByFundId(int fundId, smartFunds.Common.EditStatus? status = null)
        {
            if (fundId < 1)
            {
                throw new InvalidParameterException();
            }

            try
            {
                if (status == null)
                {
                    List<FundTransactionHistory> fundTransactionHistory = (await _unitOfWork.FundTransactionHistoryRepository.FindByAsync(i => i.FundId == fundId, "User,Fund")).OrderByDescending(i => i.TransactionDate).ToList();

                    return fundTransactionHistory;
                }
                else
                {
                    List<FundTransactionHistory> fundTransactionHistory = (await _unitOfWork.FundTransactionHistoryRepository.FindByAsync(i => i.FundId == fundId && i.Status == status, "User,Fund")).OrderByDescending(i => i.TransactionDate).ToList();

                    return fundTransactionHistory;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FundTransactionHistory>> GetsFundTransactionHistory(int pageSize, int pageIndex, SearchBalanceFund searchBalanceFund = null)
        {   // for accountant
            if (pageSize < 1 || pageIndex < 1)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var predicate = SetPredicate(searchBalanceFund);


                var fundTransactionHistory = await GetListBalanceFund(EditStatus.Updating);

                return fundTransactionHistory.Where(i => i.TotalInvestNoOfCertificates != i.TotalWithdrawnNoOfCertificates).Where(predicate).OrderBy(x => x.Fund.Code).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FundTransactionHistory>> GetsFundTransactionHistory(SearchBalanceFund searchBalanceFund = null)
        {   // for accountant
            try
            {
                var predicate = SetPredicate(searchBalanceFund);


                var fundTransactionHistory = await GetListBalanceFund(EditStatus.Updating);

                return fundTransactionHistory.Where(i => i.TotalInvestNoOfCertificates != i.TotalWithdrawnNoOfCertificates).Where(predicate)?.OrderBy(x => x.TransactionDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ExpressionStarter<FundTransactionHistory> SetPredicate(SearchBalanceFund searchBalanceFund)
        {
            var predicate = PredicateBuilder.New<FundTransactionHistory>(true);

            if (searchBalanceFund != null)
            {
                if (searchBalanceFund.CustomerName != 0)
                {
                    if (searchBalanceFund.CustomerName == 1)
                    {
                        predicate = predicate.And(i => (i.TotalInvestNoOfCertificates - i.TotalWithdrawnNoOfCertificates) > 0);
                    }
                    else if (searchBalanceFund.CustomerName == 2)
                    {
                        predicate = predicate.And(i => (i.TotalInvestNoOfCertificates - i.TotalWithdrawnNoOfCertificates) < 0);
                    }
                }
                if (!string.IsNullOrWhiteSpace(searchBalanceFund.AmountFrom) && Decimal.TryParse(searchBalanceFund.AmountFrom, out decimal amountFrom))
                { // ((i.TotalInvestNoOfCertificates - i.TotalWithdrawnNoOfCertificates) * i.Fund.NAV)
                    predicate = predicate.And(i => Decimal.Round((i.TotalInvestNoOfCertificates - i.TotalWithdrawnNoOfCertificates) * i.Fund.NAV) >= Decimal.Round(amountFrom));
                }

                if (!string.IsNullOrWhiteSpace(searchBalanceFund.AmountTo) && Decimal.TryParse(searchBalanceFund.AmountTo, out decimal amountTo))
                {
                    predicate = predicate.And(i => Decimal.Round((i.TotalInvestNoOfCertificates - i.TotalWithdrawnNoOfCertificates) * i.Fund.NAV) <= Decimal.Round(amountTo));
                }
            }

            return predicate;
        }

        public async Task<List<FundTransactionHistory>> GetFundTransactionHistoryByFundId(int fundId, int pageIndex, int pageSize, smartFunds.Common.EditStatus? status = null)
        {
            if (fundId < 1 || pageIndex < 1 || pageSize < 1)
            {
                throw new InvalidParameterException();
            }

            try
            {
                if (status == null)
                {
                    List<FundTransactionHistory> fundTransactionHistory = (await _unitOfWork.FundTransactionHistoryRepository.FindByAsync(i => i.Fund.Id == fundId, "User,Fund"))
                                                                    .OrderByDescending(i => i.TransactionDate)
                                                                    .Skip((pageIndex - 1) * pageSize).Take(pageSize)?.ToList();

                    return fundTransactionHistory;
                }
                else
                {
                    List<FundTransactionHistory> fundTransactionHistory = (await _unitOfWork.FundTransactionHistoryRepository.FindByAsync(i => i.Fund.Id == fundId && i.Status == status, "User,Fund"))
                                                                    .OrderByDescending(i => i.TransactionDate)
                                                                    .Skip((pageIndex - 1) * pageSize).Take(pageSize)?.ToList();

                    return fundTransactionHistory;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FundTransactionHistory>> GetAllFundTransactionHistory()
        {
            try
            {
                List<FundTransactionHistory> allFundTransactionHistory = (await _unitOfWork.FundTransactionHistoryRepository.GetAllAsync("User,Fund"))?.ToList();

                return allFundTransactionHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IQueryable<FundTransactionHistory>> GetListBalanceFund(smartFunds.Common.EditStatus status)
        {
            try
            {

                var fundTransactionHistory = (await _unitOfWork.FundTransactionHistoryRepository.FindByAsync(i => i.Status == status, "User,Fund"))
                                                                        //.OrderByDescending(i => i.TransactionDate)
                                                                        .GroupBy(i => i.FundId)
                                                                        .Select(g => g.OrderByDescending(h => h.TransactionDate).First());

                return fundTransactionHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FundTransactionHistory> GetBalanceFund(int fundId, smartFunds.Common.EditStatus status)
        {
            if (fundId < 1)
            {
                throw new InvalidParameterException();
            }

            try
            {
                return (await GetListBalanceFund(status)).Where(i => i.Fund.Id == fundId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<byte[]> ExportDealFund(SearchBalanceFund searchBalanceFund)
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.STT,
                    Model.Resources.Common.Deal,
                    Model.Resources.Common.Fund,
                    Model.Resources.Common.NoOfCertificate,
                    Model.Resources.Common.DealMoney
                };


                var allFunds = await GetsFundTransactionHistory(searchBalanceFund);

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Deal Fund");
                    using (var cells = worksheet.Cells[1, 1, 1, 5])
                    {
                        cells.Style.Font.Bold = true;
                    }

                    for (var i = 0; i < comlumHeadrs.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    }

                    var j = 2;
                    foreach (var fundTransactionHistory in allFunds)
                    {
                        var noOfCertificates = fundTransactionHistory.TotalInvestNoOfCertificates - fundTransactionHistory.TotalWithdrawnNoOfCertificates;
                        worksheet.Cells["A" + j].Value = j - 1;
                        worksheet.Cells["B" + j].Value = noOfCertificates > 0 ? Model.Resources.Common.Buy : Model.Resources.Common.Sell;
                        worksheet.Cells["C" + j].Value = fundTransactionHistory.Fund.Code;
                        worksheet.Cells["D" + j].Value = decimal.Round(Math.Abs(noOfCertificates), 2);
                        worksheet.Cells["E" + j].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["E" + j].Value = (noOfCertificates * fundTransactionHistory.Fund.NAV);
                        j++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export Deal Fund: " + ex.Message);
            }

        }

        public async Task<byte[]> ExportBalanceFund(smartFunds.Common.EditStatus status)
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.FundCode,
                    Model.Resources.Common.FundName,
                    Model.Resources.Common.NoOfCertificateInvest,
                    Model.Resources.Common.TotalInvestAmount,
                    Model.Resources.Common.NoOfCertificateWithdrawn,
                    Model.Resources.Common.TotalWithdrawnAmount,
                    Model.Resources.Common.BalanceFundAmount
                };


                var allFunds = await GetListBalanceFund(status);

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("BalanceFund");
                    using (var cells = worksheet.Cells[1, 1, 1, 7])
                    {
                        cells.Style.Font.Bold = true;
                    }

                    for (var i = 0; i < comlumHeadrs.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    }

                    var j = 2;
                    foreach (var fundTransactionHistory in allFunds)
                    {
                        worksheet.Cells["A" + j].Value = fundTransactionHistory.Fund.Code;
                        worksheet.Cells["B" + j].Value = fundTransactionHistory.Fund.Title;
                        worksheet.Cells["C" + j].Style.Numberformat.Format = "0.00";
                        worksheet.Cells["C" + j].Value = fundTransactionHistory.TotalInvestNoOfCertificates;
                        worksheet.Cells["D" + j].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["D" + j].Value = fundTransactionHistory.TotalInvestNoOfCertificates * fundTransactionHistory.Fund.NAV;
                        worksheet.Cells["E" + j].Style.Numberformat.Format = "0.00";
                        worksheet.Cells["E" + j].Value = fundTransactionHistory.TotalWithdrawnNoOfCertificates;
                        worksheet.Cells["F" + j].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["F" + j].Value = fundTransactionHistory.TotalWithdrawnNoOfCertificates * fundTransactionHistory.Fund.NAV;
                        worksheet.Cells["G" + j].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["G" + j].Value = ((fundTransactionHistory.TotalInvestNoOfCertificates - fundTransactionHistory.TotalWithdrawnNoOfCertificates) * fundTransactionHistory.Fund.NAV);
                        j++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export Balance Fund: " + ex.Message);
            }

        }

        public async Task<int> GetCountUserFund(int fundId)
        {
            try
            {
                var allUserFundUsed = (await _unitOfWork.UserFundRepository.GetAllAsync("User,Fund"));

                if (fundId > 0)
                {
                    allUserFundUsed = allUserFundUsed.Where(i => i.FundId == fundId);//.ToList();
                }

                var allUserIdUsed = allUserFundUsed.Select(i => i.UserId).Distinct();

                if (allUserFundUsed == null || !allUserFundUsed.Any())
                {
                    return 0;
                }

                return allUserIdUsed.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<decimal> GetTotalAmountInvestFund(int fundId)
        {
            try
            {
                var allUserFundUsed = (await _unitOfWork.UserFundRepository.GetAllAsync("User,Fund"));

                if (fundId > 0)
                {
                    allUserFundUsed = allUserFundUsed?.Where(i => i.FundId == fundId);//.ToList();
                }

                if (allUserFundUsed == null || !allUserFundUsed.Any())
                {
                    return 0;
                }

                var totalAmount = allUserFundUsed.Sum(h => (h.NoOfCertificates ?? 0) * h.Fund.NAV);
 
                return totalAmount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<InvestmentFunds> GetInvestmentFunds()
        {
            try
            {
                var result = new InvestmentFunds();

                var allFund = _fundManager.GetAllFund();
                foreach (var fund in allFund)
                {
                    var investmentFund = new InvestmentFund();
                    investmentFund.FundCode = fund.Code;
                    investmentFund.FundName = fund.Title;
                    investmentFund.TotalAmountUserFund = await GetTotalAmountInvestFund(fund.Id);
                    investmentFund.CountUserFund = await GetCountUserFund(fund.Id);
                    result.ListInvestmentFund.Add(investmentFund);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Investment(decimal amount, string customerUserName = null,string objectId = null)
        {
            try
            {
                var flag = 1;

                var currentUser = string.IsNullOrWhiteSpace(customerUserName) ? await _userManager.GetCurrentUser() : await _userManager.GetUserByName(customerUserName);
                var portfolioId = (await _unitOfWork.KVRRPortfolioRepository.GetAsync(i => i.KVRRId == currentUser.KVRR.Id))?.PortfolioId;
                
                if (portfolioId != null)
                {
                    var listPortfolioFund = await _unitOfWork.PortfolioFundRepository.FindByAsync(i => i.PortfolioId == portfolioId && i.FundPercent != null && i.FundPercent != 0, "Fund");

                    _logger.Info($"{currentUser.UserName} invests {amount}: START");
                    using (var dbContextTransaction = _unitOfWork.GetCurrentContext().Database.BeginTransaction())
                    {
                        foreach (var item in listPortfolioFund)
                        {
                            var history = new FundTransactionHistory();
                            var fund = item.Fund;
                            history.UserId = currentUser.Id;
                            history.FundId = fund.Id;
                            var noOfCertificates = (amount * (decimal)item.FundPercent / 100) / fund.NAV;
                            history.NoOfCertificates = noOfCertificates;

                            var currentFundTransactionHistory = (await GetFundTransactionHistoryByFundId(item.FundId, EditStatus.Updating)).OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                            if (currentFundTransactionHistory != null)
                            {
                                history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates + noOfCertificates;
                                history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates;
                            }
                            else
                            {
                                history.TotalInvestNoOfCertificates = noOfCertificates;
                                history.TotalWithdrawnNoOfCertificates = 0;
                            }

                            history.TransactionType = TransactionType.Investment;
                            history.TransactionDate = DateTime.Now;
                            history.Status = EditStatus.Updating;

                            history.LastUpdatedBy = _userManager.CurrentUser();
                            history.LastUpdatedDate = DateTime.Now;
                            _unitOfWork.FundTransactionHistoryRepository.Add(history);

                            flag = await _unitOfWork.SaveChangesAsync();
                            //await AddFundTransactionHistory(history);

                            var currentUserFund = await _unitOfWork.UserFundRepository.GetAsync(i => i.UserId == currentUser.Id && i.FundId == item.FundId);
                            if (currentUserFund != null)
                            {
                                currentUserFund.NoOfCertificates = (decimal)currentUserFund.NoOfCertificates + noOfCertificates;
                                currentUserFund.EditStatus = EditStatus.Updating;
                                _unitOfWork.UserFundRepository.Update(currentUserFund);
                            }
                            else
                            {
                                var userFund = new UserFund();
                                userFund.UserId = currentUser.Id;
                                userFund.FundId = fund.Id;
                                userFund.NoOfCertificates = noOfCertificates;
                                userFund.EditStatus = EditStatus.Updating;
                                _unitOfWork.UserFundRepository.Add(userFund);
                            }
                            flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                        }
                        var oldCurrentAccountAmount = currentUser.CurrentAccountAmount;
                        currentUser.CurrentAccountAmount += amount;
                        currentUser.InitialInvestmentAmount += amount;

                        var oldInvestmentAmount = currentUser.CurrentInvestmentAmount;
                        var oldAdjustmentFactor = currentUser.AdjustmentFactor;
                        currentUser.CurrentInvestmentAmount += amount;

                        if(oldCurrentAccountAmount > 0)
                        {
                            currentUser.AdjustmentFactor = (oldInvestmentAmount != 0 && oldAdjustmentFactor != 0) || currentUser.CurrentInvestmentAmount == 0 ? currentUser.CurrentInvestmentAmount / oldInvestmentAmount * oldAdjustmentFactor : -0.000000000005m;
                        }
                        else
                        {
                            currentUser.AdjustmentFactor = 1;
                        }

                        _unitOfWork.UserRepository.Update(currentUser);
                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                        //await _userManager.UpdateUser(currentUser);

                        var transactionHistory = new TransactionHistory();
                        transactionHistory.UserId = currentUser.Id;
                        transactionHistory.CurrentAccountAmount = currentUser.CurrentAccountAmount;
                        transactionHistory.TransactionType = TransactionType.Investment;
                        transactionHistory.Amount = amount;
                        transactionHistory.Status = TransactionStatus.Success;
                        transactionHistory.TransactionDate = DateTime.Now;
                        transactionHistory.ObjectId = objectId;

                        _unitOfWork.TransactionHistoryRepository.Add(transactionHistory);
                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                        //await _transactionHistoryManager.AddTransactionHistory(transactionHistory, currentUser.UserName);

                        var investment = new Investment();
                        investment.Amount = amount;
                        investment.RemainAmount = amount;
                        investment.UserId = currentUser.Id;
                        investment.DateInvestment = DateTime.Now;
                        _unitOfWork.InvestmentRepository.Add(investment);
                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;


                        if (flag != 0)
                        {
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            throw new ApplicationException("Investment: user " + currentUser.UserName + " (amount = " + amount + ") error not update database");
                        }

                        _logger.Info($"{currentUser.UserName} invests {amount}: DONE");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"{customerUserName} invests {amount}: "  + ex.Message);
                throw ex;
            }
        }

        public async Task Withdrawal(string userName, decimal amount, decimal fee, WithdrawalType? type, bool withdrawalAll = false,string objectId = null)
        {
            _logger.Info($"{userName} withdraws amount: {amount} fee: {fee} type: {type.ToString()} withdrawalAll: {withdrawalAll}");
            try
            {
                using (var dbContextTransaction = _unitOfWork.GetCurrentContext().Database.BeginTransaction())
                {
                    var flag = 1;
                    var totalAmount = amount + fee;
                    var currentUser = await _userManager.GetUserByName(userName);
                    var portfolioId = (await _unitOfWork.KVRRPortfolioRepository.GetAsync(i => i.KVRRId == currentUser.KVRR.Id))?.PortfolioId;
                    if (portfolioId != null)
                    {
                        var listPortfolioFund = (await _unitOfWork.PortfolioFundRepository.FindByAsync(i => i.PortfolioId == portfolioId && i.FundPercent != null && i.FundPercent != 0)).ToList();
                        foreach (var item in listPortfolioFund)
                        {
                            var history = new FundTransactionHistory();
                            var fund = _fundManager.GetFundById(item.FundId);
                            history.UserId = currentUser.Id;
                            history.FundId = fund.Id;
                            var noOfCertificates = (totalAmount * (decimal)item.FundPercent / 100) / fund.NAV;
                            history.NoOfCertificates = noOfCertificates;

                            var currentFundTransactionHistory = (await GetFundTransactionHistoryByFundId(item.FundId, EditStatus.Updating)).OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                            if (currentFundTransactionHistory != null)
                            {
                                history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates;
                                history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates + noOfCertificates;
                            }
                            else
                            {
                                history.TotalInvestNoOfCertificates = 0;
                                history.TotalWithdrawnNoOfCertificates = noOfCertificates;
                            }

                            history.TransactionType = TransactionType.Withdrawal;
                            history.TransactionDate = DateTime.Now;
                            history.Status = EditStatus.Updating;

                            history.LastUpdatedBy = _userManager.CurrentUser();
                            history.LastUpdatedDate = DateTime.Now;
                            _unitOfWork.FundTransactionHistoryRepository.Add(history);

                            flag = await _unitOfWork.SaveChangesAsync();
                            //await AddFundTransactionHistory(history);

                            var currentUserFund = await _unitOfWork.UserFundRepository.GetAsync(i => i.UserId == currentUser.Id && i.FundId == item.FundId);
                            if (currentUserFund != null)
                            {
                                if ((decimal)currentUserFund.NoOfCertificates - noOfCertificates < (decimal)0.01 || (withdrawalAll == true && type == WithdrawalType.Manually))
                                {
                                    _unitOfWork.UserFundRepository.Delete(currentUserFund);
                                }
                                else
                                {
                                    currentUserFund.NoOfCertificates = (decimal)currentUserFund.NoOfCertificates - noOfCertificates;
                                    currentUserFund.EditStatus = EditStatus.Updating;
                                    _unitOfWork.UserFundRepository.Update(currentUserFund);
                                }

                                flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                            }
                        }
                        var user = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userName && !u.IsDeleted);
                        if (withdrawalAll == true && type == WithdrawalType.Manually)
                        {
                            user.CurrentAccountAmount = 0;
                        }
                        else
                        {
                            user.CurrentAccountAmount -= totalAmount;
                        }

                        var oldInvestmentAmount = user.CurrentInvestmentAmount;
                        var oldAdjustmentFactor = user.AdjustmentFactor;
                        if (user.CurrentAccountAmount > 0)
                        {
                            user.CurrentInvestmentAmount -= totalAmount;
                            if(user.CurrentInvestmentAmount == 0)
                            {
                                user.CurrentInvestmentAmount = -0.00001m;
                            }
                            user.AdjustmentFactor = (oldInvestmentAmount != 0 && oldAdjustmentFactor != 0) || user.CurrentInvestmentAmount == 0 ? user.CurrentInvestmentAmount / oldInvestmentAmount * oldAdjustmentFactor : -0.000000000005m;
                        }
                        else
                        {
                            user.CurrentInvestmentAmount = -0.00001m;
                            user.AdjustmentFactor = 1;
                        }

                        user.AmountWithdrawn += totalAmount;

                        user.KVRRId = currentUser.KVRR.Id;

                        _unitOfWork.UserRepository.Update(user);
                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                        //await _userManager.UpdateUser(user);

                        var transactionHistory = new TransactionHistory();
                        transactionHistory.UserId = currentUser.Id;
                        transactionHistory.CurrentAccountAmount = user.CurrentAccountAmount;

                        var feeTransactionHistory = new TransactionHistory();
                        feeTransactionHistory.UserId = currentUser.Id;
                        feeTransactionHistory.CurrentAccountAmount = user.CurrentAccountAmount;

                        if (amount > 0)
                        {
                            transactionHistory.Amount = amount;
                            transactionHistory.TransactionType = TransactionType.Withdrawal;
                            transactionHistory.ObjectId = objectId;

                            if (type == null)
                            {
                                transactionHistory.Status = TransactionStatus.Success;
                                transactionHistory.RemittanceStatus = RemittanceStatus.Success;
                            }
                            else if (type == WithdrawalType.Quick)
                            {
                                transactionHistory.Status = TransactionStatus.Success;
                                transactionHistory.RemittanceStatus = RemittanceStatus.None;
                            }
                            else
                            {
                                transactionHistory.Status = TransactionStatus.Processing;
                                transactionHistory.RemittanceStatus = RemittanceStatus.Success;
                            }

                            feeTransactionHistory.Amount = fee;
                            feeTransactionHistory.TransactionType = TransactionType.WithdrawalFee;
                            feeTransactionHistory.Status = TransactionStatus.Success;
                            feeTransactionHistory.RemittanceStatus = RemittanceStatus.Success;
                        }
                        else
                        {
                            transactionHistory.Amount = fee;
                            transactionHistory.TransactionType = TransactionType.AccountFee;
                            transactionHistory.Status = TransactionStatus.Success;
                        }

                        transactionHistory.TransactionDate = DateTime.Now;

                        feeTransactionHistory.TransactionDate = DateTime.Now;

                        var currentTransactionHistory = (await _transactionHistoryManager.GetAllTransactionHistory(currentUser.Id, new SearchTransactionHistory() { TransactionType = TransactionType.Withdrawal, Status = TransactionStatus.Processing })).OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                        //if (currentTransactionHistory != null)
                        //{
                        //    transactionHistory.TotalWithdrawal = currentTransactionHistory.TotalWithdrawal + amount;
                        //}
                        //else
                        //{
                        //    transactionHistory.TotalWithdrawal = amount;
                        //}
                        transactionHistory.TotalWithdrawal = amount;

                        transactionHistory.WithdrawalType = type;

                        feeTransactionHistory.WithdrawalType = type;

                        _unitOfWork.TransactionHistoryRepository.Add(transactionHistory);
                        _unitOfWork.TransactionHistoryRepository.Add(feeTransactionHistory);

                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                    }

                    if (flag != 0)
                    {
                        dbContextTransaction.Commit();
                    }
                    else
                    {
                        dbContextTransaction.Rollback();
                        throw new ApplicationException("Withdrawal: user " + currentUser.UserName + " (amount = " + amount + ") error not update database");
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ChangeKVRR(int newKVRRId)
        {
            try
            {
                using (var dbContextTransaction = _unitOfWork.GetCurrentContext().Database.BeginTransaction())
                {
                    var flag = 1;
                    var currentUser = await _userManager.GetCurrentUser();
                    if (currentUser.KVRR != null && currentUser.KVRR.Id == newKVRRId) return;

                    var listCurrentUserFund = (await _unitOfWork.UserFundRepository.FindByAsync(i => i.UserId == currentUser.Id, "User,Fund")).ToList();
                    if (listCurrentUserFund == null || !listCurrentUserFund.Any())
                    {
                        //var kvrr = await _unitOfWork.KVRRRepository.GetAsync(x => x.Id == newKVRRId);
                        //currentUser.KVRR = kvrr;
                        //_unitOfWork.UserRepository.Update(currentUser);
                        //flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                        await _userManager.ConfirmKVRR(newKVRRId);
                    }
                    else
                    {
                        var portfolioId = (await _unitOfWork.KVRRPortfolioRepository.GetAsync(i => i.KVRRId == newKVRRId))?.PortfolioId;
                        if (portfolioId != null)
                        {
                            var listCurrentFundIdUsed = listCurrentUserFund.Select(i => i.FundId);
                            var listPortfolioFund = (await _unitOfWork.PortfolioFundRepository.FindByAsync(i => i.PortfolioId == portfolioId && i.FundPercent != null && i.FundPercent != 0)).ToList();
                            foreach (var portfolioFund in listPortfolioFund)
                            {
                                var fund = _fundManager.GetFundById(portfolioFund.FundId);
                                var noOfCertificates = Decimal.Round((currentUser.CurrentAccountAmount * (decimal)portfolioFund.FundPercent / 100) / fund.NAV, 2);
                                if (listCurrentFundIdUsed.Contains(portfolioFund.FundId))
                                {
                                    var userFund = listCurrentUserFund.Where(i => i.FundId == portfolioFund.FundId).FirstOrDefault();
                                    var history = new FundTransactionHistory();
                                    history.UserId = currentUser.Id;
                                    history.FundId = fund.Id;
                                    history.TransactionDate = DateTime.Now;
                                    history.Status = EditStatus.Updating;
                                    var currentFundTransactionHistory = (await GetFundTransactionHistoryByFundId(portfolioFund.FundId, EditStatus.Updating)).OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                                    var currentNoOfCertificates = Decimal.Round((decimal)userFund.NoOfCertificates, 2);
                                    if (currentNoOfCertificates > noOfCertificates)
                                    {
                                        history.NoOfCertificates = currentNoOfCertificates - noOfCertificates;

                                        if (currentFundTransactionHistory != null)
                                        {
                                            history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates;
                                            history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates + currentNoOfCertificates - noOfCertificates;
                                        }
                                        else
                                        {
                                            history.TotalInvestNoOfCertificates = 0;
                                            history.TotalWithdrawnNoOfCertificates = currentNoOfCertificates - noOfCertificates;
                                        }

                                        history.TransactionType = TransactionType.Withdrawal;

                                        history.LastUpdatedBy = _userManager.CurrentUser();
                                        history.LastUpdatedDate = DateTime.Now;
                                        _unitOfWork.FundTransactionHistoryRepository.Add(history);
                                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                        //await AddFundTransactionHistory(history);

                                        userFund.NoOfCertificates = noOfCertificates;
                                        userFund.EditStatus = EditStatus.Updating;
                                        _unitOfWork.UserFundRepository.Update(userFund);
                                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                    }
                                    else if (currentNoOfCertificates < noOfCertificates)
                                    {
                                        //ban
                                        history.NoOfCertificates = noOfCertificates - currentNoOfCertificates;


                                        if (currentFundTransactionHistory != null)
                                        {
                                            history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates + noOfCertificates - currentNoOfCertificates;
                                            history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates;
                                        }
                                        else
                                        {
                                            history.TotalInvestNoOfCertificates = noOfCertificates - currentNoOfCertificates;
                                            history.TotalWithdrawnNoOfCertificates = 0;
                                        }

                                        history.TransactionType = TransactionType.Investment;

                                        await AddFundTransactionHistory(history);

                                        userFund.NoOfCertificates = noOfCertificates;
                                        userFund.EditStatus = EditStatus.Updating;
                                        _unitOfWork.UserFundRepository.Update(userFund);
                                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                    }
                                    
                                }
                                else
                                {
                                    //mua new
                                    var history = new FundTransactionHistory();
                                    history.UserId = currentUser.Id;
                                    history.FundId = fund.Id;
                                    history.TransactionDate = DateTime.Now;
                                    history.Status = EditStatus.Updating;
                                    history.NoOfCertificates = noOfCertificates;
                                    var currentFundTransactionHistory = (await GetFundTransactionHistoryByFundId(portfolioFund.FundId, EditStatus.Updating)).OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                                    if (currentFundTransactionHistory != null)
                                    {
                                        history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates + noOfCertificates;
                                        history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates;
                                    }
                                    else
                                    {
                                        history.TotalInvestNoOfCertificates = noOfCertificates;
                                        history.TotalWithdrawnNoOfCertificates = 0;
                                    }

                                    history.TransactionType = TransactionType.Investment;

                                    history.LastUpdatedBy = _userManager.CurrentUser();
                                    history.LastUpdatedDate = DateTime.Now;
                                    _unitOfWork.FundTransactionHistoryRepository.Add(history);
                                    flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                    //await AddFundTransactionHistory(history);

                                    var userFund = new UserFund();
                                    userFund.UserId = currentUser.Id;
                                    userFund.FundId = fund.Id;
                                    userFund.NoOfCertificates = noOfCertificates;
                                    userFund.EditStatus = EditStatus.Updating;
                                    _unitOfWork.UserFundRepository.Add(userFund);
                                    flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                }
                            }

                            //remove fund used
                            var listPortfolioFundId = listPortfolioFund.Select(i => i.FundId);
                            listCurrentUserFund = listCurrentUserFund.Where(i => !listPortfolioFundId.Contains(i.FundId)).ToList();

                            //ban con lai
                            if (listCurrentUserFund != null && listCurrentUserFund.Any())
                            {
                                foreach (var oldUserFund in listCurrentUserFund)
                                {
                                    var history = new FundTransactionHistory();
                                    history.UserId = currentUser.Id;
                                    history.FundId = oldUserFund.FundId;
                                    history.TransactionDate = DateTime.Now;
                                    history.Status = EditStatus.Updating;
                                    history.NoOfCertificates = (decimal)oldUserFund.NoOfCertificates;
                                    var currentFundTransactionHistory = (await GetFundTransactionHistoryByFundId(oldUserFund.FundId, EditStatus.Updating)).OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                                    if (currentFundTransactionHistory != null)
                                    {
                                        history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates;
                                        history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates + (decimal)oldUserFund.NoOfCertificates;
                                    }
                                    else
                                    {
                                        history.TotalInvestNoOfCertificates = 0;
                                        history.TotalWithdrawnNoOfCertificates = (decimal)oldUserFund.NoOfCertificates;
                                    }

                                    history.TransactionType = TransactionType.Withdrawal;

                                    history.LastUpdatedBy = _userManager.CurrentUser();
                                    history.LastUpdatedDate = DateTime.Now;
                                    _unitOfWork.FundTransactionHistoryRepository.Add(history);
                                    flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                    //await AddFundTransactionHistory(history);
                                }
                                _unitOfWork.UserFundRepository.BulkDelete(listCurrentUserFund);
                                flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                            }

                            //var kvrr = await _unitOfWork.KVRRRepository.GetAsync(x => x.Id == newKVRRId);
                            //currentUser.KVRR = kvrr;
                            //_unitOfWork.UserRepository.Update(currentUser);
                            //flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                            await _userManager.ConfirmKVRR(newKVRRId);
                        }

                        if (flag != 0)
                        {
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            throw new ApplicationException("Change KVRR: user " + currentUser.UserName + " error not update database");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task StartBalancing(int fundId)
        {
            var fund = _unitOfWork.FundRepository.GetFundById(fundId);
            fund.IsBalancing = true;
            _unitOfWork.FundRepository.Update(fund);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task ApproveBalanceFund(int fundId)
        {
            var fund = _unitOfWork.FundRepository.GetFundById(fundId);
            var startTime = DateTime.Now;
            try
            {
                _logger.Info("Start approve balance fund " + fundId);
                fund.IsBalancing = true;
                _unitOfWork.FundRepository.Update(fund);
                await _unitOfWork.SaveChangesAsync();
                using (var dbContextTransaction = _unitOfWork.GetCurrentContext().Database.BeginTransaction())
                {
                    var flag = 1;
                    var listNoOfCertificatesByUser = new Dictionary<string, decimal>();

                    var listFundTransactionHistory = await GetFundTransactionHistoryByFundId(fundId, EditStatus.Updating);
                    var listUpdateFundTransactionHistory = new List<FundTransactionHistory>();

                    var userFunds = listFundTransactionHistory.GroupBy(h => h.UserId);
                    foreach (var uf in userFunds)
                    {
                        listNoOfCertificatesByUser.Add(uf.Key, uf.Where(h => h.TransactionType == TransactionType.Investment).Sum(h => h.NoOfCertificates) - uf.Where(h => h.TransactionType != TransactionType.Investment).Sum(h => h.NoOfCertificates));
                    }
                    await _unitOfWork.FundTransactionHistoryRepository.ExecuteSql(@"Update FundTransactionHistory SET [Status]=" + (int)EditStatus.Success + " WHERE FundId=" + fundId);

                    foreach (var userCer in listNoOfCertificatesByUser)
                    {
                        if (userCer.Value == 0) continue;
                        var user = await _customerManager.GetCustomerById(userCer.Key);
                        var amount = Math.Abs(userCer.Value * fund.NAV);
                        decimal fee = 0;
                        if (userCer.Value > 0)
                        {
                            fee = await _fundPurchaseFeeManager.GetFeeValue(fundId, amount);
                            if (fee == 0) continue;

                            if (user.CurrentAccountAmount - (fee / 100) * amount >= 0)
                            {
                                user.CurrentAccountAmount -= (fee / 100) * amount;

                                var oldInvestmentAmount = user.CurrentInvestmentAmount;
                                var oldAdjustmentFactor = user.AdjustmentFactor;
                                if (user.CurrentAccountAmount > 0)
                                {
                                    user.CurrentInvestmentAmount -= (fee / 100) * amount;
                                    if (user.CurrentInvestmentAmount == 0)
                                    {
                                        user.CurrentInvestmentAmount = -0.00001m;
                                    }
                                    user.AdjustmentFactor = (oldInvestmentAmount != 0 && oldAdjustmentFactor != 0) || user.CurrentInvestmentAmount == 0 ? user.CurrentInvestmentAmount / oldInvestmentAmount * oldAdjustmentFactor : -0.000000000005m;
                                }
                                else
                                {
                                    user.CurrentInvestmentAmount = -0.00001m;
                                    user.AdjustmentFactor = 1;
                                }
                            }
                            else
                            {
                                user.CurrentAccountAmount = 0;

                                user.CurrentInvestmentAmount = -0.00001m;
                                user.AdjustmentFactor = 1;
                            }

                            //_unitOfWork.UserRepository.Update(user);
                            //flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                            //                            await _userManager.UpdateUser(user);
                            await _unitOfWork.UserRepository.ExecuteSql($"UPDATE [dbo].[AspNetUsers] SET [CurrentAccountAmount] = {user.CurrentAccountAmount},CurrentInvestmentAmount = {user.CurrentInvestmentAmount},AdjustmentFactor = {user.AdjustmentFactor} WHERE Id='{user.Id}'");

                            var transactionHistory = new TransactionHistory()
                            {
                                Amount = (fee / 100) * amount,
                                CurrentAccountAmount = user.CurrentAccountAmount,
                                Status = TransactionStatus.Success,
                                TotalWithdrawal = 0,
                                TransactionDate = DateTime.Now,
                                TransactionType = TransactionType.FundPurchaseFee,
                                UserId = user.Id
                            };
                            //_unitOfWork.TransactionHistoryRepository.Add(transactionHistory);
                            await _unitOfWork.TransactionHistoryRepository.ExecuteSql($"INSERT INTO [dbo].[TransactionHistory]  ([UserId],[Amount],[Status],[TransactionDate],[CurrentAccountAmount],[TransactionType],[TotalWithdrawal],[RemittanceStatus],[WithdrawalType]) VALUES (N'{transactionHistory.UserId}',{ transactionHistory.Amount},{ (int)transactionHistory.Status},GETDATE(),{ transactionHistory.CurrentAccountAmount},{ (int)transactionHistory.TransactionType},{ transactionHistory.TotalWithdrawal},NULL,NULL)");
                            //flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                        }
                        else if (userCer.Value < 0)
                        {
                            var investment = (await _unitOfWork.InvestmentRepository.FindByAsync(i => i.UserId == user.Id)).OrderBy(i => i.DateInvestment).FirstOrDefault();
                            if (investment != null)
                            {
                                var day = (DateTime.Now - investment.DateInvestment).Days;
                                fee = await _fundSellFeeManager.GetFeeValue(fundId, day);
                                if (fee == 0) continue;

                                if (user.CurrentAccountAmount - (fee / 100) * amount >= 0)
                                {
                                    user.CurrentAccountAmount -= (fee / 100) * amount;

                                    var oldInvestmentAmount = user.CurrentInvestmentAmount;
                                    var oldAdjustmentFactor = user.AdjustmentFactor;
                                    if (user.CurrentAccountAmount > 0)
                                    {
                                        user.CurrentInvestmentAmount -= (fee / 100) * amount;
                                        if (user.CurrentInvestmentAmount == 0)
                                        {
                                            user.CurrentInvestmentAmount = -0.00001m;
                                        }
                                        user.AdjustmentFactor = (oldInvestmentAmount != 0 && oldAdjustmentFactor != 0) || user.CurrentInvestmentAmount == 0 ? user.CurrentInvestmentAmount / oldInvestmentAmount * oldAdjustmentFactor : -0.000000000005m;
                                    }
                                    else
                                    {
                                        user.CurrentInvestmentAmount = -0.00001m;
                                        user.AdjustmentFactor = 1;
                                    }
                                }
                                else
                                {
                                    user.CurrentAccountAmount = 0;
                                    user.CurrentInvestmentAmount = -0.00001m;
                                    user.AdjustmentFactor = 1;
                                }

                                //_unitOfWork.UserRepository.Update(user);
                                //flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                //await _userManager.UpdateUser(user);
                                await _unitOfWork.UserRepository.ExecuteSql($"UPDATE [dbo].[AspNetUsers] SET [CurrentAccountAmount] = {user.CurrentAccountAmount},CurrentInvestmentAmount = {user.CurrentInvestmentAmount},AdjustmentFactor = {user.AdjustmentFactor} WHERE Id='{user.Id}'");

                                var transactionHistory = new TransactionHistory()
                                {
                                    Amount = (fee / 100) * amount,
                                    CurrentAccountAmount = user.CurrentAccountAmount,
                                    Status = TransactionStatus.Success,
                                    TotalWithdrawal = 0,
                                    TransactionDate = DateTime.Now,
                                    TransactionType = TransactionType.FundSellFee,
                                    UserId = user.Id
                                };
                                // _unitOfWork.TransactionHistoryRepository.Add(transactionHistory);
                                await _unitOfWork.TransactionHistoryRepository.ExecuteSql($"INSERT INTO [dbo].[TransactionHistory]  ([UserId],[Amount],[Status],[TransactionDate],[CurrentAccountAmount],[TransactionType],[TotalWithdrawal],[RemittanceStatus],[WithdrawalType]) VALUES (N'{transactionHistory.UserId}',{ transactionHistory.Amount},{ (int)transactionHistory.Status},GETDATE(),{ transactionHistory.CurrentAccountAmount},{ (int)transactionHistory.TransactionType},{ transactionHistory.TotalWithdrawal},NULL,NULL)");

                                //flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                            }
                        }
                    }

                    var listUserFund = (await _unitOfWork.UserFundRepository.FindByAsync(i => i.FundId == fundId && i.EditStatus == EditStatus.Updating)).ToList();
                    var listUpdateUserFund = new List<UserFund>();
                    foreach (var userFund in listUserFund)
                    {
                        if (userFund.EditStatus == EditStatus.Updating)
                        {
                            if (listNoOfCertificatesByUser.ContainsKey(userFund.UserId) && listNoOfCertificatesByUser[userFund.UserId] == 0) continue;
                            var user = await _customerManager.GetCustomerById(userFund.UserId);

                            decimal fee = 0;
                            if (listNoOfCertificatesByUser.ContainsKey(userFund.UserId) && listNoOfCertificatesByUser[userFund.UserId] > 0)
                            {
                                fee = await _fundPurchaseFeeManager.GetFeeValue(fundId, listNoOfCertificatesByUser[userFund.UserId] * fund.NAV);

                                if (userFund.NoOfCertificates - (fee / 100) * listNoOfCertificatesByUser[userFund.UserId] >= 0)
                                {
                                    userFund.NoOfCertificates -= Decimal.Round((fee / 100) * listNoOfCertificatesByUser[userFund.UserId], 2);
                                }
                                else
                                {
                                    userFund.NoOfCertificates = 0;
                                }

                            }
                            else if (listNoOfCertificatesByUser.ContainsKey(userFund.UserId) && listNoOfCertificatesByUser[userFund.UserId] < 0)
                            {
                                var investment = (await _unitOfWork.InvestmentRepository.FindByAsync(i => i.UserId == user.Id && i.RemainAmount > 0)).OrderBy(i => i.DateInvestment).FirstOrDefault();
                                if (investment != null)
                                {
                                    var day = (DateTime.Now - investment.DateInvestment).Days;
                                    fee = await _fundSellFeeManager.GetFeeValue(fundId, day);

                                    if (userFund.NoOfCertificates - (fee / 100) * Math.Abs(listNoOfCertificatesByUser[userFund.UserId]) >= 0)
                                    {
                                        userFund.NoOfCertificates -= Decimal.Round((fee / 100) * Math.Abs(listNoOfCertificatesByUser[userFund.UserId]), 2);
                                    }
                                    else
                                    {
                                        userFund.NoOfCertificates = 0;
                                    }
                                }
                            }

                        }
                        userFund.EditStatus = EditStatus.Success;
                        await _unitOfWork.UserFundRepository.ExecuteSql($"UPDATE [dbo].[UserFunds] SET  [NoOfCertificates] = {userFund.NoOfCertificates} ,[EditStatus] = {(int)userFund.EditStatus} WHERE UserId = '{userFund.UserId}' and FundId = {userFund.FundId}");
                        //listUpdateUserFund.Add(userFund);
                    }

                    dbContextTransaction.Commit();

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }
            finally
            {
                fund.IsBalancing = false;
                _unitOfWork.FundRepository.Update(fund);
                await _unitOfWork.SaveChangesAsync();
                _logger.Info("End approve balance fund " + fundId + " in " + (DateTime.Now-startTime).TotalMinutes + " minutes");
            }
        }


        public async Task<bool> ApproveFundPercent(int portfolioId)
        {
            try
            {
                using (var dbContextTransaction = _unitOfWork.GetCurrentContext().Database.BeginTransaction())
                {
                    var flag = 1;
                    var listPortfolioFundUpdating = (await _unitOfWork.PortfolioFundRepository.FindByAsync(m => m.PortfolioId == portfolioId && m.EditStatus == EditStatus.Updating)).ToList();
                    if (listPortfolioFundUpdating == null)
                    {
                        return false;
                    }
                    var listCurrentFundId = new List<int>();
                    foreach (var portfolioFundUpdating in listPortfolioFundUpdating)
                    {
                        if (portfolioFundUpdating.FundPercentNew == null || portfolioFundUpdating.FundPercentNew <= 0)
                        {
                            _unitOfWork.PortfolioFundRepository.Delete(portfolioFundUpdating);
                            flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                        }
                        else
                        {
                            portfolioFundUpdating.FundPercent = portfolioFundUpdating.FundPercentNew;
                            portfolioFundUpdating.EditStatus = EditStatus.Success;
                            portfolioFundUpdating.LastUpdatedBy = _userManager.CurrentUser();
                            portfolioFundUpdating.DateLastUpdated = DateTime.Now;
                            _unitOfWork.PortfolioFundRepository.Update(portfolioFundUpdating);
                            flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                            listCurrentFundId.Add(portfolioFundUpdating.FundId);
                        }
                    }

                    var kvrrId = (await _unitOfWork.KVRRPortfolioRepository.GetAsync(i => i.PortfolioId == portfolioId))?.KVRRId;
                    if (kvrrId == null)
                    {
                        return true;
                    }

                    var listUserUsed = (await _customerManager.GetAllCustomer()).Where(i => i.KVRR != null && i.KVRR.Id == kvrrId);
                    if (listUserUsed == null || !listUserUsed.Any())
                    {
                        return true;
                    }

                    var listPortfolioFund = (await _unitOfWork.PortfolioFundRepository.FindByAsync(i => i.PortfolioId == portfolioId && i.FundPercent != 0)).ToList();
                    foreach (var user in listUserUsed)
                    {
                        var listCurrentUserFund = (await _unitOfWork.UserFundRepository.FindByAsync(i => i.UserId == user.Id, "User,Fund")).ToList();
                        if (listCurrentUserFund != null && listCurrentUserFund.Any())
                        {
                            var listCurrentFundIdUsed = listCurrentUserFund.Select(i => i.FundId);
                            foreach (var portfolioFund in listPortfolioFund)
                            {
                                var fund = _fundManager.GetFundById(portfolioFund.FundId);
                                var noOfCertificates = Decimal.Round((user.CurrentAccountAmount * (decimal)portfolioFund.FundPercent / 100) / fund.NAV, 2);
                                if (listCurrentFundIdUsed.Contains(portfolioFund.FundId))
                                {
                                    var userFund = listCurrentUserFund.Where(i => i.FundId == portfolioFund.FundId).FirstOrDefault();
                                    var history = new FundTransactionHistory();
                                    history.UserId = user.Id;
                                    history.FundId = fund.Id;
                                    history.TransactionDate = DateTime.Now;
                                    history.Status = EditStatus.Updating;
                                    var currentFundTransactionHistory = (await GetFundTransactionHistoryByFundId(portfolioFund.FundId, EditStatus.Updating)).OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                                    var currentNoOfCertificates = Decimal.Round((decimal)userFund.NoOfCertificates, 2);
                                    if (currentNoOfCertificates > noOfCertificates)
                                    {
                                        history.NoOfCertificates = currentNoOfCertificates - noOfCertificates;

                                        if (currentFundTransactionHistory != null)
                                        {
                                            history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates;
                                            history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates + currentNoOfCertificates - noOfCertificates;
                                        }
                                        else
                                        {
                                            history.TotalInvestNoOfCertificates = 0;
                                            history.TotalWithdrawnNoOfCertificates = currentNoOfCertificates - noOfCertificates;
                                        }

                                        history.TransactionType = TransactionType.Withdrawal;

                                        history.LastUpdatedBy = _userManager.CurrentUser();
                                        history.LastUpdatedDate = DateTime.Now;
                                        var fundTransactionHistoryAdded = _unitOfWork.FundTransactionHistoryRepository.Add(history);

                                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                        //await AddFundTransactionHistory(history);

                                        var userFundUpdate = await _unitOfWork.UserFundRepository.GetAsync(i => i.UserId == userFund.UserId && i.FundId == userFund.FundId);
                                        userFundUpdate.NoOfCertificates = noOfCertificates;
                                        userFundUpdate.EditStatus = EditStatus.Updating;
                                        _unitOfWork.UserFundRepository.Update(userFundUpdate);
                                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                    }
                                    else if (currentNoOfCertificates < noOfCertificates)
                                    {
                                        //ban
                                        history.NoOfCertificates = noOfCertificates - currentNoOfCertificates;

                                        if (currentFundTransactionHistory != null)
                                        {
                                            history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates + noOfCertificates - currentNoOfCertificates;
                                            history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates;
                                        }
                                        else
                                        {
                                            history.TotalInvestNoOfCertificates = noOfCertificates - currentNoOfCertificates;
                                            history.TotalWithdrawnNoOfCertificates = 0;
                                        }

                                        history.TransactionType = TransactionType.Investment;

                                        history.LastUpdatedBy = _userManager.CurrentUser();
                                        history.LastUpdatedDate = DateTime.Now;
                                        var fundTransactionHistoryAdded = _unitOfWork.FundTransactionHistoryRepository.Add(history);
                                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                        //await AddFundTransactionHistory(history);

                                        var userFundUpdate = await _unitOfWork.UserFundRepository.GetAsync(i => i.UserId == userFund.UserId && i.FundId == userFund.FundId);
                                        userFundUpdate.NoOfCertificates = noOfCertificates;
                                        userFundUpdate.EditStatus = EditStatus.Updating;
                                        _unitOfWork.UserFundRepository.Update(userFundUpdate);
                                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                    }

                                }
                                else
                                {
                                    //mua new
                                    var history = new FundTransactionHistory();
                                    history.UserId = user.Id;
                                    history.FundId = fund.Id;
                                    history.TransactionDate = DateTime.Now;
                                    history.Status = EditStatus.Updating;
                                    history.NoOfCertificates = noOfCertificates;
                                    var currentFundTransactionHistory = (await GetFundTransactionHistoryByFundId(portfolioFund.FundId, EditStatus.Updating)).OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                                    if (currentFundTransactionHistory != null)
                                    {
                                        history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates + noOfCertificates;
                                        history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates;
                                    }
                                    else
                                    {
                                        history.TotalInvestNoOfCertificates = noOfCertificates;
                                        history.TotalWithdrawnNoOfCertificates = 0;
                                    }

                                    history.TransactionType = TransactionType.Investment;

                                    history.LastUpdatedBy = _userManager.CurrentUser();
                                    history.LastUpdatedDate = DateTime.Now;
                                    var fundTransactionHistoryAdded = _unitOfWork.FundTransactionHistoryRepository.Add(history);
                                    flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                    //await AddFundTransactionHistory(history);

                                    var userFund = new UserFund();
                                    userFund.UserId = user.Id;
                                    userFund.FundId = fund.Id;
                                    userFund.NoOfCertificates = noOfCertificates;
                                    userFund.EditStatus = EditStatus.Updating;
                                    _unitOfWork.UserFundRepository.Add(userFund);
                                    flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                }
                            }

                            //remove fund used
                            var listPortfolioFundId = listPortfolioFund.Select(i => i.FundId);
                            listCurrentUserFund = listCurrentUserFund.Where(i => !listPortfolioFundId.Contains(i.FundId)).ToList();

                            //ban con lai
                            if (listCurrentUserFund != null && listCurrentUserFund.Any())
                            {
                                var listDeleteUserFund = new List<UserFund>();
                                foreach (var oldUserFund in listCurrentUserFund)
                                {
                                    var history = new FundTransactionHistory();
                                    history.UserId = user.Id;
                                    history.FundId = oldUserFund.FundId;
                                    history.TransactionDate = DateTime.Now;
                                    history.Status = EditStatus.Updating;
                                    history.NoOfCertificates = (decimal)oldUserFund.NoOfCertificates;
                                    var currentFundTransactionHistory = (await GetFundTransactionHistoryByFundId(oldUserFund.FundId, EditStatus.Updating)).OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                                    if (currentFundTransactionHistory != null)
                                    {
                                        history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates;
                                        history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates + (decimal)oldUserFund.NoOfCertificates;
                                    }
                                    else
                                    {
                                        history.TotalInvestNoOfCertificates = 0;
                                        history.TotalWithdrawnNoOfCertificates = (decimal)oldUserFund.NoOfCertificates;
                                    }

                                    history.TransactionType = TransactionType.Withdrawal;

                                    history.LastUpdatedBy = _userManager.CurrentUser();
                                    history.LastUpdatedDate = DateTime.Now;
                                    var fundTransactionHistoryAdded = _unitOfWork.FundTransactionHistoryRepository.Add(history);
                                    flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                                    //await AddFundTransactionHistory(history);

                                    var userFundDelete = new UserFund()
                                    {
                                        EditStatus = oldUserFund.EditStatus,
                                        FundId = oldUserFund.FundId,
                                        NoOfCertificates = oldUserFund.NoOfCertificates,
                                        UserId = oldUserFund.UserId
                                    };
                                    listDeleteUserFund.Add(userFundDelete);
                                }

                                _unitOfWork.UserFundRepository.BulkDelete(listDeleteUserFund);
                                flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;

                            }
                        }
                    }

                    if (flag != 0)
                    {
                        dbContextTransaction.Commit();
                    }
                    else
                    {
                        dbContextTransaction.Rollback();
                        throw new ApplicationException("ApproveBalanceFund error not update database");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task UpdateAccountAmountByFee()
        {
            var allCustomer = (await _customerManager.GetAllCustomer()).Where(i => i.CurrentAccountAmount > 0);

            foreach (var customer in allCustomer)
            {
                var perFee = _maintainingFeeManager.GetPercentageByValue(customer.CurrentAccountAmount);
                var fee = perFee / 100;
                if (fee > 0 && customer.CurrentAccountAmount > 0)
                {
                    await Withdrawal(customer.UserName, 0, fee * customer.CurrentAccountAmount, null);
                }
            }
        }

        public async Task WithdrawRollback(decimal amount, string customerUserName)
        {
            try
            {
                var flag = 1;
                var userName = "0" + customerUserName.Remove(0, 2);
                var currentUser =  await _userManager.GetUserByName(userName);
                var portfolioId = (await _unitOfWork.KVRRPortfolioRepository.GetAsync(i => i.KVRRId == currentUser.KVRR.Id))?.PortfolioId; // get %

                if (portfolioId != null)
                {
                    var listPortfolioFund = await _unitOfWork.PortfolioFundRepository.FindByAsync(i => i.PortfolioId == portfolioId && i.FundPercent != null && i.FundPercent != 0, "Fund");

                    _logger.Info($"{currentUser.UserName} WithdrawRollback {amount}: START");

                    using (var dbContextTransaction = _unitOfWork.GetCurrentContext().Database.BeginTransaction())
                    {
                        foreach (var item in listPortfolioFund)
                        {
                            var history = new FundTransactionHistory();
                            var fund = item.Fund;
                            history.UserId = currentUser.Id;
                            history.FundId = fund.Id;
                            var noOfCertificates = (amount * (decimal)item.FundPercent / 100) / fund.NAV;
                            history.NoOfCertificates = noOfCertificates;

                            var currentFundTransactionHistory = (await GetFundTransactionHistoryByFundId(item.FundId, EditStatus.Updating)).OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                            if (currentFundTransactionHistory != null)
                            {
                                history.TotalInvestNoOfCertificates = currentFundTransactionHistory.TotalInvestNoOfCertificates + noOfCertificates;
                                history.TotalWithdrawnNoOfCertificates = currentFundTransactionHistory.TotalWithdrawnNoOfCertificates;
                            }
                            else
                            {
                                history.TotalInvestNoOfCertificates = noOfCertificates;
                                history.TotalWithdrawnNoOfCertificates = 0;
                            }

                            history.TransactionType = TransactionType.Investment;
                            history.TransactionDate = DateTime.Now;
                            history.Status = EditStatus.Updating;

                            history.LastUpdatedBy = "Withdraw Rollback Job";
                            history.LastUpdatedDate = DateTime.Now;
                            _unitOfWork.FundTransactionHistoryRepository.Add(history);

                            flag = await _unitOfWork.SaveChangesAsync();

                            var currentUserFund = await _unitOfWork.UserFundRepository.GetAsync(i => i.UserId == currentUser.Id && i.FundId == item.FundId);
                            if (currentUserFund != null)
                            {
                                currentUserFund.NoOfCertificates = (decimal)currentUserFund.NoOfCertificates + noOfCertificates;
                                currentUserFund.EditStatus = EditStatus.Updating;
                                _unitOfWork.UserFundRepository.Update(currentUserFund);
                            }
                            else
                            {
                                var userFund = new UserFund();
                                userFund.UserId = currentUser.Id;
                                userFund.FundId = fund.Id;
                                userFund.NoOfCertificates = noOfCertificates;
                                userFund.EditStatus = EditStatus.Updating;
                                _unitOfWork.UserFundRepository.Add(userFund);
                            }
                            flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;
                        }

                        var oldCurrentAccountAmount = currentUser.CurrentAccountAmount;
                        currentUser.CurrentAccountAmount += amount;
                        currentUser.InitialInvestmentAmount += amount;

                        var oldInvestmentAmount = currentUser.CurrentInvestmentAmount;
                        var oldAdjustmentFactor = currentUser.AdjustmentFactor;
                        currentUser.CurrentInvestmentAmount += amount;

                        if (oldCurrentAccountAmount > 0)
                        {
                            currentUser.AdjustmentFactor = (oldInvestmentAmount != 0 && oldAdjustmentFactor != 0) || currentUser.CurrentInvestmentAmount == 0 ? currentUser.CurrentInvestmentAmount / oldInvestmentAmount * oldAdjustmentFactor : -0.000000000005m;
                        }
                        else
                        {
                            currentUser.AdjustmentFactor = 1;
                        }

                        _unitOfWork.UserRepository.Update(currentUser);
                        flag = flag != 0 ? await _unitOfWork.SaveChangesAsync() : 0;


                        if (flag != 0)
                        {
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            throw new ApplicationException("Withdraw Rollback Job : user " + currentUser.UserName + " (amount = " + amount + ") error not update database");
                        }

                        _logger.Info($"{currentUser.UserName} Withdraw Rollback Job {amount}: DONE");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"{customerUserName} invests {amount}: " + ex.Message);
                throw ex;
            }
        }
    }
}
