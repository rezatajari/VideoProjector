using VideoProjector.Services.Impelements.Account;

namespace VideoProjector.Configuration
{
    public static class EmailConfiguration
    {
        public static void ConfigureEmail(this IServiceCollection services)
        {
            services.AddScoped<EmailConfirmationService>();
        }
    }
}
