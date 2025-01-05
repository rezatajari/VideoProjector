using Microsoft.AspNetCore.Identity;
using VideoProjector.Data;

namespace VideoProjector.Configuration
{
    public static class IdentityConfiguration
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
        .AddEntityFrameworkStores<VpDatabase>()
        .AddDefaultTokenProviders();
        }
    }
}
