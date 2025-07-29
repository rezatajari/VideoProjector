using Back.Repositories.Interfaces;
using VideoProjector.Common;
using VideoProjector.DTOs.ShoppingCart;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class ShoppingCartService(IShoppingCartRepository repo, ILogger<ShoppingCartService> logger) : IShoppingCartService
    {
        public async Task<GeneralResponse<bool>> AddCart(ShoppingCartDto cartDto)
        {

            // Check duplicate cart
            var cart = await repo.GetShoppingCart(cartDto.CustomerId);
            if (cart != null)
                return GeneralResponse<bool>.Success(data: true);

            // Generate cart
            cart = new ShoppingCart
            {
                CustomerId = cartDto.CustomerId,
                CreatedDate = DateTime.UtcNow,
                Items = GenerateCartItems(cartDto.Items),
            };

            var result = await repo.AddShoppingCart(cart);
            if (result)
                return GeneralResponse<bool>.Success(data: true);
            logger.LogWarning("Don't add cart for this customerId: {customerId}", cartDto.CustomerId);
            return GeneralResponse<bool>.Failure(message: "Don't add into database");

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


        public async Task<GeneralResponse<bool>> AddItemToCart(ShoppingCartItemDto itemDto)
        {
            var item = new ShoppingCartItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                Price = itemDto.Price,
            };
            var result = await repo.AddShoppingCartItem(item);
            if (!result)
                return GeneralResponse<bool>.Failure(message: "Failed to add item to cart");
            return GeneralResponse<bool>.Success(data: true);
        }

        public async Task<GeneralResponse<List<ShoppingCartItemDto>>> GetShoppingCartItems(int shoppingCartId)
        {
            var cartItems = await repo.GetShoppingCartItems(shoppingCartId);
            var cartItemsDto = cartItems.Select(it => new ShoppingCartItemDto
            {
                ProductName = it.Product.Name,
                Quantity = it.Quantity,
                Price = it.Price,
            }).ToList();

            return GeneralResponse<List<ShoppingCartItemDto>>.Success(data: cartItemsDto);
        }

        public async Task<GeneralResponse<bool>> UpdateCartItem(List<ShoppingCartItemDto> itemDto, int shoppingCartId)
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
                return GeneralResponse<bool>.Failure(message: "Failed to update shopping cart items");
            return GeneralResponse<bool>.Success(data: true);
        }

        public async Task<GeneralResponse<bool>> RemoveCartItem(int shoppingCartItemId)
        {
            var result = await repo.DeleteShoppingCartItem(shoppingCartItemId);
            if (!result)
                return GeneralResponse<bool>.Failure(message: "Failed to delete shopping cart item");
            return GeneralResponse<bool>.Success(data: true);
        }

        public async Task<GeneralResponse<bool>> ValidateCart(int shoppingCartId)
        {
            var result = await repo.IsCartProcessable(shoppingCartId);
            if (!result)
                return GeneralResponse<bool>.Failure(message: "Cart is not processable");
            return GeneralResponse<bool>.Success(data: true);
        }

        public GeneralResponse<decimal> GetTotalPriceItems(List<ShoppingCartItemDto> cartItemsDto)
        {
            var totalPrice = cartItemsDto.Sum(item => item.TotalPrice);
            return GeneralResponse<decimal>.Success(data: totalPrice);
        }
    }
}
