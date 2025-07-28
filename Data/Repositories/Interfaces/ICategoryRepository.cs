using VideoProjector.Common;
using VideoProjector.DTOs.Category;
using VideoProjector.Models;

namespace VideoProjector.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> CategoriesAsync();
    }
}
