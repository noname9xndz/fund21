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
using smartFunds.Business.Admin;

namespace smartFunds.Business
{
    public interface IFundManager
    {
        IEnumerable<Fund> GetFunds(int pageSize, int pageIndex);
        Fund GetFundById(int id);
        IEnumerable<Fund> GetFundByIds(int[] fundIds);
        Task<ICollection<Fund>> GetFundByStatus(EditStatus status);
        IEnumerable<Fund> GetAllFund();
        Task<Fund> Save(Fund fund);
        Task<Fund> Update(Fund fund);
        Task<List<Fund>> Updates(List<Fund> funds);
        Task<byte[]> ExportFund(SearchFund searchFund = null);
        Task<bool> UpdateApprovedFunds(bool isApproved = true);
        Task DeleteFunds(int[] fundIds);
        Task UpdateFundCertificateValues();
        bool IsExistPortfolioUsing(int[] fundIds);
        Task<bool> IsDuplicateName(string newValue, string initValue);
        Task<bool> IsDuplicateCode(string newValue, string initValue);
    }
    public class FundManager : IFundManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;
        private readonly IFundFeed _fundFeed;
        private readonly ICustomerManager _customerManager;

        public FundManager(IUnitOfWork unitOfWork, IUserManager userManager, IFundFeed fundFeed, ICustomerManager customerManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _fundFeed = fundFeed;
            _customerManager = customerManager;
        }

        public IEnumerable<Fund> GetFunds(int pageSize, int pageIndex)
        {
            if (pageSize < 0 || pageIndex < 0) throw new InvalidParameterException();
            if (pageSize == 0 && pageIndex == 0) return _unitOfWork.FundRepository.GetAllFund()?.ToList();

            return _unitOfWork.FundRepository.GetAllFund()?.Take(pageSize).Skip((pageIndex - 1) * pageSize).ToList();
        }

        public async Task<ICollection<Fund>> GetFundByStatus(EditStatus status)
        {
            try
            {
                return await _unitOfWork.FundRepository.FindByAsync(m => m.EditStatus == status && m.IsDeleted == false && m.NAVNew != 0 && m.NAVNew != m.NAV);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Fund GetFundById(int id)
        {
            return _unitOfWork.FundRepository.GetFundById(id);
        }

        public IEnumerable<Fund> GetFundByIds(int[] fundIds)
        {
            return _unitOfWork.FundRepository.GetFundsByIds(fundIds).ToList();
        }

        public IEnumerable<Fund> GetAllFund()
        {
            return _unitOfWork.FundRepository.GetAllFund();
        }

        public async Task<Fund> Save(Fund fund)
        {
            try
            {
                if (fund == null) throw new InvalidParameterException();

                fund.DateLastApproved = DateTime.Now;
                fund.NAVNew = fund.NAV;
                fund.EditStatus = EditStatus.Success;
                fund = _unitOfWork.FundRepository.Add(fund);

                await _unitOfWork.SaveChangesAsync();
                return fund;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Fund> Update(Fund fund)
        {
            try
            {
                if (fund == null) throw new InvalidParameterException();

                _unitOfWork.FundRepository.Update(fund);
                await _unitOfWork.SaveChangesAsync();

                return fund;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Fund>> Updates(List<Fund> funds)
        {
            if (funds == null || !funds.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                foreach (var fund in funds)
                {
                    if (fund.NAVNew != 0)
                    {
                        var _fundUpdate = _unitOfWork.FundRepository.GetFundById(fund.Id);
                        _fundUpdate.DateLastUpdated = _fundUpdate.DateLastUpdated;
                        _fundUpdate.LastUpdatedBy = _userManager.CurrentUser();
                        _fundUpdate.NAVNew = fund.NAVNew;
                        if (fund.NAVNew != _fundUpdate.NAV)
                            _fundUpdate.EditStatus = EditStatus.Updating;

                        _unitOfWork.FundRepository.Update(_fundUpdate);
                        await _unitOfWork.SaveChangesAsync();
                    }
                }
               
                return funds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<byte[]> ExportFund(SearchFund searchFund = null)
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.FundName,
                    Model.Resources.Common.FundCode,
                    Model.Resources.Common.NAV,
                    Model.Resources.Common.FundContent,
                    Model.Resources.Common.FundUseBy,
                    Model.Resources.Common.LastUpdatedDate
                };

                var predicate = PredicateBuilder.New<Fund>(true);

                if (searchFund != null)
                {
                    if (!string.IsNullOrWhiteSpace(searchFund.FundName))
                    {
                        predicate = predicate.And(u => !string.IsNullOrWhiteSpace(u.Title) && u.Title.Contains(searchFund.FundName));
                    }
                    if (!string.IsNullOrWhiteSpace(searchFund.FundCode))
                    {
                        predicate = predicate.And(u => !string.IsNullOrWhiteSpace(u.Code) && u.Title.Contains(searchFund.FundCode));
                    }
                    if (!string.IsNullOrWhiteSpace(searchFund.FundContent))
                    {
                        predicate = predicate.And(u => !string.IsNullOrWhiteSpace(u.Content) && u.Content.Contains(searchFund.FundContent));
                    }
                    if (!string.IsNullOrWhiteSpace(searchFund.PortfolioName))
                    {
                        predicate = predicate.And(u => u.PortfolioFunds != null
                                                       && u.PortfolioFunds.Any()
                                                       && u.PortfolioFunds.First().Portfolio != null
                                                       && !string.IsNullOrWhiteSpace(u.PortfolioFunds.First().Portfolio.Title)
                                                       && u.PortfolioFunds.First().Portfolio.Title.Contains(searchFund.PortfolioName));
                    }
                }

                var listFund = _unitOfWork.FundRepository.GetAllFund().Where(predicate).ToList();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Funds");
                    using (var cells = worksheet.Cells[1, 1, 1, 6])
                    {
                        cells.Style.Font.Bold = true;
                    }

                    for (var i = 0; i < comlumHeadrs.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    }

                    var j = 2;
                    foreach (var fund in listFund)
                    {
                        worksheet.Cells["A" + j].Value = fund.Title;
                        worksheet.Cells["B" + j].Value = fund.Code;
                        worksheet.Cells["C" + j].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells["C" + j].Value = fund.NAV;
                        worksheet.Cells["D" + j].Value = fund.Content;

                        var portfolio = string.Empty;
                        if (fund.PortfolioFunds != null && fund.PortfolioFunds.Any())
                        {
                            foreach (var portfolioFund in fund.PortfolioFunds)
                            {
                                portfolio += portfolioFund.Portfolio.Title + ", ";
                            }

                            portfolio = portfolio.Substring(0, portfolio.Length - 2);
                        }

                        worksheet.Cells["E" + j].Value = portfolio;
                        worksheet.Cells["F" + j].Value = fund.DateLastApproved.ToString("dd/MM/yyyy HH:mm:ss");
                        j++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export Fund: " + ex.Message);
            }
        }

        public async Task<bool> UpdateApprovedFunds(bool isApproved = true)
        {
            try
            {
                var listFundUpdate = _unitOfWork.FundRepository.GetAllFund().Where(f => f.EditStatus == EditStatus.Updating && f.NAVNew != 0 && f.NAVNew != f.NAV).ToList();
                var listCustomer = (await _customerManager.GetAllCustomer()).Where(x => x.CurrentAccountAmount > 0);
                foreach (var fund in listFundUpdate)
                {
                    if (isApproved)
                    {
                        fund.NAV = fund.NAVNew;
                        fund.DateLastApproved = DateTime.Now;
                    }
                    else
                    {
                        fund.NAVNew = fund.NAV;
                    }
                    fund.EditStatus = EditStatus.Success;
                }
                _unitOfWork.FundRepository.BulkUpdate(listFundUpdate);
                await _unitOfWork.SaveChangesAsync();

                if (isApproved)
                {
                    var allFund = await _unitOfWork.FundRepository.GetAllAsync();
                    foreach (var custom in listCustomer)
                    {
                        var currentAccountAmount = custom.CurrentAccountAmount;
                        decimal newAccountAmount = 0;
                        foreach (var fund in allFund)
                        {
                            var userFund = await _unitOfWork.UserFundRepository.GetAsync(x => x.UserId == custom.Id && x.FundId == fund.Id);
                            if (userFund != null)
                                newAccountAmount += (decimal)userFund.NoOfCertificates * fund.NAV;
                        }

                        var user = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == custom.UserName && !u.IsDeleted);
                        user.CurrentAccountAmount = newAccountAmount;
                        user.KVRRId = custom.KVRR.Id;
                        await _userManager.UpdateUser(user);

                        var transactionHistory = new TransactionHistory();
                        transactionHistory.UserId = custom.Id;
                        transactionHistory.CurrentAccountAmount = newAccountAmount;
                        transactionHistory.Amount = Math.Abs(newAccountAmount - currentAccountAmount);
                        transactionHistory.TransactionType = TransactionType.None;
                        transactionHistory.Status = TransactionStatus.Success;
                        transactionHistory.TransactionDate = DateTime.Now;
                        _unitOfWork.TransactionHistoryRepository.Add(transactionHistory);

                        if (newAccountAmount > currentAccountAmount)
                        {
                            var investment = new Investment();
                            investment.Amount = newAccountAmount - currentAccountAmount;
                            investment.RemainAmount = newAccountAmount - currentAccountAmount;
                            investment.UserId = custom.Id;
                            investment.DateInvestment = DateTime.Now;
                            _unitOfWork.InvestmentRepository.Add(investment);
                        }

                        await _unitOfWork.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteFunds(int[] fundIds)
        {
            if (fundIds == null || !fundIds.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                var funds = _unitOfWork.FundRepository.GetFundsByIds(fundIds);
                if (funds != null && funds.Any())
                {
                    _unitOfWork.FundRepository.BulkDelete(funds.ToList());
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateFundCertificateValues()
        {
            try
            {
                // step 1: get funds certificate values from API
                List<FundModel> fundsFromAPI = await _fundFeed.GetFunds();

                // step 2: get funds from database 
                ICollection<Fund> fundsFromDb = (ICollection<Fund>)_unitOfWork.FundRepository.GetAllFund();

                // step 3: update funds with new certificate values 
                foreach (var fundModel in fundsFromAPI)
                {
                    Fund fundFromDb = fundsFromDb.Where(m => m.Code == fundModel.Code).FirstOrDefault();
                    if (fundFromDb != null)
                    {
                        fundFromDb.NAV = fundModel.NAV;
                    }
                }
                _unitOfWork.FundRepository.BulkUpdate(fundsFromDb);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsExistPortfolioUsing(int[] fundIds)
        {
            if (fundIds == null || !fundIds.Any())
            {
                throw new InvalidParameterException();
            }
            var funds = _unitOfWork.FundRepository.GetFundsByIds(fundIds);
            var existPortfolio = funds?.Where(x => x.PortfolioFunds != null && x.PortfolioFunds.Count > 0)?.ToList() ?? null;
            if (existPortfolio != null && existPortfolio.Any())
                return true;
            return false;
        }

        public async Task<bool> IsDuplicateName(string newValue, string initValue)
        {
            if (!string.IsNullOrEmpty(newValue) && newValue.Trim().Equals(initValue))
                return true;
            var isValueExisted = await _unitOfWork.FundRepository.FindByAsync(x => x.Title.ToLower().Equals(newValue.ToLower()));
            if (isValueExisted != null && isValueExisted.Any())
                return false;

            return true;
        }

        public async Task<bool> IsDuplicateCode(string newValue, string initValue)
        {
            if (!string.IsNullOrEmpty(newValue) && newValue.Trim().Equals(initValue))
                return true;
            var isValueExisted = await _unitOfWork.FundRepository.FindByAsync(x => x.Code.ToLower().Equals(newValue.ToLower()));
            if (isValueExisted != null && isValueExisted.Any())
                return false;

            return true;
        }
    }
}
