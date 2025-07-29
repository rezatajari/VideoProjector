using VideoProjector.Common;
using VideoProjector.Models;

namespace Back.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrder(int orderId);
        Task<List<Order>> ListOrder(string customerId);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(Order order);
        Task<List<OrderDetail>> GetOrderDetails(int orderId);
    }
}
