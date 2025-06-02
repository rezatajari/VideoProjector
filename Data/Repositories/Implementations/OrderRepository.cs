using Microsoft.EntityFrameworkCore;
using VideoProjector.Common;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.Models;

namespace VideoProjector.Data.Repositories.Implementations
{
    public class OrderRepository(VpDatabase database) : IOrderRepository
    {
        public async Task<Order?> GetOrder(int orderId)
        {
            return await database.Orders.Include(d => d.OrderDetails)
                .Where(o => o.OrderId == orderId)
                .FirstOrDefaultAsync();
        }

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

        public async Task<List<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await database.OrderDetails
                .Include(p => p.Product)
                .Include(o => o.OrderId)
                .Where(c => c.OrderId == orderId)
                .ToListAsync();

        }
    }
}
