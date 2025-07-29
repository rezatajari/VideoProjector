using VideoProjector.Common;
using VideoProjector.DTOs.Category;
using VideoProjector.Models;

namespace Back.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> CategoriesAsync();
        Task<Category> CategoryAsync(int categoryId);
    }
}
