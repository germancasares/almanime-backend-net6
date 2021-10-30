using API.Settings.Swagger;

namespace API.Settings;

public class SwaggerSettings
{
    public const string Accessor = "SwaggerSettings";

    public SwaggerDoc Doc { get; set; } = default!;
    public SwaggerSecurityDefinition SecurityDefinition { get; set; } = default!;
    public SwaggerSecurityRequirement SecurityRequirement { get; set; } = default!;
    public SwaggerUI UI { get; set; } = default!;
}
