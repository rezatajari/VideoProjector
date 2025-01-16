using VideoProjector.Common;
using VideoProjector.DTOs.Product;

namespace VideoProjector.Services.Interfaces
{
    public interface IProductService
    {
        Task<ResponseCenter<List<ProductListDto>>> GetProductList();
        Task<ResponseCenter<ProductDetailDto>> GetProductDetail(GetProductDto getProduct);
        Task<ResponseCenter<List<ProductListDto>>> GetProductSearch(ProductSearchDto searchProduct);
        Task<ResponseCenter<List<ProductListDto>>> GetProductByCategory(CategoryDto listByCategory);
    }
}
