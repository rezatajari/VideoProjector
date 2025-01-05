using Microsoft.EntityFrameworkCore;
using VideoProjector.Data;


namespace VideoProjector.Configuration.Db
{
    public static class DatabaseConfiguration
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<VpDatabase>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
