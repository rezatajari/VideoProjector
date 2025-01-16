using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Common;
using VideoProjector.DTOs.Product;
using VideoProjector.Services.Impelements;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Controllers
{
    [Route(template: "api/product")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet(template: "product-list")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await productService.GetProductList();

            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet(template: "detail")]
        public async Task<IActionResult> GetProductDetail([FromBody] GetProductDto productId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<GetProductDto>(
                    message: "Validation failed",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                        .ToList()));

            var response = await productService.GetProductDetail(productId);
            if (response.Status == "Error")
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet(template: "search")]
        public async Task<IActionResult> Search([FromBody] ProductSearchDto searchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<ProductSearchDto>(message: "Validation failed",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                        .ToList()));

            var result = await productService.GetProductSearch(searchDto);
            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }

    }
}
