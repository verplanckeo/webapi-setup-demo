namespace DemoAuth.ApiClient.Models
{
    public class Auth
    {
        /// <summary>
        /// Url to log in with Azure.
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// Id of the tenant.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Settings specific for authenticating 
        /// </summary>
        public ApiClientSettings ApiClientSettings { get; set; }
    }

    /// <summary>
    /// ApiClientSettings
    /// </summary>
    public class ApiClientSettings
    {
        /// <summary>
        /// Client credentials (id & secret)
        /// </summary>
        public Credentials Credentials { get; set; }

        /// <summary>
        /// Url and scope of demo auth api.
        /// </summary>
        public DemoAuthSettings DemoAuth { get; set; }
    }

    /// <summary>
    /// Credentials class
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Id of app registration.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Secret of app registration.
        /// </summary>
        public string ClientSecret { get; set; }
    }

    /// <summary>
    /// Settings for demo auth api.
    /// </summary>
    public class DemoAuthSettings
    {
        public string Url { get; set; }
        public string Scopes { get; set; }
    }

}
