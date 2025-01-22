using VideoProjector.Common;
using VideoProjector.DTOs.ShoppingCart;
using VideoProjector.Models;

namespace VideoProjector.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ResponseCenter<bool>> AddCart(ShoppingCartDto cartDto);
        Task<ResponseCenter<bool>> AddItemToCart(ShoppingCartItemDto itemDto); // Add item to cart
        Task<ResponseCenter<List<ShoppingCartItemDto>>> GetShoppingCartItems(int shoppingCartId); // Retrieve cart items
        Task<ResponseCenter<bool>> UpdateCartItem(List<ShoppingCartItemDto> itemDto, int shoppingCartId); // Update item details
        Task<ResponseCenter<bool>> RemoveCartItem(int shoppingCartItemId); // Remove an item from the cart
        Task<ResponseCenter<bool>> ValidateCart(int shoppingCartId); // Check if the cart is valid for further processing
        ResponseCenter<decimal> GetTotalPriceItems(List<ShoppingCartItemDto> cartItemsDto); // Calculate the total price of the cart

    }
}
