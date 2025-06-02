using Microsoft.EntityFrameworkCore;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.Migrations;
using VideoProjector.Models;

namespace VideoProjector.Data.Repositories.Implementations
{
    public class ShoppingCartRepository(VpDatabase database) : IShoppingCartRepository
    {
        public async Task<bool> AddShoppingCart(ShoppingCart cart)
        {
            await database.ShoppingCarts.AddAsync(cart);
            var result = await database.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> AddShoppingCartItem(ShoppingCartItem item)
        {
            await database.ShoppingCartItems.AddAsync(item);
            var result = await database.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ShoppingCart?> GetShoppingCart(string customerId)
        {
            var shoppingCart = await database.ShoppingCarts
                .Where(c => c.CustomerId == customerId)
                .FirstOrDefaultAsync();

            return shoppingCart;

        }

        public async Task<bool> DeleteShoppingCartItem(int itemId)
        {
            var getShoppingCartItem = await database.ShoppingCartItems.FindAsync(itemId);
            if (getShoppingCartItem == null)
                throw new Exception("Shopping cart item not found.");

            database.ShoppingCartItems.Remove(getShoppingCartItem);
            var result = await database.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartItems(int shoppingCartId)
        {
            var shoppingCartItems = await database.ShoppingCartItems
                .Where(x => x.ShoppingCartId == shoppingCartId)
                .ToListAsync();

            return shoppingCartItems;
        }

        public async Task<bool> IsCartProcessable(int shoppingCartId)
        {
            var shoppingCart = await database.ShoppingCarts
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .Where(s => s.ShoppingCartId == shoppingCartId)
                .FirstOrDefaultAsync();

            return shoppingCart != null && shoppingCart.Items.All(item => item.Quantity <= item.Product.StockQuantity);
        }

        public async Task<bool> UpdateShoppingCartItem(List<ShoppingCartItem> shoppingCartItems)
        {
            // Get customer cart from database
            var cart = await GetCartCustomer(shoppingCartItems);

            // Update shopping cart items
            cart.Items = shoppingCartItems;
            database.ShoppingCarts.Update(cart);
            var result = await database.SaveChangesAsync();

            return result > 0;
        }

        private async Task<ShoppingCart?> GetCartCustomer(List<ShoppingCartItem> shoppingCartItems)
        {
            var cartId = shoppingCartItems.Select(id => id.ShoppingCartId).FirstOrDefault();
            return await database.ShoppingCarts.FindAsync(cartId);

        }
    }
}
