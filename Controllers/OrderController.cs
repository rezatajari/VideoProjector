using System.Security.AccessControl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
                return BadRequest(ResponseCenter.CreateErrorResponse<int>(
                    message: "Validation is error",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()));

            var result = await orderService.GetOrder(orderId);
            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet(template: "list-order")]
        public async Task<IActionResult> GetOrders(string customerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<string>(
                    message: "Validation is error",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()));
            var result = await orderService.GetOrders(customerId);
            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost(template: "add-order")]
        public async Task<IActionResult> AddOrder([FromBody] OrderAddDto orderAdd)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<OrderAddDto>(
                    message: "Validation is error",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()));

            var result = await orderService.AddOrder(orderAdd);
            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet(template: "orders-detail")]
        public async Task<IActionResult> GetOrdersDetail(int orderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<int>(
                    message: "Validation is error",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()));

            var result = await orderService.GetOrdersDetail(orderId);
            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut(template: "update-order")]
        public async Task<IActionResult> UpdateOrder([FromBody] CustomerOrderUpdateDto orderUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<CustomerOrderUpdateDto>(
                    message: "Validation is error",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()));

            var result = await orderService.UpdateOrderByCustomer(orderUpdate);
            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost(template: "delete-order")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<int>(
                    message: "Validation is error",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()));

            var result = await orderService.DeleteOrder(orderId);
            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }
    }
}
