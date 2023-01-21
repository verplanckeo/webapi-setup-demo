using DemoAuth.ApiClient.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class ConfigurationExtensions
{
    public static IHostBuilder AddAppConfiguration(this IHostBuilder host, string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Development;

        host.UseEnvironment(environment)
            .ConfigureAppConfiguration((context, builder) =>
         {
             builder.SetBasePath(Directory.GetCurrentDirectory());
             builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
             builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true);
             builder.AddUserSecrets<Program>();
             builder.AddEnvironmentVariables();
             builder.AddCommandLine(args);
         });

        return host;
    }

    public static IServiceCollection AddOptionsConfiguration(this IServiceCollection services, HostBuilderContext hostContext)
    {
        services.Configure<Auth>(hostContext.Configuration.GetSection(nameof(Auth)));

        return services;
    }
}