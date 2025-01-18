using VideoProjector.Common;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.DTOs.Order;
using VideoProjector.DTOs.OrderDetail;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class OrderService(IOrderRepository repo, Logger<OrderService> logger) : IOrderService
    {
        public async Task<ResponseCenter<List<OrderListDto>>> GetOrders(string customerId)
        {
            var orders = await repo.ListOrder(customerId);
            if (orders.Count == 0)
                return ResponseCenter.CreateErrorResponse<List<OrderListDto>>(
                    message: "Order is null",
                    errorCode: "NULL");

            var ordersDto = orders.Select(o => new OrderListDto
            {
                OrderId = o.OrderId,
                OrderStatus = o.OrderStatus,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount
            }).ToList();

            return ResponseCenter.CreateSuccessResponse(ordersDto);
        }

        public async Task<ResponseCenter<List<OrderDetailDto>>> GetOrdersDetail(int orderId)
        {
            var orderDetails = await repo.GetOrderDetails(orderId);
            if (orderDetails.Count == 0)
                return ResponseCenter.CreateErrorResponse<List<OrderDetailDto>>(
                    message: "Order detail is null",
                    errorCode: "NULL");

            var orderDetailDto = orderDetails.Select(order => new OrderDetailDto
            {
                OrderId = order.OrderId,
                ProductName = order.Product.Name,
                Price = order.Price,
                Quantity = order.Quantity,
                OrderDetailId = order.OrderDetailId,
                TotalAmount = order.Order.TotalAmount
            }).ToList();

            return ResponseCenter.CreateSuccessResponse(data: orderDetailDto);
        }

        public async Task<ResponseCenter<OrderGetDto>> GetOrder(int orderId)
        {
            var order = await repo.GetOrder(orderId);
            if (order == null)
                return ResponseCenter.CreateErrorResponse<OrderGetDto>(
                    message: "Order is null",
                    errorCode: "NULL");

            var orderDto = new OrderGetDto
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                ShippingDate = order.ShippingDate,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderId = od.OrderId,
                    OrderDetailId = od.OrderDetailId,
                    Price = od.Price,
                    ProductName = od.Product.Name,
                    TotalAmount = order.TotalAmount,
                    Quantity = od.Quantity
                }).ToList()
            };

            return ResponseCenter.CreateSuccessResponse(data: orderDto);
        }

        public async Task<ResponseCenter<bool>> UpdateOrderByCustomer(CustomerOrderUpdateDto orderUpdate)
        {
            var order = await repo.GetOrder(orderUpdate.OrderId);
            if (order == null)
                return ResponseCenter.CreateErrorResponse<bool>(
                    message: "Order is null",
                    errorCode: "NULL");

            if (order.ShippingAddress == orderUpdate.ShippingAddress)
                return ResponseCenter.CreateErrorResponse<bool>(
                    message: "You order is same before",
                    errorCode: "SAME");

            order.ShippingAddress = orderUpdate.ShippingAddress;
            await repo.UpdateOrder(order);

            logger.LogInformation("Order is updated and order ID is: {OrderId}", order.OrderId);
            return ResponseCenter.CreateSuccessResponse(data: true);
        }

        public async Task<ResponseCenter<bool>> AddOrder(OrderAddDto orderAdd)
        {
            var order = new Order
            {
                CustomerId = orderAdd.CustomerId,
                OrderStatus = orderAdd.OrderStatus,
                ShippingAddress = orderAdd.ShippingAddress,
                TotalAmount = orderAdd.TotalAmount,
                OrderDate = DateTime.UtcNow,
                ShippingDate = DateTime.Today.AddDays(10),
                OrderDetails = orderAdd.OrderDetails.Select(od => new OrderDetail
                {
                    Quantity = od.Quantity,
                    ProductId = od.ProductId,
                    Price = od.Price,
                }).ToList()
            };

            await repo.AddOrder(order);
            logger.LogInformation("Order is created for this customer ID: {CustomerId}", orderAdd.CustomerId);
            return ResponseCenter.CreateSuccessResponse(data: true);
        }

        public async Task<ResponseCenter<bool>> DeleteOrder(int orderId)
        {
            var order = await repo.GetOrder(orderId);
            if (order == null)
                return ResponseCenter.CreateErrorResponse<bool>(
                    message: "Order is null",
                    errorCode: "NULL");

            if (order.ShippingDate.HasValue && order.ShippingDate.Value < DateTime.Now.AddDays(-5))
                await repo.DeleteOrder(order);

            logger.LogInformation("Order is deleted and order ID is: {OrderId}", orderId);
            return ResponseCenter.CreateSuccessResponse(data: true);
        }
    }
}
