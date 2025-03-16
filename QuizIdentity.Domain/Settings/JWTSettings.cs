using Newtonsoft.Json;

namespace QuizIdentity.Domain.Settings;

public class JWTSettings
{
    public const string DefaultSection = "JWT";

    public string JwtKey { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Authority { get; set; }

    public int Expiry { get; set; }

    public int RefreshExpiry { get; set; }
}
