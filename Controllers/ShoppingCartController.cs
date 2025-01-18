using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Controllers
{
    [Route(template:"api/shoppingcart")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController(IShoppingCartService shoppingCartService) : ControllerBase
    {
    }
}
