namespace DemoAuth.ApiClient.Services
{
    public interface IAccessTokenService
    {
        /// <summary>
        /// Retrieve an accesstoken.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
    }
}
