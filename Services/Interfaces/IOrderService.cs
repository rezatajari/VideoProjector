using VideoProjector.Common;
using VideoProjector.DTOs.Order;
using VideoProjector.DTOs.OrderDetail;
using VideoProjector.Models;

namespace VideoProjector.Services.Interfaces
{
    public interface IOrderService
    {
        Task<GeneralResponse<OrderGetDto>> GetOrder(int orderId);
        Task<GeneralResponse<List<OrderListDto>>> GetOrders(string customerId);
        Task<GeneralResponse<List<OrderDetailDto>>> GetOrdersDetail(int orderId);
        Task<GeneralResponse<bool>> UpdateOrderByCustomer(CustomerOrderUpdateDto orderUpdate);
        Task<GeneralResponse<bool>> AddOrder(OrderAddDto orderAdd);
        Task<GeneralResponse<bool>> DeleteOrder(int orderId);
    }
}

