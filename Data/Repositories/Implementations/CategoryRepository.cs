using VideoProjector.Data.Repositories.Interfaces;

namespace VideoProjector.Data.Repositories.Implementations
{
    public class CategoryRepository(VpDatabase db) : ICategoryRepository
    {
        private readonly VpDatabase _db = db;
    }
}
