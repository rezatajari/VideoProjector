using VideoProjector.Common;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.DTOs.Category;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;


        public async Task<GeneralResponse<List<CategoryDto>>> Categories()
        {
            var categories = await _categoryRepository.CategoriesAsync();

            var categoriesDto = categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                ProductCount = c.Products?.Count ?? 0
            }).ToList();

            return GeneralResponse<List<CategoryDto>>.Success(data:categoriesDto);
        }
    }
}
