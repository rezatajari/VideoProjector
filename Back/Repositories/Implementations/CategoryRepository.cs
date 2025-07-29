using Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using VideoProjector.Data;
using VideoProjector.Models;

namespace Back.Repositories.Implementations
{
    public class CategoryRepository(VpDatabase db) : ICategoryRepository
    {
        public async Task<List<Category>> CategoriesAsync()
        {
            return await db.Categories
                           .AsNoTracking()
                           .Include(p => p.Products)
                           .ToListAsync();
        }

        public async Task<Category> CategoryAsync(int categoryId)
        {
         return await db.Categories
                .Where(c=>c.CategoryId==categoryId)
                .Select(c=>new Category
                {
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    Products = c.Products.Select(p=>new Product
                    {
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
