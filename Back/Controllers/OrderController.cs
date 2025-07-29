using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Common;
using VideoProjector.DTOs.Order;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Controllers
{
    [Route(template: "api/order")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        [HttpGet(template: "get-order")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<int>.Failure(message: "Validation is error"));

            var result = await orderService.GetOrder(orderId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet(template: "list-order")]
        public async Task<IActionResult> GetOrders(string customerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<string>.Failure(message: "Validation is error"));

            var result = await orderService.GetOrders(customerId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost(template: "add-order")]
        public async Task<IActionResult> AddOrder([FromBody] OrderAddDto orderAdd)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<OrderAddDto>.Failure(message: "Validation is error"));

            var result = await orderService.AddOrder(orderAdd);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet(template: "orders-detail")]
        public async Task<IActionResult> GetOrdersDetail(int orderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<int>.Failure(message: "Validation is error"));

            var result = await orderService.GetOrdersDetail(orderId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut(template: "update-order")]
        public async Task<IActionResult> UpdateOrder([FromBody] CustomerOrderUpdateDto orderUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<CustomerOrderUpdateDto>.Failure(message: "Validation is error"));
                    
            var result = await orderService.UpdateOrderByCustomer(orderUpdate);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost(template: "delete-order")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<int>.Failure(message: "Validation is error"));

            var result = await orderService.DeleteOrder(orderId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
