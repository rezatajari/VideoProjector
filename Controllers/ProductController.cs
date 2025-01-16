using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Services.Impelements;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Controllers
{
    [Route(template:"api/product")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
       //TODO: implement

    }
}
