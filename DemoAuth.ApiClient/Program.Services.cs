using DemoAuth.ApiClient;
using DemoAuth.ApiClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class ServicesExtensions
{
    public static IHostBuilder AddServices(this IHostBuilder host, string[] args)
    {
        host.ConfigureServices((context, services) =>
        {
            services.AddSingleton<IAccessTokenService, AccessTokenService>();
            services.AddSingleton<IMoviesIntegrationService, MoviesIntegrationService>(); //singleton because hostedservice is singleton

            services.AddHostedService<HostedService>();

            services.AddOptionsConfiguration(context);

            services.AddMemoryCache();
            services.AddHttpClient();
        });

        return host;
    }
}