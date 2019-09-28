using Newtonsoft.Json;

namespace SbpExampleShop.Backend.Integration.Models
{
    public class AuthResponse
    {
        [JsonProperty("access_token")] 
        public string AccessToken { get; set; }
        
        [JsonProperty("expires_in")] 
        public string ExpiresIn { get; set; }
        
        [JsonProperty("token_type")] 
        public string TokenType { get; set; }
    }
}