using Back.DTOs.Product;
using VideoProjector.Common;
using VideoProjector.DTOs.Product;

namespace VideoProjector.Services.Interfaces
{
    public interface IProductService
    {
        Task<GeneralResponse<List<ProductListDto>>> ProductList();
        Task<GeneralResponse<ProductDetailsDto>> Detail(int productId);
        Task<GeneralResponse<List<ProductListDto>>> GetProductSearch(ProductSearchDto searchProduct);
    }
}
