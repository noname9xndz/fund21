using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Business.Common
{
    public interface IMaintainingFeeManager
    {
        List<MaintainingFee> GetConfiguration();
        List<MaintainingFee> GetConfigurationByIds(int[] Ids);
        void DeleteConfigurationByIds(int[] Ids);
        Task UpdateFees(List<MaintainingFee> MaintainingFees);
        Task<MaintainingFee> UpdateConfiguration(MaintainingFee MaintainingFee);
        Task<MaintainingFee> AddConfiguration(MaintainingFee model);
        Task AddListFees(List<MaintainingFee> list);
        decimal GetPercentageByValue(decimal amount);
        Task<bool> ValidAmountMoney(MaintainingFee currentFee);
    }
    public class MaintainingFeeManager : IMaintainingFeeManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public MaintainingFeeManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public decimal GetPercentageByValue(decimal amount)
        {
            List<MaintainingFee> listValue = GetConfiguration();
            foreach (var value in listValue)
            {
                if (value.AmountFrom <= amount && value.AmountTo >= amount)
                {
                    return value.Percentage;
                }
            }
            return 0;
        }

        public async Task<MaintainingFee> AddConfiguration(MaintainingFee model)
        {
            try
            {
                var savedModel = _unitOfWork.MaintainingFeeRepository.Add(model);
                await _unitOfWork.SaveChangesAsync();
                return savedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddListFees(List<MaintainingFee> list)
        {
            try
            {
                foreach(var currentFee in list)
                {
                    _unitOfWork.MaintainingFeeRepository.Add(currentFee);
                }
               //_unitOfWork.MaintainingFeeRepository.BulkInsert(list);
               await _unitOfWork.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteConfigurationByIds(int[] Ids)
        {
            if(Ids == null || !Ids.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                List<MaintainingFee> list = GetConfigurationByIds(Ids);
                _unitOfWork.MaintainingFeeRepository.BulkDelete(list);
                _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<MaintainingFee> GetConfiguration()
        {
            return _unitOfWork.MaintainingFeeRepository.GetMaintainingFeeDefault();
        }

        public List<MaintainingFee> GetConfigurationByIds(int[] Ids)
        {
            if (Ids == null || !Ids.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                return _unitOfWork.MaintainingFeeRepository.GetMaintainingFeeByIds(Ids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MaintainingFee> UpdateConfiguration(MaintainingFee maintainingFee)
        {
            try
            {
                if (maintainingFee == null) throw new InvalidParameterException();
                _unitOfWork.MaintainingFeeRepository.Update(maintainingFee);
                await _unitOfWork.SaveChangesAsync();
                return maintainingFee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateFees(List<MaintainingFee> fees)
        {
            if (fees == null || !fees.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                var oldList = _unitOfWork.MaintainingFeeRepository.GetMaintainingFeeByIds(fees.Select(s => s.Id).ToArray()).ToList();
                foreach( var currentFee in fees)
                {
                    foreach(var data in oldList)
                    {
                        if (data.Id == currentFee.Id)
                        {
                            data.AmountFrom = currentFee.AmountFrom;
                            data.AmountTo = currentFee.AmountTo;
                            data.Percentage = currentFee.Percentage;
                        }
                    }
                }
                _unitOfWork.MaintainingFeeRepository.BulkUpdate(oldList);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ValidAmountMoney(MaintainingFee currentFee)
        {
            var rangeMoney = await _unitOfWork.MaintainingFeeRepository.GetAllAsync();
            if (rangeMoney == null || !rangeMoney.Any()) return false;

            if (currentFee.EntityState == FormState.Edit)
                rangeMoney = rangeMoney.Where(x => x.Id != currentFee.Id).ToList();

            var existedMarks = rangeMoney.Where(x => (currentFee.AmountFrom >= x.AmountFrom && currentFee.AmountTo <= x.AmountTo)
                             || (currentFee.AmountFrom >= x.AmountFrom && currentFee.AmountFrom <= x.AmountTo && currentFee.AmountTo > x.AmountTo)
                             || (currentFee.AmountFrom < x.AmountFrom && currentFee.AmountTo >= x.AmountFrom && currentFee.AmountTo <= x.AmountTo)
                             || (currentFee.AmountFrom < x.AmountFrom && currentFee.AmountTo > x.AmountTo)).ToList();
            if (existedMarks != null && existedMarks.Any())
                return true;

            return false;
        }
    }
}
