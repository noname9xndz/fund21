using AutoMapper;
using smartFunds.Business;
using smartFunds.Business.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using smartFunds.Common;

namespace smartFunds.Service.Services
{
    public interface IOrderRequestService
    {
        Task<OrderRequestModel> GetOrder(int? orderId);
        Task<OrderRequestModel> SaveOrder(OrderRequestModel order);
        Task UpdateOrder(OrderRequestModel order);
        Task<List<OrderRequestModel>> GetAllOrderRequestByStatus(OrderRequestStatus status);
        Task<OrderRequestStatus> GetStatsByOrderRequestId(int? orderId);
    }
    public class OrderRequestService : IOrderRequestService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRequestManager _orderManager;
        public OrderRequestService(IMapper mapper, IOrderRequestManager orderManager)
        {
            _mapper = mapper;
            _orderManager = orderManager;
        }

        public async Task<List<OrderRequestModel>> GetAllOrderRequestByStatus(OrderRequestStatus status)
        {
            List<OrderRequestModel> orderRequest = new List<OrderRequestModel>();
            try
            {
                var listOrder = await _orderManager.GetAllOrderRequestByStatus(status);
                if (listOrder != null)
                {
                    orderRequest =  _mapper.Map<List<OrderRequest>, List<OrderRequestModel>>(listOrder);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return orderRequest;
        }

        public async Task<OrderRequestStatus> GetStatsByOrderRequestId(int? orderId)
        {
            try
            {
                var order = await _orderManager.GetOrder(orderId);
                return order.Status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OrderRequestModel> GetOrder(int? orderId)
        {
            try
            {
                var order = await _orderManager.GetOrder(orderId);
                return _mapper.Map<OrderRequest, OrderRequestModel>(order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OrderRequestModel> SaveOrder(OrderRequestModel order)
        {
            OrderRequest newOrder = _mapper.Map<OrderRequest>(order);
            OrderRequest savedOrder = await _orderManager.SaveOrder(newOrder);
            OrderRequestModel savedOrderModel = _mapper.Map<OrderRequestModel>(savedOrder);
            return savedOrderModel;
        }

        public async Task UpdateOrder(OrderRequestModel order)
        {
            OrderRequest newOrder = _mapper.Map<OrderRequest>(order);
            await _orderManager.UpdateOrder(newOrder);
        }

    }
}
