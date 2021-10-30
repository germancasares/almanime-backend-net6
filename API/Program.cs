using API.Settings;
using API.Utils;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Settings
var tokenSettings = builder.Configuration.GetSection(TokenSettings.Accessor).Get<TokenSettings>();
var swaggerSettings = builder.Configuration.GetSection(SwaggerSettings.Accessor).Get<SwaggerSettings>();
var frontendSettings = builder.Configuration.GetSection(FrontendSettings.Accessor).Get<FrontendSettings>();

builder.Services
    .AddConfiguration(builder.Configuration)
    .AddContext()
    .AddRepositories()
    .AddServices()
    .AddDuende(builder.Configuration)
    .AddCors()
    .AddControllers(opt =>
        {
            opt.Filters.Add(typeof(ValidatorActionFilter));
        }
    )
    .AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
    .AddFluentValidation();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    var doc = swaggerSettings.Doc;
    c.SwaggerDoc(doc.Version, new OpenApiInfo
    {
        Title = doc.Title,
        Description = doc.Description,
        Version = doc.Version,
        Contact = new OpenApiContact
        {
            Name = doc.Name,
            Url = new Uri(doc.Url ?? ""),
            Email = doc.Email,
        },
    });

    var req = swaggerSettings.SecurityRequirement;
    var reqIn = EnumHelper.GetEnumFromString<ParameterLocation>(req.In) ?? throw new ArgumentException(ExceptionMessage.ParameterNotValid);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                In = reqIn,
                Name = req.Name,
                Scheme = req.Scheme,
                Reference = new OpenApiReference
                {
                    Type = EnumHelper.GetEnumFromString<ReferenceType>(req.Type),
                    Id = req.Id
                },
            },
            new List<string>()
        }
    });

    var def = swaggerSettings.SecurityDefinition;
    var defType = EnumHelper.GetEnumFromString<SecuritySchemeType>(def.Type) ?? throw new ArgumentException(ExceptionMessage.ParameterNotValid);
    var defIn = EnumHelper.GetEnumFromString<ParameterLocation>(def.In) ?? throw new ArgumentException(ExceptionMessage.ParameterNotValid);

    c.AddSecurityDefinition(def.Scheme, new OpenApiSecurityScheme
    {
        Type = defType,
        Description = def.Description,
        Name = def.Name,
        In = defIn,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(def.AuthorizationUrl ?? ""),
                TokenUrl = new Uri(def.TokenUrl ?? ""),
                RefreshUrl = new Uri(def.RefreshUrl ?? ""),
                Scopes = new Dictionary<string, string>(),
            },
        },
    });
});

// Build app
var app = builder.Build();

// --------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();

    var swaggerUI = swaggerSettings.UI;
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(swaggerUI.EndpointUrl, swaggerUI.EndpointName);
        c.RoutePrefix = swaggerUI.RoutePrefix;
    });
}

app.UseHttpsRedirection();

app.UseCors(builder => builder.WithOrigins(frontendSettings.Urls.ToArray()).AllowAnyMethod().AllowAnyHeader().AllowCredentials());

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();