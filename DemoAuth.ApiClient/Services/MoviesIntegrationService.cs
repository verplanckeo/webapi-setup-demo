using DemoAuth.ApiClient.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DemoAuth.ApiClient.Services
{
    public class MoviesIntegrationService : BaseIntegrationService, IMoviesIntegrationService
    {
        public MoviesIntegrationService(IHttpClientFactory httpClientFactory, IAccessTokenService accessTokenService, IOptions<Auth> authSettings, ILogger<MoviesIntegrationService> logger):
            base(httpClientFactory, accessTokenService, authSettings, logger)
        {
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync(CancellationToken cancellationToken)
        {
            return await GetAsync<List<Movie>>("/movies", cancellationToken);
        }

        public async Task<string> RemoveOneMovieAsync(string id, CancellationToken cancellationToken)
        {
            return await DeleteAsync<string>($"/movies/{id}", cancellationToken);
        }
    }
}
