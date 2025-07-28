using VideoProjector.Common;
using VideoProjector.DTOs.Category;

namespace VideoProjector.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<ResponseCenter<List<CategoryDto>> Categories();
    }
}
