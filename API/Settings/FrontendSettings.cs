namespace API.Settings;

public class FrontendSettings
{
    public const string Accessor = "FrontendSettings";

    public IEnumerable<string> Urls { get; set; } = default!;
}
