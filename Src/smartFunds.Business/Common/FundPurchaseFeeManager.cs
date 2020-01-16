using LinqKit;
using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Business.Common
{
    public interface IFundPurchaseFeeManager
    {
        Task<FundPurchaseFee> GetFundPurchaseFee(int? fundPurchaseFeeId);
        Task<FundPurchaseFee> SaveFundPurchaseFee(FundPurchaseFee fundPurchaseFee);
        Task UpdateFundPurchaseFee(FundPurchaseFee fundPurchaseFee);
        Task<List<FundPurchaseFee>> GetListFundPurchaseFee(int fundId);
        Task DeleteListFundPurchaseFee(List<int> fundPurchaseFeeIds);
        Task<decimal> GetFeeValue(int fundId, decimal amount);
    }
    public class FundPurchaseFeeManager : IFundPurchaseFeeManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public FundPurchaseFeeManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FundPurchaseFee> GetFundPurchaseFee(int? fundPurchaseFeeId)
        {
            if (fundPurchaseFeeId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var fundPurchaseFee = await _unitOfWork.FundPurchaseFeeRepository.GetAsync(m => m.Id == fundPurchaseFeeId);
                if (fundPurchaseFee != null)
                {
                    return fundPurchaseFee;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FundPurchaseFee> SaveFundPurchaseFee(FundPurchaseFee fundPurchaseFee)
        {
            try
            {
                var savedFundPurchaseFee = _unitOfWork.FundPurchaseFeeRepository.Add(fundPurchaseFee);
                await _unitOfWork.SaveChangesAsync();
                return savedFundPurchaseFee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateFundPurchaseFee(FundPurchaseFee fundPurchaseFee)
        {
            try
            {
                if (fundPurchaseFee == null) throw new InvalidParameterException();

                _unitOfWork.FundPurchaseFeeRepository.Update(fundPurchaseFee);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FundPurchaseFee>> GetListFundPurchaseFee(int fundId)
        {
            try
            {
                var list = (await _unitOfWork.FundPurchaseFeeRepository.FindByAsync(i => i.FundId == fundId)).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task DeleteListFundPurchaseFee(List<int> fundPurchaseFeeIds)
        {
            try
            {
                if (fundPurchaseFeeIds == null || !fundPurchaseFeeIds.Any()) throw new InvalidParameterException();
                var list = await _unitOfWork.FundPurchaseFeeRepository.FindByAsync(i => fundPurchaseFeeIds.Contains(i.Id));
                if(list != null && list.Any())
                {
                    _unitOfWork.FundPurchaseFeeRepository.BulkDelete(list.ToList());
                    await _unitOfWork.SaveChangesAsync();
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<decimal> GetFeeValue(int fundId, decimal amount)
        {
            try
            {
                var predicate = PredicateBuilder.New<FundPurchaseFee>(true);

                predicate = predicate.And(u => u.From == 0 || (u.From !=0 && amount >= GetFromValue(u)));
                predicate = predicate.And(u => u.To == -1 || (u.To != -1 && amount <= GetToValue(u)));

                var fundPurchaseFee = (await _unitOfWork.FundPurchaseFeeRepository.FindByAsync(m => m.FundId == fundId)).Where(predicate)?.FirstOrDefault();
                if(fundPurchaseFee != null)
                {
                    return fundPurchaseFee.Fee;
                }

                return 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private decimal GetFromValue(FundPurchaseFee fundPurchaseFee)
        {
            if (fundPurchaseFee.FromLabel == FromLabel.From)
            {
                return fundPurchaseFee.From;
            }
            else
            {
                return fundPurchaseFee.From + 1;
            }
        }

        private decimal GetToValue(FundPurchaseFee fundPurchaseFee)
        {
            if (fundPurchaseFee.ToLabel == ToLabel.To)
            {
                return fundPurchaseFee.To;
            }
            else
            {
                return fundPurchaseFee.To - 1;
            }
        }
    }
}
