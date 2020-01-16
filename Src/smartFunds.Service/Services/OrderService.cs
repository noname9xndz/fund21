using AutoMapper;
using smartFunds.Business;
using smartFunds.Business.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IOrderService
    {
        Task<OrderModel> GetOrder(int? orderId);
        Task<OrderModel> SaveOrder(OrderModel order);
        Task UpdateOrder(OrderModel order);
        Task DeleteOrder(OrderModel order);
    }
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderManager _orderManager;
        public OrderService(IMapper mapper, IOrderManager orderManager)
        {
            _mapper = mapper;
            _orderManager = orderManager;
        }
        public async Task<OrderModel> GetOrder(int? orderId)
        {
            try
            {
                var order = await _orderManager.GetOrder(orderId);
                return _mapper.Map<Order, OrderModel>(order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OrderModel> SaveOrder(OrderModel order)
        {
            Order newOrder = _mapper.Map<Order>(order);
            Order savedOrder = await _orderManager.SaveOrder(newOrder);
            OrderModel savedOrderModel = _mapper.Map<OrderModel>(savedOrder);
            return savedOrderModel;
        }

        public async Task UpdateOrder(OrderModel order)
        {
            Order newOrder = _mapper.Map<Order>(order);
            await _orderManager.UpdateOrder(newOrder);
        }

        public async Task DeleteOrder(OrderModel order)
        {
            Order deleteOrder = _mapper.Map<Order>(order);
            await _orderManager.DeleteOrder(deleteOrder);
        }
    }
}
