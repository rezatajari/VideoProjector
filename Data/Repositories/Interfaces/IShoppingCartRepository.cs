using VideoProjector.Models;

namespace VideoProjector.Data.Repositories.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<bool> AddShoppingCart(ShoppingCart cart); // Add a new cart
        Task<bool> AddShoppingCartItem(ShoppingCartItem item);
        Task<ShoppingCart?> GetShoppingCart(string customerId);
        Task<bool> DeleteShoppingCartItem(int itemId); // Accept itemId instead of the full object
        Task<List<ShoppingCartItem>> GetShoppingCartItems(int shoppingCartId); // Filter by shoppingCartId
        Task<bool> IsCartProcessable(int shoppingCartId); // Replace CheckProcess with a clear intention
        Task<bool> UpdateShoppingCartItem(List<ShoppingCartItem> shoppingCartItems);
    }
}
