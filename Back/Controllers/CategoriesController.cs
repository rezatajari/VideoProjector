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

        [HttpGet(template: "Details/{categoryId}")]
        public async Task<IActionResult> Details(int categoryId)
        {
            var result = await categoryService.Category(categoryId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }
}
