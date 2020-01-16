using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using smartFunds.Common;
using smartFunds.Infrastructure.Services;

namespace smartFunds.Business.Common
{
    public interface IOrderRequestManager
    {
        Task<OrderRequest> GetOrder(int? orderId);
        Task<OrderRequest> SaveOrder(OrderRequest order);
        Task UpdateOrder(OrderRequest order);
        Task<List<OrderRequest>> GetAllOrderRequestByStatus(OrderRequestStatus status);
        Task<OrderRequestStatus> GetStatsByOrderRequestId(int? orderId);

    }
    public class OrderRequestManager : IOrderRequestManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;

        public OrderRequestManager(IUnitOfWork unitOfWork, IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
                order.CreatedDate = DateTime.Now;
                order.CreatedBy = _userManager.CurrentUser();
                order.UpdatedDate = DateTime.Now;
                order.UpdatedBy = _userManager.CurrentUser();
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
                order.UpdatedDate = DateTime.Now;
                order.UpdatedBy = _userManager.CurrentUser();
                _unitOfWork.OrderRequestRepository.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<OrderRequest>> GetAllOrderRequestByStatus(OrderRequestStatus status)
        {
            try
            {
                var order = await _unitOfWork.OrderRequestRepository.FindByAsync(x =>x.Status == status).Result.ToListAsync();
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

        public async Task<OrderRequestStatus> GetStatsByOrderRequestId(int? orderId)
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
                    return order.Status;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
