using VideoProjector.Common;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.DTOs.ShoppingCart;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class ShoppingCartService(IShoppingCartRepository repo, ILogger<ShoppingCartService> logger) : IShoppingCartService
    {
        public async Task<ResponseCenter<bool>> AddCart(ShoppingCartDto cartDto)
        {

            // Check duplicate cart
            var cart = await repo.GetShoppingCart(cartDto.CustomerId);
            if (cart != null)
                return ResponseCenter.CreateSuccessResponse(true);

            // Generate cart
            cart = new ShoppingCart
            {
                CustomerId = cartDto.CustomerId,
                CreatedDate = DateTime.UtcNow,
                Items = GenerateCartItems(cartDto.Items),
            };

            var result = await repo.AddShoppingCart(cart);
            if (result)
                return ResponseCenter.CreateSuccessResponse(data: true);
            logger.LogWarning("Don't add cart for this customerId: {customerId}", cartDto.CustomerId);
            return ResponseCenter.CreateErrorResponse<bool>(
                message: "Don't add into database",
                errorCode: "Failed_ERROR");

        }

        private static List<ShoppingCartItem> GenerateCartItems(List<ShoppingCartItemDto> itemsDto)
        {
            return itemsDto.Select(it => new ShoppingCartItem
            {
                Quantity = it.Quantity,
                Price = it.Price,
                ProductId = it.ProductId,
            }).ToList();
        }


        public async Task<ResponseCenter<bool>> AddItemToCart(ShoppingCartItemDto itemDto)
        {
            var item = new ShoppingCartItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                Price = itemDto.Price,
            };
            var result = await repo.AddShoppingCartItem(item);
            if (!result)
                return ResponseCenter.CreateErrorResponse<bool>(
                    message: "Failed to add item to cart",
                    errorCode: "FAILED_ERROR");
            return ResponseCenter.CreateSuccessResponse(data: true);
        }

        public async Task<ResponseCenter<List<ShoppingCartItemDto>>> GetShoppingCartItems(int shoppingCartId)
        {
            var cartItems = await repo.GetShoppingCartItems(shoppingCartId);
            var cartItemsDto = cartItems.Select(it => new ShoppingCartItemDto
            {
                ProductName = it.Product.Name,
                Quantity = it.Quantity,
                Price = it.Price,
            }).ToList();

            return ResponseCenter.CreateSuccessResponse(data: cartItemsDto);
        }

        public async Task<ResponseCenter<bool>> UpdateCartItem(List<ShoppingCartItemDto> itemDto, int shoppingCartId)
        {
            // Get items from database
            var cartItems = await repo.GetShoppingCartItems(shoppingCartId);

            foreach (var item in cartItems)
            {
                foreach (var itDto in itemDto)
                {
                    item.ProductId = itDto.ProductId;
                    item.Quantity = itDto.Quantity;
                    item.Price = itDto.Price;
                }
            }

            // Update items
            var result = await repo.UpdateShoppingCartItem(cartItems);
            if (!result)
                return ResponseCenter.CreateErrorResponse<bool>(
                    message: "Failed to update shopping cart items",
                    errorCode: "FAILED_ERROR");
            return ResponseCenter.CreateSuccessResponse(data: true);
        }

        public async Task<ResponseCenter<bool>> RemoveCartItem(int shoppingCartItemId)
        {
            var result = await repo.DeleteShoppingCartItem(shoppingCartItemId);
            if (!result)
                return ResponseCenter.CreateErrorResponse<bool>(
                    message: "Failed to delete shopping cart item",
                    errorCode: "FAILED_ERROR");
            return ResponseCenter.CreateSuccessResponse(data: true);
        }

        public async Task<ResponseCenter<bool>> ValidateCart(int shoppingCartId)
        {
            var result = await repo.IsCartProcessable(shoppingCartId);
            if (!result)
                return ResponseCenter.CreateErrorResponse<bool>(
                    message: "Cart is not processable",
                    errorCode: "FAILED_ERROR");
            return ResponseCenter.CreateSuccessResponse(data: true);
        }

        public ResponseCenter<decimal> GetTotalPriceItems(List<ShoppingCartItemDto> cartItemsDto)
        {
            var totalPrice = cartItemsDto.Sum(item => item.TotalPrice);
            return ResponseCenter.CreateSuccessResponse(data: totalPrice);
        }
    }
}
