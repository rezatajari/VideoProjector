using VideoProjector.Common;
using VideoProjector.DTOs.ShoppingCart;
using VideoProjector.Models;

namespace VideoProjector.Services.Interfaces
{
    /// <summary>
    /// Interface for shopping cart service operations.
    /// </summary>
    public interface IShoppingCartService
    {
        /// <summary>
        /// Adds a new shopping cart.
        /// </summary>
        /// <param name="cartDto">The shopping cart data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ResponseCenter object with a boolean indicating success or failure.</returns>
        Task<ResponseCenter<bool>> AddCart(ShoppingCartDto cartDto);

        /// <summary>
        /// Adds an item to the shopping cart.
        /// </summary>
        /// <param name="itemDto">The shopping cart item data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ResponseCenter object with a boolean indicating success or failure.</returns>
        Task<ResponseCenter<bool>> AddItemToCart(ShoppingCartItemDto itemDto);

        /// <summary>
        /// Retrieves the items in a shopping cart.
        /// </summary>
        /// <param name="shoppingCartId">The identifier of the shopping cart.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ResponseCenter object with a list of ShoppingCartItemDto.</returns>
        Task<ResponseCenter<List<ShoppingCartItemDto>>> GetShoppingCartItems(int shoppingCartId);

        /// <summary>
        /// Updates the details of items in a shopping cart.
        /// </summary>
        /// <param name="itemDto">The list of shopping cart item data transfer objects.</param>
        /// <param name="shoppingCartId">The identifier of the shopping cart.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ResponseCenter object with a boolean indicating success or failure.</returns>
        Task<ResponseCenter<bool>> UpdateCartItem(List<ShoppingCartItemDto> itemDto, int shoppingCartId);

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <param name="shoppingCartItemId">The identifier of the shopping cart item.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ResponseCenter object with a boolean indicating success or failure.</returns>
        Task<ResponseCenter<bool>> RemoveCartItem(int shoppingCartItemId);

        /// <summary>
        /// Validates the shopping cart for further processing.
        /// </summary>
        /// <param name="shoppingCartId">The identifier of the shopping cart.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ResponseCenter object with a boolean indicating success or failure.</returns>
        Task<ResponseCenter<bool>> ValidateCart(int shoppingCartId);

        /// <summary>
        /// Calculates the total price of the items in the shopping cart.
        /// </summary>
        /// <param name="cartItemsDto">The list of shopping cart item data transfer objects.</param>
        /// <returns>A ResponseCenter object with the total price of the items.</returns>
        ResponseCenter<decimal> GetTotalPriceItems(List<ShoppingCartItemDto> cartItemsDto);
    }
}
