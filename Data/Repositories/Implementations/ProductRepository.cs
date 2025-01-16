using Microsoft.EntityFrameworkCore;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.DTOs.Product;
using VideoProjector.Models;

namespace VideoProjector.Data.Repositories.Implementations
{
    public class ProductRepository(VpDatabase database) : IProductRepository
    {
        public async Task<List<Product>?> GetAllProducts()
        {
            try
            {
                var products = await database.Products
                    .Include(c => c.Category)
                    .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception($"Get products operation is failed from database: {ex.Message}");
            }
        }

        public async Task<Product?> GetProductById(int productId)
        {
            try
            {
                var product = await database.Products
                    .Include(c => c.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception($"Get product details operation is failed from database: {ex}");
            }
        }

        public async Task<List<Product>?> GetProductSearch(ProductSearchDto searchDto)
        {
            var query = database.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
                query = query.Where(p => p.Name.Contains(searchDto.SearchTerm));
            if (searchDto.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == searchDto.CategoryId.Value);
            if (searchDto.MinPrice.HasValue)
                query = query.Where(p => p.Price >= searchDto.MinPrice.Value);
            if (searchDto.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= searchDto.MaxPrice.Value);

            try
            {
                // Execute the query
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Search operation is failed", ex);
            }
        }
    }
}
