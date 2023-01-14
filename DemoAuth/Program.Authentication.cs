using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;


namespace DemoAuth
{
    /// <summary>
    /// Extension methods for API endpoints.
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Set up API authentication
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthentication(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration, "Auth");

            services.AddHttpContextAccessor();

            services.AddCors();
        }

        /// <summary>
        /// Add authentication middleware to the api.
        /// </summary>
        /// <param name="app"></param>
        public static void UseAuthenticationAndAuthorization(this WebApplication app)
        {
            app.UseAuthentication(); //must add UseAuthentication before UseAuthorization, or else [Authorize] attribute doesn't work
            app.UseAuthorization();

            app.UseCors(builder => {
                builder.WithOrigins(app.Configuration.GetSection("Auth:AllowedOrigins").Get<string[]>());
            });
        }
    }
}
