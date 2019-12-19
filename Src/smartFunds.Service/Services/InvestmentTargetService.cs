using AutoMapper;
using smartFunds.Business.Common;
using smartFunds.Common;
using smartFunds.Data.Models;
using smartFunds.Infrastructure.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IInvestmentTargetService
    {
        Task<InvestmentTargetModel> AddInvestmentTarget(InvestmentTargetModel investmentTargetModel);
        Task<InvestmentTargetModel> GetInvestmentTarget(string customerId);
        Task<decimal> GetInterestRate(int kvrrId, Duration duration);
        Task UpdateInvestmentTarget(InvestmentTargetModel investmentTarget);
        Task DeleteInvestmentTarget(InvestmentTargetModel investmentTargetModel);
        Task AutoSendSMSRemind(string userName, SMSConfig smsConfig);
    }
    public class InvestmentTargetService : IInvestmentTargetService
    {
        private readonly IMapper _mapper;
        private readonly IInvestmentTargetManager _investmentTargetManager;
        private readonly IUserManager _userManager;
        public InvestmentTargetService(IMapper mapper, IInvestmentTargetManager investmentTargetManager, IUserManager userManager)
        {
            _mapper = mapper;
            _investmentTargetManager = investmentTargetManager;
            _userManager = userManager;
        }        
      
        public async Task<InvestmentTargetModel> AddInvestmentTarget(InvestmentTargetModel investmentTargetModel)
        {
            InvestmentTarget investmentTarget = _mapper.Map<InvestmentTarget>(investmentTargetModel);
            var addedInvestmentTarget = await _investmentTargetManager.AddInvestmentTarget(investmentTarget);
            return _mapper.Map<InvestmentTargetModel>(addedInvestmentTarget);
        }

        public async Task<InvestmentTargetModel> GetInvestmentTarget(string customerId)
        {
            var investmentTarget = await _investmentTargetManager.GetInvestmentTarget(customerId);
            var investmentTargetModel = _mapper.Map<InvestmentTargetModel>(investmentTarget);

            if(investmentTargetModel != null)
            {
                var currentUser = await _userManager.GetUserById(customerId);
                var investmentDuration = (DateTime.Now - investmentTargetModel.DateLastUpdated).Days / 30 >= (int)investmentTargetModel.Duration ? (int)investmentTargetModel.Duration : ((float)(DateTime.Now - investmentTargetModel.DateLastUpdated).Days) / 30;
                var investmentStatus = currentUser.CurrentAccountAmount >= investmentTargetModel.ExpectedAmount || (DateTime.Now - investmentTargetModel.DateLastUpdated).Days / 30 >= (int)investmentTargetModel.Duration ? Model.Resources.Common.Completed : Model.Resources.Common.Investing;

                investmentTargetModel.InvestmentStatus = investmentStatus;
                investmentTargetModel.InvestmentAmount = currentUser.InitialInvestmentAmount;
                investmentTargetModel.InvestmentDuration = (int)investmentDuration;
            }
            
            return investmentTargetModel;
        }

        public async Task<decimal> GetInterestRate(int kvrrId, Duration duration)
        {
            return await _investmentTargetManager.GetInterestRate(kvrrId, duration);
        }

        public async Task UpdateInvestmentTarget(InvestmentTargetModel investmentTargetModel)
        {
            InvestmentTarget investmentTarget = _mapper.Map<InvestmentTarget>(investmentTargetModel);
            await _investmentTargetManager.UpdateInvestmentTarget(investmentTarget);
        }

        public async Task DeleteInvestmentTarget(InvestmentTargetModel investmentTargetModel)
        {
            InvestmentTarget investmentTarget = _mapper.Map<InvestmentTarget>(investmentTargetModel);
            await _investmentTargetManager.DeleteInvestmentTarget(investmentTarget);
        }

        public async Task AutoSendSMSRemind(string userName, SMSConfig smsConfig)
        {
            await _investmentTargetManager.AutoSendSMSRemind(userName, smsConfig);
        }
    }
}
