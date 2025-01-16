using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class OrderService(IOrderRepository repo, Logger<OrderService> logger) : IOrderService
    {
    }
}
