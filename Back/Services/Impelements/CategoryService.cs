using Back.DTOs.Category;
using Back.DTOs.Product;
using Back.Repositories.Interfaces;
using VideoProjector.Common;
using VideoProjector.DTOs.Category;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        public async Task<GeneralResponse<List<CategoryDto>>> Categories()
        {
            var categories = await categoryRepository.CategoriesAsync();

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

        public async Task<GeneralResponse<CategoryDetailsDto>> Category(int categoryId)
        {
            var category = await categoryRepository.CategoryAsync(categoryId);


            var dto = new CategoryDetailsDto
            {
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                TotalProducts = category.Products?.Count ?? 0,
                Products = category.Products?.Select(
                        p => new ProductSummaryDto
                        {
                            Name = p.Name,
                            ShortDescription = p.Description,
                            Price = p.Price,
                            StockCount = p.StockQuantity
                        })
                    .ToList() ?? []
            };

            return GeneralResponse<CategoryDetailsDto>.Success(data:dto);
        }
    }
}
