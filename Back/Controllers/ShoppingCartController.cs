using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Common;
using VideoProjector.DTOs.ShoppingCart;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Controllers
{
    /// <summary>
    /// Controller for managing shopping cart operations.
    /// </summary>
    [Route(template: "api/shoppingcart")]
    [ApiController]
    [Authorize]
   public class ShoppingCartController(IShoppingCartService shoppingCartService) : ControllerBase
    {
        /// <summary>
        /// Adds a new shopping cart.
        /// </summary>
        /// <param name="cartDto">The shopping cart data transfer object.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost(template: "add-cart")]
        public async Task<IActionResult> AddCart([FromBody] ShoppingCartDto cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<ShoppingCartDto>.Failure(message: "Validation is failed"));

            var result = await shoppingCartService.AddCart(cartDto);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        /// <summary>
        /// Adds an item to the shopping cart.
        /// </summary>
        /// <param name="cartItemDto">The shopping cart item data transfer object.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost(template: "add-item")]
        public async Task<IActionResult> AddItem([FromBody] ShoppingCartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<ShoppingCartItemDto>.Failure(message: "Validation is failed"));


            var result = await shoppingCartService.AddItemToCart(cartItemDto);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        /// <summary>
        /// Gets the items in a shopping cart.
        /// </summary>
        /// <param name="shoppingCartId">The shopping cart identifier.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpGet(template: "get-items/{shoppingCartId}")]
        public async Task<IActionResult> GetItems(int shoppingCartId)
        {
            var result = await shoppingCartService.GetShoppingCartItems(shoppingCartId);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        /// <summary>
        /// Updates the items in a shopping cart.
        /// </summary>
        /// <param name="cartItemsDto">The list of shopping cart item data transfer objects.</param>
        /// <param name="shoppingCartId">The shopping cart identifier.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPut(template: "update-items/{shoppingCartId}")]
        public async Task<IActionResult> UpdateItems([FromBody] List<ShoppingCartItemDto> cartItemsDto, int shoppingCartId)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<ShoppingCartItemDto>.Failure(message: "Validation is failed"));

            var result = await shoppingCartService.UpdateCartItem(cartItemsDto, shoppingCartId);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <param name="shoppingCartItemId">The shopping cart item identifier.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpDelete(template: "remove-item/{shoppingCartItemId}")]
        public async Task<IActionResult> RemoveItem(int shoppingCartItemId)
        {
            var result = await shoppingCartService.RemoveCartItem(shoppingCartItemId);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        /// <summary>
        /// Validates the shopping cart.
        /// </summary>
        /// <param name="shoppingCartId">The shopping cart identifier.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpGet(template: "validate-cart/{shoppingCartId}")]
        public async Task<IActionResult> ValidateCart(int shoppingCartId)
        {
            var result = await shoppingCartService.ValidateCart(shoppingCartId);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        /// <summary>
        /// Gets the total price of items in the shopping cart.
        /// </summary>
        /// <param name="cartItemsDto">The list of shopping cart item data transfer objects.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpGet(template: "get-total-price")]
        public Task<IActionResult> GetTotalPrice([FromBody] List<ShoppingCartItemDto> cartItemsDto)
        {
            var result = shoppingCartService.GetTotalPriceItems(cartItemsDto);
            return !result.IsSuccess ? Task.FromResult<IActionResult>(BadRequest(result)) : Task.FromResult<IActionResult>(Ok(result));
        }
    }
}
