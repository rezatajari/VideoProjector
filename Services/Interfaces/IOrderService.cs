using VideoProjector.Common;
using VideoProjector.DTOs.Order;
using VideoProjector.DTOs.OrderDetail;
using VideoProjector.Models;

namespace VideoProjector.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseCenter<OrderGetDto>> GetOrder(int orderId);
        Task<ResponseCenter<List<OrderListDto>>> GetOrders(string customerId);
        Task<ResponseCenter<List<OrderDetailDto>>> GetOrdersDetail(int orderId);
        Task<ResponseCenter<bool>> UpdateOrderByCustomer(CustomerOrderUpdateDto orderUpdate);
        Task<ResponseCenter<bool>> AddOrder(OrderAddDto orderAdd);
        Task<ResponseCenter<bool>> DeleteOrder(int orderId);
    }
}

