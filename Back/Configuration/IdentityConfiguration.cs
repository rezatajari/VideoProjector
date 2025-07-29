using Microsoft.AspNetCore.Identity;
using VideoProjector.Data;
using VideoProjector.Models;

namespace VideoProjector.Configuration
{
    public static class IdentityConfiguration
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<Customer, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
        .AddEntityFrameworkStores<VpDatabase>()
        .AddDefaultTokenProviders();
        }
    }
}
