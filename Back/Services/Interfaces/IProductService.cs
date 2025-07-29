using VideoProjector.Common;
using VideoProjector.DTOs.Product;

namespace VideoProjector.Services.Interfaces
{
    public interface IProductService
    {
        Task<GeneralResponse<List<ProductListDto>>> GetProductList();
        Task<GeneralResponse<ProductDetailDto>> GetProductDetail(GetProductDto getProduct);
        Task<GeneralResponse<List<ProductListDto>>> GetProductSearch(ProductSearchDto searchProduct);
    }
}
