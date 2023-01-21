using DemoAuth.ApiClient.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace DemoAuth.ApiClient.Services
{
    public abstract class BaseIntegrationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccessTokenService _accessTokenService;
        private readonly Auth _authSettings;
        protected readonly ILogger Logger;

        /// <summary>
        /// CTor
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="accessTokenService"></param>
        /// <param name="logger"></param>
        public BaseIntegrationService(IHttpClientFactory httpClientFactory, IAccessTokenService accessTokenService, IOptions<Auth> authSettings, ILogger logger)
        {
            _httpClientFactory = httpClientFactory;
            _accessTokenService = accessTokenService;
            _authSettings = authSettings.Value;
            Logger = logger;
        }

        private async Task<HttpClient> GetHttpClient(CancellationToken cancellationToken, bool forceNewSession = false)
        {
            var token = await _accessTokenService.GetAccessTokenAsync(cancellationToken);

            var client = _httpClientFactory.CreateClient("moviesclient");
            client.BaseAddress = new System.Uri(_authSettings.ApiClientSettings.DemoAuth.Url);
            client.Timeout = TimeSpan.FromSeconds(30);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("api-version", "1.0");

            return client;
        }

        private async Task<T> InvokeHttpRequest<T>(Func<HttpClient, string, CancellationToken, Task<HttpResponseMessage>> func, string url, CancellationToken cancellationToken)
        {
            using var client = await GetHttpClient(cancellationToken);
            try
            {
                var httpResponse = await func(client, url, cancellationToken);
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    Logger.LogError("<Movies Api> Http request (url: {0}) failed with an unsuccessful statuscode (Code: {1}). Response: {2}", url, httpResponse.StatusCode, responseContent);
                    return default;
                }

                if (string.IsNullOrEmpty(responseContent)) return default;

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (Exception ex)
            {
                Logger.LogError($"<Movies Api> An exception ocurred: {ex.Message}", ex);
                throw new Exception(JsonConvert.SerializeObject(new { Url = url, Message = ex.Message }), ex);
            }
        }

        /// <summary>
        /// Execute an HTTP GET for the given relative url.
        /// </summary>
        /// <typeparam name="T">Type of data to return.</typeparam>
        /// <param name="url">Relative url (i.e.: /movies)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        protected async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken) where T : class, new()
        {
            var result = await InvokeHttpRequest<T>(async (client, relativeUrl, token) => 
            { 
                return await client.GetAsync(relativeUrl, cancellationToken); 
            }, url, cancellationToken);
            return result;
        }

        /// <summary>
        /// Execute an HTTP DELETE for the given relative url.
        /// </summary>
        /// <typeparam name="T">Type of data to return.</typeparam>
        /// <param name="url">Relative url (i.e.: /movies/1)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        protected async Task<T> DeleteAsync<T>(string url, CancellationToken cancellationToken)
        {
            var result = await InvokeHttpRequest<T>(async (client, relativeUrl, token) => { return await client.DeleteAsync(relativeUrl, cancellationToken); }, url, cancellationToken);
            return result;
        }

        /// <summary>
        /// Execute an HTTP POST for the given relative url.
        /// </summary>
        /// <typeparam name="T">Type of data to return.</typeparam>
        /// <param name="url">Relative url (i.e.: /v2/services/projects)</param>
        /// <param name="body">Payload to send</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        protected async Task<T> PostAsync<TBody, T>(string url, TBody body, CancellationToken cancellationToken) where T : class, new()
        {
            var result = await InvokeHttpRequest<T>(async (client, relativeUrl, token) => { return await client.PostAsync(relativeUrl, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"), cancellationToken); }, url, cancellationToken);
            return result;
        }
    }
}
