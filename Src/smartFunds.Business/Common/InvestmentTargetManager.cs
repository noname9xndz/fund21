using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using Hangfire;
using smartFunds.Common;

namespace smartFunds.Business.Common
{
    public interface IInvestmentTargetManager
    {
        Task<InvestmentTarget> AddInvestmentTarget(InvestmentTarget investmentTarget);
        Task<InvestmentTarget> GetInvestmentTarget(string customerId);
        Task<decimal> GetInterestRate(int kvrrId, Duration duration);
        Task UpdateInvestmentTarget(InvestmentTarget investmentTarget);
        Task DeleteInvestmentTarget(InvestmentTarget investmentTarget);
        Task AutoSendSMSRemind(string userName, SMSConfig smsConfig);
    }
    public class InvestmentTargetManager : IInvestmentTargetManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;
        private readonly ISMSGateway _smsGateway;
        private readonly IInvestmentTargetCMSManager _investmentTargetCMSManager;

        public InvestmentTargetManager(IUnitOfWork unitOfWork, IUserManager userManager, ISMSGateway smsGateway, IInvestmentTargetCMSManager investmentTargetCMSManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _smsGateway = smsGateway;
            _investmentTargetCMSManager = investmentTargetCMSManager;
        }

        public async Task<InvestmentTarget> AddInvestmentTarget(InvestmentTarget investmentTarget)
        {
            if (investmentTarget == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                investmentTarget.User = null;
                var currentUser = await _userManager.GetCurrentUser();
                investmentTarget.UserId = currentUser.Id;     
                var interestRate = await GetInterestRate(currentUser.KVRR.Id, investmentTarget.Duration);
                var rt = (double)(1 + interestRate / (int)investmentTarget.Duration);
                var d = (double)((int)investmentTarget.Duration);
                
                investmentTarget.OneTimeAmount = ((interestRate / (int)investmentTarget.Duration) * (investmentTarget.ExpectedAmount / (decimal)(Math.Pow(rt, d)) - currentUser.CurrentAccountAmount) / (decimal)(1 - (Math.Pow(rt, -d)))) / (int)investmentTarget.Frequency;
                var newInvestmentTarget = _unitOfWork.InvestmentTargetRepository.Add(investmentTarget);
                await _unitOfWork.SaveChangesAsync();
                return newInvestmentTarget;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<decimal> GetInterestRate(int kvrrId, Duration duration)
        {
            var portfolioId = (await _unitOfWork.KVRRPortfolioRepository.GetAsync(i => i.KVRRId == kvrrId))?.PortfolioId;
            if(portfolioId != null)
            {
                return (await _investmentTargetCMSManager.GetInvestmentTargetSettingValue((int)portfolioId, duration))/100;
            }

            return 0;
        }

        public async Task<InvestmentTarget> GetInvestmentTarget(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidParameterException();
            }
            try
            {
                var investmentTarget = await _unitOfWork.InvestmentTargetRepository.GetAsync(i => !i.IsDeleted && i.User.Id == customerId, "User");
                return investmentTarget;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateInvestmentTarget(InvestmentTarget investmentTarget)
        {
            if (investmentTarget == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                investmentTarget.User = null;
                _unitOfWork.InvestmentTargetRepository.Update(investmentTarget);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteInvestmentTarget(InvestmentTarget investmentTarget)
        {
            if (investmentTarget == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                investmentTarget.User = null;
                _unitOfWork.InvestmentTargetRepository.Delete(investmentTarget);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AutoSendSMSRemind(string userName, SMSConfig smsConfig)
        {
            var currentUser = await _userManager.GetUserByName(userName);
            var currentInvestmentTarget = await GetInvestmentTarget(currentUser.Id);
            if(currentInvestmentTarget == null || currentInvestmentTarget.Status==smartFunds.Common.EditStatus.Updating || currentInvestmentTarget.ExpectedAmount<=currentUser.CurrentAccountAmount || (DateTime.Now - currentInvestmentTarget.DateLastUpdated).Days / 30 >= (int)currentInvestmentTarget.Duration)
            {
                RecurringJob.RemoveIfExists("InvestmentTargetSend" + currentUser.UserName);
                return;
            }

            _smsGateway.Send(smsConfig);
        }
    }
}
