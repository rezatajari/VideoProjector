using VideoProjector.DTOs.Product;
using VideoProjector.Models;

namespace VideoProjector.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>?> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<List<Product>?> GetProductSearch(ProductSearchDto searchDto);
    }
}
