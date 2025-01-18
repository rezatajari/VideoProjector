using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class ShoppingCartService(IShoppingCartRepository repo, ILogger<ShoppingCartService> logger) : IShoppingCartService
    {
    }
}
