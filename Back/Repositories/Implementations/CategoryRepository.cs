using Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using VideoProjector.Data;
using VideoProjector.Models;

namespace Back.Repositories.Implementations
{
    public class CategoryRepository(VpDatabase db) : ICategoryRepository
    {
        private readonly VpDatabase _db = db;
        public async Task<List<Category>> CategoriesAsync()
        {
            return await _db.Categories
                           .AsNoTracking()
                           .Include(p => p.Products)
                           .ToListAsync();
        }
    }
}
