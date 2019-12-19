using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Business.Common
{
    public interface IWithdrawFeeManager
    {
        List<WithdrawalFee> GetConfiguration();
        List<WithdrawalFee> GetConfigurationByIds(int[] Ids);
        Task UpdateFees(List<WithdrawalFee> WithdrawFees);
        Task<WithdrawalFee> UpdateConfiguration(WithdrawalFee WithdrawFee);
        Task<WithdrawalFee> AddConfiguration(WithdrawalFee model);
        Task AddListFees(List<WithdrawalFee> list);
        Task<decimal> GetQuickWithdrawalFee();
        Task<decimal> GetFeeAmount(decimal withdrawalAmount, bool noUpdateDB = false);
        Task<bool> ValidRangeMonth(WithdrawalFee currentFee);
        Task<WithdrawalFee> GetQuickWithdrawalFeeItem();
        void DeleteConfigurationByIds(int[] Ids);
    }
    public class WithdrawFeeManager : IWithdrawFeeManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;
        public WithdrawFeeManager(IUnitOfWork unitOfWork, IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<WithdrawalFee> AddConfiguration(WithdrawalFee model)
        {
            try
            {
                var savedModel = _unitOfWork.WithdrawalFeeRepository.Add(model);
                await _unitOfWork.SaveChangesAsync();
                return savedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddListFees(List<WithdrawalFee> list)
        {
            try
            {
                _unitOfWork.WithdrawalFeeRepository.BulkInsert(list);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteConfigurationByIds(int[] Ids)
        {
            if (Ids == null || !Ids.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                List<WithdrawalFee> list = GetConfigurationByIds(Ids);
                _unitOfWork.WithdrawalFeeRepository.BulkDelete(list);
                _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WithdrawalFee> GetConfiguration()
        {
            var listData = new List<WithdrawalFee>();
            var list = _unitOfWork.WithdrawalFeeRepository.GetAllAsync();
            if (list.Result.Count > 0)
                listData = _unitOfWork.WithdrawalFeeRepository.FindByAsync(i => i.TimeInvestmentEnd != -1).Result.ToList();
            else
            {
                WithdrawalFee newData = new WithdrawalFee
                {
                    TimeInvestmentBegin = -2,
                    TimeInvestmentEnd = -1,
                    Percentage = 0.0375m,
                };
                _unitOfWork.WithdrawalFeeRepository.Add(newData);
                _unitOfWork.SaveChangesAsync();

                listData = list.Result.ToList();
            }
            return listData;
        }

        public async Task<WithdrawalFee> GetQuickWithdrawalFeeItem()
        {
            var quickWithdrawalFee = await _unitOfWork.WithdrawalFeeRepository.GetAsync(i => i.TimeInvestmentBegin == -1 || i.TimeInvestmentEnd == -1);
            return quickWithdrawalFee;
        }

        public List<WithdrawalFee> GetConfigurationByIds(int[] Ids)
        {
            if (Ids == null || !Ids.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                return _unitOfWork.WithdrawalFeeRepository.FindByAsync(i => Ids.Contains(i.Id)).Result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WithdrawalFee> UpdateConfiguration(WithdrawalFee WithdrawFee)
        {
            try
            {
                if (WithdrawFee == null) throw new InvalidParameterException();
                _unitOfWork.WithdrawalFeeRepository.Update(WithdrawFee);
                await _unitOfWork.SaveChangesAsync();
                return WithdrawFee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateFees(List<WithdrawalFee> fees)
        {
            if (fees == null || !fees.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                _unitOfWork.WithdrawalFeeRepository.BulkUpdate(fees);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<decimal> GetQuickWithdrawalFee()
        {
            var quickWithdrawalFee = await _unitOfWork.WithdrawalFeeRepository.GetAsync(i => i.TimeInvestmentBegin == -1 || i.TimeInvestmentEnd == -1);
            if (quickWithdrawalFee != null)
            {
                return quickWithdrawalFee.Percentage / 100;
            }
            return 0;
        }

        public async Task<decimal> GetFeeAmount(decimal withdrawalAmount, bool noUpdateDB = false)
        {
            try
            {
                //var currentUser = await _userManager.GetCurrentUser();

                //var listFee = GetConfiguration().Where(i => i.TimeInvestmentEnd > -1).OrderByDescending(i => i.TimeInvestmentBegin).ToList();
                //var total = withdrawalAmount;
                //decimal feeAmount = 0;
                //foreach (var fee in listFee)
                //{
                //    var listInvestment = (await _unitOfWork.InvestmentRepository.FindByAsync(i => i.UserId == currentUser.Id &&
                //                            DateTimeHelper.GetMonthDifference(i.DateInvestment, DateTime.Now) >= fee.TimeInvestmentBegin &&
                //                            DateTimeHelper.GetMonthDifference(i.DateInvestment, DateTime.Now) <= fee.TimeInvestmentEnd /*&& i.RemainAmount > 0*/)
                //                        ).OrderBy(i => i.DateInvestment).ToList();
                //    var listCase = listInvestment.Where(i => i.RemainAmount > 0).ToList();

                //    if (listCase.Count > 0)
                //    {
                //        var RemainAmountMax = listCase.OrderByDescending(item => item.RemainAmount).First();

                //        if (RemainAmountMax.RemainAmount >= total)
                //        {
                //            // Tồn tại một record có giá trị >= số tiền cần rút
                //            feeAmount += total * fee.Percentage / 100;
                //            RemainAmountMax.RemainAmount = RemainAmountMax.RemainAmount - total;
                //            if (!noUpdateDB)
                //            {
                //                _unitOfWork.InvestmentRepository.Update(RemainAmountMax);
                //                await _unitOfWork.SaveChangesAsync();
                //            }

                //            return feeAmount;
                //        }
                //        else
                //        {
                //            //var totalCase = withdrawalAmount;
                //            //decimal feeAmountCase = 0;
                //            // Săp xếp listCase theo tứ tự tăng dần của time đầu tư và trừ dần
                //            foreach (var investment in listCase)
                //            {
                //                if (total >= investment.RemainAmount)
                //                {
                //                    total = total - investment.RemainAmount;
                //                    feeAmount = feeAmount + (investment.RemainAmount * fee.Percentage / 100);
                //                    investment.RemainAmount = 0;
                //                    if (!noUpdateDB)
                //                    {
                //                        _unitOfWork.InvestmentRepository.Update(investment);
                //                        await _unitOfWork.SaveChangesAsync();
                //                    }
                //                }
                //                else
                //                {
                //                    feeAmount = feeAmount + (total * fee.Percentage / 100);
                //                    investment.RemainAmount = investment.RemainAmount - total;
                //                    total = 0;
                //                    if (!noUpdateDB)
                //                    {
                //                        _unitOfWork.InvestmentRepository.Update(investment);
                //                        await _unitOfWork.SaveChangesAsync();
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                //return feeAmount;

                #region -- Chau --
                var currentUser = await _userManager.GetCurrentUser();

                var listFee = GetConfiguration().Where(i => i.TimeInvestmentEnd > -1).OrderByDescending(i => i.TimeInvestmentBegin).ToList();
                var total = withdrawalAmount;
                decimal feeAmount = 0;
                foreach (var fee in listFee)
                {
                    var listInvestment = (await _unitOfWork.InvestmentRepository.FindByAsync(i => i.UserId == currentUser.Id &&
                                            DateTimeHelper.GetMonthDifference(i.DateInvestment, DateTime.Now) >= fee.TimeInvestmentBegin &&
                                            DateTimeHelper.GetMonthDifference(i.DateInvestment, DateTime.Now) <= fee.TimeInvestmentEnd && i.RemainAmount > 0)
                                        ).OrderBy(i => i.DateInvestment).ToList();
                    foreach (var investment in listInvestment)
                    {
                        if (investment.RemainAmount >= total)
                        {
                            feeAmount += total * fee.Percentage / 100;
                            investment.RemainAmount -= total;
                            if (!noUpdateDB)
                            {
                                _unitOfWork.InvestmentRepository.Update(investment);
                                await _unitOfWork.SaveChangesAsync();
                            }

                            return feeAmount;
                        }
                        else
                        {
                            feeAmount += investment.RemainAmount * fee.Percentage / 100;
                            total -= investment.RemainAmount;
                            investment.RemainAmount = 0;
                            if (!noUpdateDB)
                            {
                                _unitOfWork.InvestmentRepository.Update(investment);
                                await _unitOfWork.SaveChangesAsync();
                            }
                        }
                    }
                }

                //if (total > 0)
                //{
                //    var firstFee = listFee?.Last();
                //    if (firstFee != null)
                //    {
                //        feeAmount += total * firstFee.Percentage / 100;
                //    }
                //}

                return feeAmount;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ValidRangeMonth(WithdrawalFee currentFee)
        {
            var rangeMonth = await _unitOfWork.WithdrawalFeeRepository.GetAllAsync();
            if (rangeMonth == null || !rangeMonth.Any()) return false;
            rangeMonth = rangeMonth.Where(x => x.Id != currentFee.Id).ToList();
            var existedRangeMonth = rangeMonth.Where(x => (currentFee.TimeInvestmentBegin >= x.TimeInvestmentBegin && currentFee.TimeInvestmentEnd <= x.TimeInvestmentEnd)
                             || (currentFee.TimeInvestmentBegin >= x.TimeInvestmentBegin && currentFee.TimeInvestmentBegin <= x.TimeInvestmentEnd && currentFee.TimeInvestmentEnd > x.TimeInvestmentEnd)
                             || (currentFee.TimeInvestmentBegin < x.TimeInvestmentBegin && currentFee.TimeInvestmentEnd >= x.TimeInvestmentBegin && currentFee.TimeInvestmentEnd <= x.TimeInvestmentEnd)
                             || (currentFee.TimeInvestmentBegin < x.TimeInvestmentBegin && currentFee.TimeInvestmentEnd > x.TimeInvestmentEnd)).ToList();

            if (existedRangeMonth.Count > 0 && existedRangeMonth.Any())
                return true;

            return false;
        }

    }
}
