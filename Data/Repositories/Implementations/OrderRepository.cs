using Microsoft.EntityFrameworkCore;
using VideoProjector.Common;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.Models;

namespace VideoProjector.Data.Repositories.Implementations
{
    public class OrderRepository(VpDatabase database) : IOrderRepository
    {
        public async Task<List<Order>> ListOrder(string customerId)
        {
            return await database.Orders
                       .Where(c => c.CustomerId == customerId)
                       .ToListAsync();
        }

        public async Task AddOrder(Order order)
        {
            await database.Orders.AddAsync(order);
            await database.SaveChangesAsync();
        }

        public async Task UpdateOrder(Order order)
        {

            database.Orders.Update(order);
            await database.SaveChangesAsync();
        }

        public async Task DeleteOrder(Order order)
        {
            database.Orders.Remove(order);
            await database.SaveChangesAsync();
        }

        public async Task<Order> GetOrder(int orderId)
        {
            return await database.Orders
                       .FirstOrDefaultAsync(c => c.OrderId == orderId);
        }
    }
}
