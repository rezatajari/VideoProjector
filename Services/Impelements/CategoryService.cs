using VideoProjector.Common;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.DTOs.Category;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;


        public async Task<ResponseCenter<List<CategoryDto>>> Categories()
        {
        }
    }
}
