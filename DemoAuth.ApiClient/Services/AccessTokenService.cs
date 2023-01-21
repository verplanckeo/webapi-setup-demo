using DemoAuth.ApiClient.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAuth.ApiClient.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly Auth _authSettings;

        public AccessTokenService(IHttpClientFactory httpClientFactory, IMemoryCache cache, IOptions<Auth> authSettings)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _authSettings = authSettings.Value;
        }

        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            const string cachekey = "authdemoapi";
            if (_cache.TryGetValue<AzureAdTokenResponse>(cachekey, out var token))
            {
                return token.AccessToken;
            }

            var url = $"{_authSettings.TenantId}/oauth2/v2.0/token";
            var body = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _authSettings.ApiClientSettings.Credentials.ClientId },
                { "client_secret", _authSettings.ApiClientSettings.Credentials.ClientSecret },
                { "scope", _authSettings.ApiClientSettings.DemoAuth.Scopes },
            });

            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(_authSettings.Instance);
                var httpResponse = await client.PostAsync(url, body);
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new UnauthorizedAccessException($"Unable to get an access token for scope: {_authSettings.ApiClientSettings.DemoAuth.Scopes}\n\r{responseContent}");
                }
                var tokenResponse = JsonConvert.DeserializeObject<AzureAdTokenResponse>(responseContent);

                _cache.Set(cachekey, tokenResponse, DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 30));
                return tokenResponse.AccessToken;
            }
        }
    }
}
