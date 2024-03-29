namespace API.Extras;
public class JwtOptions
{
    public const string Position = "Jwt";

    public string Issuer { get; set; } = String.Empty;
    public string Audience { get; set; } = String.Empty;
    public string Key { get; set; } = String.Empty;
}