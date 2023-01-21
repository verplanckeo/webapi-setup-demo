using DemoAuth.ApiClient.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DemoAuth.ApiClient
{
    public class HostedService : IHostedService
    {
        private readonly IMoviesIntegrationService _moviesIntegrationService;
        private readonly ILogger _logger;

        public HostedService(
            IMoviesIntegrationService moviesIntegrationService,
            ILogger<HostedService> logger,
            IHostApplicationLifetime appLifetime
            )
        {
            _moviesIntegrationService = moviesIntegrationService;
            _logger = logger;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("1. {0} has been called.", nameof(StartAsync));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }

        private async void OnStarted()
        {
            _logger.LogDebug("2. OnStarted has been called.");

            try
            {
                var result = await _moviesIntegrationService.GetAllMoviesAsync(default);
                _logger.LogInformation(JsonConvert.SerializeObject(result));

                var deletedResult = await _moviesIntegrationService.RemoveOneMovieAsync("10", default);
                _logger.LogInformation(deletedResult);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private void OnStopping()
        {
            _logger.LogDebug("3. OnStopping has been called.");
        }

        private void OnStopped()
        {
            _logger.LogDebug("5. OnStopped has been called.");
        }
    }
}
