using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Business.Common
{
    public interface IOrderRequestManager
    {
        Task<OrderRequest> GetOrder(int? orderId);
        Task<OrderRequest> SaveOrder(OrderRequest order);
        Task UpdateOrder(OrderRequest order);
    }
    public class OrderRequestManager : IOrderRequestManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderRequestManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderRequest> GetOrder(int? orderId)
        {
            if (orderId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var order = await _unitOfWork.OrderRequestRepository.GetAsync(m => m.Id == orderId);
                if (order != null)
                {
                    return order;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OrderRequest> SaveOrder(OrderRequest order)
        {
            try
            {
                var savedOrder = _unitOfWork.OrderRequestRepository.Add(order);
                await _unitOfWork.SaveChangesAsync();
                return savedOrder;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateOrder(OrderRequest order)
        {
            try
            {
                if (order == null) throw new InvalidParameterException();

                _unitOfWork.OrderRequestRepository.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
