using Back.DTOs.Product;
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

        public async Task<GeneralResponse<ProductDetailsDto>> Detail(int productId)
        {
            var product = await repo.Details(productId);
            if (product == null)
                return GeneralResponse<ProductDetailsDto>.Failure(message: "Product is null");

            var productDetails = new ProductDetailsDto(
                product.ProductId,
                product.Name,
                product.Description,
                product.Price,
                product.StockQuantity,
                product.ImageUrl
            );

            return GeneralResponse<ProductDetailsDto>.Success(data: productDetails);
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
