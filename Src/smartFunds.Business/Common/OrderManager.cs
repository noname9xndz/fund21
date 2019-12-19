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
    public interface IOrderManager
    {
        Task<Order> GetOrder(int? orderId);
        Task<Order> SaveOrder(Order order);
        Task UpdateOrder(Order order);
    }
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> GetOrder(int? orderId)
        {
            if (orderId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var order = await _unitOfWork.OrderRepository.GetAsync(m => m.Id == orderId);
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

        public async Task<Order> SaveOrder(Order order)
        {
            try
            {
                var savedOrder = _unitOfWork.OrderRepository.Add(order);
                await _unitOfWork.SaveChangesAsync();
                return savedOrder;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateOrder(Order order)
        {
            try
            {
                if (order == null) throw new InvalidParameterException();

                _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
