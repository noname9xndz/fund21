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
    public interface IFundSellFeeManager
    {
        Task<FundSellFee> GetFundSellFee(int? fundSellFeeId);
        Task<FundSellFee> SaveFundSellFee(FundSellFee fundSellFee);
        Task UpdateFundSellFee(FundSellFee fundSellFee);
        Task<List<FundSellFee>> GetListFundSellFee(int fundId);
        Task DeleteListFundSellFee(List<int> fundSellFeeIds);
        Task<decimal> GetFeeValue(int fundId, int day);
    }
    public class FundSellFeeManager : IFundSellFeeManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public FundSellFeeManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FundSellFee> GetFundSellFee(int? fundSellFeeId)
        {
            if (fundSellFeeId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var fundSellFee = await _unitOfWork.FundSellFeeRepository.GetAsync(m => m.Id == fundSellFeeId);
                if (fundSellFee != null)
                {
                    return fundSellFee;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FundSellFee> SaveFundSellFee(FundSellFee fundSellFee)
        {
            try
            {
                var savedFundSellFee = _unitOfWork.FundSellFeeRepository.Add(fundSellFee);
                await _unitOfWork.SaveChangesAsync();
                return savedFundSellFee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateFundSellFee(FundSellFee fundSellFee)
        {
            try
            {
                if (fundSellFee == null) throw new InvalidParameterException();

                _unitOfWork.FundSellFeeRepository.Update(fundSellFee);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FundSellFee>> GetListFundSellFee(int fundId)
        {
            try
            {
                var list = (await _unitOfWork.FundSellFeeRepository.FindByAsync(i => i.FundId == fundId)).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task DeleteListFundSellFee(List<int> fundSellFeeIds)
        {
            try
            {
                if (fundSellFeeIds == null || !fundSellFeeIds.Any()) throw new InvalidParameterException();
                var list = await _unitOfWork.FundSellFeeRepository.FindByAsync(i => fundSellFeeIds.Contains(i.Id));
                if(list != null && list.Any())
                {
                    _unitOfWork.FundSellFeeRepository.BulkDelete(list.ToList());
                    await _unitOfWork.SaveChangesAsync();
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<decimal> GetFeeValue(int fundId, int day)
        {
            try
            {
                var predicate = PredicateBuilder.New<FundSellFee>(true);

                predicate = predicate.And(u => u.From == 0 || (u.From != 0 && day >= GetFromValue(u)));
                predicate = predicate.And(u => u.To == -1 || (u.To != -1 && day <= GetToValue(u)));

                var fundSellFee = (await _unitOfWork.FundSellFeeRepository.FindByAsync(m => m.FundId == fundId)).Where(predicate)?.FirstOrDefault();
                if (fundSellFee != null)
                {
                    return fundSellFee.Fee;
                }

                return 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private decimal GetFromValue(FundSellFee fundSellFee)
        {
            if (fundSellFee.FromLabel == FromLabel.From)
            {
                return fundSellFee.From;
            }
            else
            {
                return fundSellFee.From + 1;
            }
        }

        private decimal GetToValue(FundSellFee fundSellFee)
        {
            if (fundSellFee.ToLabel == ToLabel.To)
            {
                return fundSellFee.To;
            }
            else
            {
                return fundSellFee.To - 1;
            }
        }
    }
}
