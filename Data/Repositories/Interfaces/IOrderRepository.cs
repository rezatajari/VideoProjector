using VideoProjector.Common;
using VideoProjector.Models;

namespace VideoProjector.Data.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> ListOrder(string customerId);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(Order order);
        Task<Order> GetOrder(int orderId);
    }
}
