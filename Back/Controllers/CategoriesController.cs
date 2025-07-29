using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Controllers
{
    [Route(template: "api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet(template:"list")]
        public async Task<IActionResult> Categories()
        {
            var result = await categoryService.Categories();

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }
}
