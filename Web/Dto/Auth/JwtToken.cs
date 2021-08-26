using System;
using System.Text.Json.Serialization;

namespace Web.Dto.Auth
{
    public class JwtToken
    {
        public string Token { get; init; }
        public DateTime Expires { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RefreshToken { get; set; }

        [JsonIgnore]
        public DateTime RefreshExpires { get; set; } = DateTime.Now;

        public AuthUser User { get; init; }
    }
}