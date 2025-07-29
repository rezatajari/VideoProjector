using VideoProjector.DTOs.Product;
using VideoProjector.Models;

namespace Back.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int productId);
        Task<List<Product>> GetProductSearch(ProductSearchDto searchDto);
    }
}
