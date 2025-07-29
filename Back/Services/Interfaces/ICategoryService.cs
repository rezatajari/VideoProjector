using VideoProjector.Common;
using VideoProjector.DTOs.Category;

namespace VideoProjector.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<GeneralResponse<List<CategoryDto>>> Categories();
    }
}
