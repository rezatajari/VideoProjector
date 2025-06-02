using VideoProjector.Common;
using VideoProjector.Data.Repositories.Implementations;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.DTOs.Product;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class ProductService(IProductRepository repo, ILogger<ProductService> logger) : IProductService
    {
        public async Task<ResponseCenter<List<ProductListDto>>> GetProductList()
        {

            var products = await repo.GetAllProducts();
            if (products.Count == 0)
                return ResponseCenter.CreateErrorResponse<List<ProductListDto>>(
                    message: "No found products",
                    errorCode: "NO_PRODUCTS");

            var productsDto = products.Select(p => new ProductListDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.Name,
            }).ToList();

            return ResponseCenter.CreateSuccessResponse(data: productsDto);
        }

        public async Task<ResponseCenter<ProductDetailDto>> GetProductDetail(GetProductDto getProduct)
        {
            var product = await repo.GetProductById(getProduct.ProductId);
            if (product == null)
                return ResponseCenter.CreateErrorResponse<ProductDetailDto>(
                    message: "Product is null",
                    errorCode: "NULL");

            var productDetails = new ProductDetailDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category?.Name
            };

            return ResponseCenter.CreateSuccessResponse(data: productDetails);
        }

        public async Task<ResponseCenter<List<ProductListDto>>> GetProductSearch(ProductSearchDto searchProduct)
        {
            var result = await repo.GetProductSearch(searchProduct);
            if (result.Count == 0)
                return ResponseCenter.CreateErrorResponse<List<ProductListDto>>(
                    message: "No found products",
                    errorCode: "NO_PRODUCTS");

            var productsDto = result.Select(p => new ProductListDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Name
            }).ToList();

            return ResponseCenter.CreateSuccessResponse(data: productsDto);
        }

    }
}
