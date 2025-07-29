using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Common;
using VideoProjector.DTOs.Product;
using VideoProjector.Services.Impelements;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Controllers
{
    /// <summary>
    /// Products http handler
    /// </summary>
    /// <param name="productService"></param>
    [Route(template: "api/product")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        /// <summary>
        /// list of product
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "list")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await productService.GetProductList();

            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet(template: "detail")]
        public async Task<IActionResult> GetProductDetail([FromBody] GetProductDto productId)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<GetProductDto>.Failure(message: "Validation failed"));

            var response = await productService.GetProductDetail(productId);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet(template: "search")]
        public async Task<IActionResult> Search([FromBody] ProductSearchDto searchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(GeneralResponse<ProductSearchDto>.Failure(message: "Validation failed"));

            var result = await productService.GetProductSearch(searchDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
