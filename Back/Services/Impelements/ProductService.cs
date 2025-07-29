using Back.Repositories.Interfaces;
using VideoProjector.Common;
using VideoProjector.DTOs.Product;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class ProductService(IProductRepository repo, ILogger<ProductService> logger) : IProductService
    {
        public async Task<GeneralResponse<List<ProductListDto>>> GetProductList()
        {

            var products = await repo.GetAllProducts();
            if (products.Count == 0)
                return GeneralResponse<List<ProductListDto>>.Failure(message: "No found products");

            var productsDto = products.Select(p => new ProductListDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.Name,
            }).ToList();

            return GeneralResponse<List<ProductListDto>>.Success(data: productsDto);
        }

        public async Task<GeneralResponse<ProductDetailDto>> GetProductDetail(GetProductDto getProduct)
        {
            var product = await repo.GetProductById(getProduct.ProductId);
            if (product == null)
                return GeneralResponse<ProductDetailDto>.Failure(message: "Product is null");

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

            return GeneralResponse<ProductDetailDto>.Success(data: productDetails);
        }

        public async Task<GeneralResponse<List<ProductListDto>>> GetProductSearch(ProductSearchDto searchProduct)
        {
            var result = await repo.GetProductSearch(searchProduct);
            if (result.Count == 0)
                return GeneralResponse<List<ProductListDto>>.Failure(message: "No found products");

            var productsDto = result.Select(p => new ProductListDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Name
            }).ToList();

            return GeneralResponse<List<ProductListDto>>.Success(data: productsDto);
        }

    }
}
