using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Common;
using VideoProjector.DTOs.ShoppingCart;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Controllers
{
    [Route(template: "api/shoppingcart")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController(IShoppingCartService shoppingCartService) : ControllerBase
    {

        [HttpPost(template: "add-cart")]
        public async Task<IActionResult> AddCart([FromBody] ShoppingCartDto cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<ShoppingCartDto>(
                    message: "Validation is failed",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage)
                        .ToList()));

            var result = await shoppingCartService.AddCart(cartDto);
            return result.Status == "Error" ? BadRequest(result) : Ok(result);
        }

        [HttpPost(template: "add-item")]
        public async Task<IActionResult> AddItem([FromBody] ShoppingCartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<ShoppingCartItemDto>(
                    message: "Validation is failed",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage)
                        .ToList()));


            var result = await shoppingCartService.AddItemToCart(cartItemDto);
            return result.Status == "Error" ? BadRequest(result) : Ok(result);
        }

        [HttpGet(template: "get-items/{shoppingCartId}")]
        public async Task<IActionResult> GetItems(int shoppingCartId)
        {
            var result = await shoppingCartService.GetShoppingCartItems(shoppingCartId);
            return result.Status == "Error" ? BadRequest(result) : Ok(result);
        }

        [HttpPut(template: "update-items/{shoppingCartId}")]
        public async Task<IActionResult> UpdateItems([FromBody] List<ShoppingCartItemDto> cartItemsDto,
            int shoppingCartId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<List<ShoppingCartItemDto>>(
                    message: "Validation is failed",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage)
                        .ToList()));

            var result = await shoppingCartService.UpdateCartItem(cartItemsDto, shoppingCartId);
            return result.Status == "Error" ? BadRequest(result) : Ok(result);
        }

        [HttpDelete(template: "remove-item/{shoppingCartItemId}")]
        public async Task<IActionResult> RemoveItem(int shoppingCartItemId)
        {
            var result = await shoppingCartService.RemoveCartItem(shoppingCartItemId);
            return result.Status == "Error" ? BadRequest(result) : Ok(result);
        }

        [HttpGet(template: "validate-cart/{shoppingCartId}")]
        public async Task<IActionResult> ValidateCart(int shoppingCartId)
        {
            var result = await shoppingCartService.ValidateCart(shoppingCartId);
            return result.Status == "Error" ? BadRequest(result) : Ok(result);
        }

        [HttpGet(template: "get-total-price")]
        public Task<IActionResult> GetTotalPrice([FromBody] List<ShoppingCartItemDto> cartItemsDto)
        {
            var result = shoppingCartService.GetTotalPriceItems(cartItemsDto);
            return result.Status == "Error" ? Task.FromResult<IActionResult>(BadRequest(result)) : Task.FromResult<IActionResult>(Ok(result));
        }
    }
}
