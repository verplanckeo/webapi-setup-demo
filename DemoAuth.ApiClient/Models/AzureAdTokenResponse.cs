using Newtonsoft.Json;

namespace DemoAuth.ApiClient.Models
{
    /// <summary>
    /// Model returned by Azure AD when using client_credentials flow
    /// </summary>
    internal class AzureAdTokenResponse
    {
        /// <summary>
        /// Type of token (i.e. Bearer).
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Time (in seconds) when token expires.
        /// </summary>
        [JsonProperty("expires_in")]
        public ushort ExpiresIn { get; set; }

        /// <summary>
        /// Epoch (linux) timestamp when token expires.
        /// </summary>
        [JsonProperty("expires_on")]
        public uint ExpiresOn { get; set; }

        /// <summary>
        /// The actual access token.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
