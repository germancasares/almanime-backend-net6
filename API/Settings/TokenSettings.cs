namespace API.Settings;

public class TokenSettings
{
    public const string Accessor = "TokenSettings";

    public string? Secret { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int AccessExpirationDays { get; set; }
    public int RefreshExpirationDays { get; set; }
}
