using Microsoft.AspNetCore.Identity;
using VideoProjector.Data;

namespace VideoProjector.Configuration.IdentityLibrary
{
    public static class IdentityConfiguration
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<VpDatabase>()
                .AddDefaultTokenProviders();
        }
    }
}
