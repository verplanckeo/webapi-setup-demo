using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .AddAppConfiguration(args)
    .AddServices(args);

hostBuilder.ConfigureLogging((context, builder) =>
{
    builder.AddConfiguration(context.Configuration.GetSection("Logging"));
});


var host = hostBuilder.Build();
await host.RunAsync();