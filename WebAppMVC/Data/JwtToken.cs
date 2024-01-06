using Newtonsoft.Json;

namespace WebAppMVC.Data
{
    public class JwtToken
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }
    }
}
