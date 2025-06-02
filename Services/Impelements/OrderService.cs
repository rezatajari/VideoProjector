using VideoProjector.Common;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.DTOs.Order;
using VideoProjector.DTOs.OrderDetail;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Implementations;


public class OrderService(IOrderRepository repo, ILogger<OrderService> logger) : IOrderService
{
    /// <summary>
    /// Retrieves a list of orders for a specific customer.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <returns>A response containing a list of order DTOs.</returns>
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

    /// <summary>
    /// Retrieves the details of a specific order.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <returns>A response containing a list of order detail DTOs.</returns>
    public async Task<ResponseCenter<List<OrderDetailDto>>> GetOrdersDetail(int orderId)
    {
        var orderDetails = await repo.GetOrderDetails(orderId);
        if (orderDetails == null || orderDetails.Count == 0)
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

    /// <summary>
    /// Retrieves a specific order by its identifier.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <returns>A response containing the order DTO.</returns>
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

    /// <summary>
    /// Updates an order's shipping address by the customer.
    /// </summary>
    /// <param name="orderUpdate">The order update DTO.</param>
    /// <returns>A response indicating whether the update was successful.</returns>
    public async Task<ResponseCenter<bool>> UpdateOrderByCustomer(CustomerOrderUpdateDto orderUpdate)
    {
        if (orderUpdate == null)
            return ResponseCenter.CreateErrorResponse<bool>(
                message: "Order update data is null",
                errorCode: "NULL");

        var order = await repo.GetOrder(orderUpdate.OrderId);
        if (order == null)
            return ResponseCenter.CreateErrorResponse<bool>(
                message: "Your order is the same as before",
                errorCode: "SAME");

        if (order.ShippingAddress == orderUpdate.ShippingAddress)
            return ResponseCenter.CreateErrorResponse<bool>(
                message: "You order is same before",
                errorCode: "SAME");

        order.ShippingAddress = orderUpdate.ShippingAddress;
        await repo.UpdateOrder(order);

        logger.LogInformation("Order is updated and order ID is: {OrderId}", order.OrderId);
        return ResponseCenter.CreateSuccessResponse(data: true);
    }

    /// <summary>
    /// Adds a new order.
    /// </summary>
    /// <param name="orderAdd">The order add DTO.</param>
    /// <returns>A response indicating whether the addition was successful.</returns>
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

    /// <summary>
    /// Deletes a specific order by its identifier.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <returns>A response indicating whether the deletion was successful.</returns>
    public async Task<ResponseCenter<bool>> DeleteOrder(int orderId)
    {
        var order = await repo.GetOrder(orderId);
        if (order == null)
            return ResponseCenter.CreateErrorResponse<bool>(
                message: "Order is null",
                errorCode: "NULL");

        if (!order.ShippingDate.HasValue || order.ShippingDate.Value <= DateTime.Now.AddDays(-5))
            return ResponseCenter.CreateErrorResponse<bool>(
                message: "Order cannot be deleted",
                errorCode: "CANNOT_DELETE");

        await repo.DeleteOrder(order);
        logger.LogInformation("Order is deleted and order ID is: {OrderId}", orderId);
        return ResponseCenter.CreateSuccessResponse(data: true);

    }
}