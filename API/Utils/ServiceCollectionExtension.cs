using API.Duende;
using API.Models;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Interfaces;
using API.Settings;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace API.Utils;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAnimeRepository, AnimeRepository>();
        services.AddScoped<IBookmarkRepository, BookmarkRepository>();
        services.AddScoped<IEpisodeRepository, EpisodeRepository>();
        services.AddScoped<IFansubRepository, FansubRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IStorageRepository, StorageRepository>();
        services.AddScoped<ISubtitleRepository, SubtitleRepository>();
        services.AddScoped<ISubtitlePartialRepository, SubtitlePartialRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        //services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAnimeService, AnimeService>();
        //services.AddScoped<IBookmarkService, BookmarkService>();
        //services.AddScoped<IEpisodeService, EpisodeService>();
        //services.AddScoped<IFansubService, FansubService>();
        //services.AddScoped<ISubtitleService, SubtitleService>();
        //services.AddScoped<IUserService, UserService>();

        return services;
    }

    //private static IServiceCollection AddValidators(this IServiceCollection services)
    //{
    //    services.AddTransient<IValidator<LoginDTO>, LoginDTOValidator>();
    //    services.AddTransient<IValidator<RegisterDTO>, RegisterDTOValidator>();
    //    services.AddTransient<IValidator<UserDTO>, UserDTOValidator>();
    //    services.AddTransient<IValidator<SubtitleDTO>, SubtitleDTOValidator>();

    //    return services;
    //}

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<FrontendSettings>()
            .Bind(config.GetSection(FrontendSettings.Accessor))
            .ValidateDataAnnotations();

        services.AddOptions<TokenSettings>()
            .Bind(config.GetSection(TokenSettings.Accessor))
            .ValidateDataAnnotations();

        services.AddOptions<SwaggerSettings>()
            .Bind(config.GetSection(SwaggerSettings.Accessor))
            .ValidateDataAnnotations();

        return services;
    }

    public static IServiceCollection AddContext(this IServiceCollection services, string connectionString = "Name=AlmanimeConnection")
        => services.AddDbContext<AlmanimeContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));

    //public static IServiceCollection AddAlmAuthentication(this IServiceCollection services, TokenSettings tokenOptions)
    //{
    //    services.AddAuthentication(options =>
    //    {
    //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    //    }).AddJwtBearer(jwtBearerOptions =>
    //    {
    //        jwtBearerOptions.Authority = "https://localhost:7006";

    //        jwtBearerOptions.SaveToken = true;
    //        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    //        {
    //            ValidateIssuerSigningKey = true,
    //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Secret ?? throw new ArgumentException(ExceptionMessage.ParameterNotValid))),

    //            ValidateIssuer = true,
    //            ValidIssuer = tokenOptions.Issuer,

    //            ValidateAudience = true,
    //            ValidAudience = tokenOptions.Audience,

    //            ValidateLifetime = true, //validate the expiration and not before values in the token

    //            ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
    //        };
    //    });

    //    return services;
    //}

    public static IServiceCollection AddDuende(this IServiceCollection services, IConfiguration config)
    {
        var persistedGrantConnection = config.GetConnectionString("PersistedGrantConnection");
        var configurationDB = config.GetConnectionString("ConfigurationConnection");

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        services
            .AddIdentity<User, Role>()
            .AddEntityFrameworkStores<AlmanimeContext>()
            .AddDefaultTokenProviders();

        services
            .AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api");
                });
            })
            .AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(configurationDB);
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(persistedGrantConnection);
            })
            .AddAspNetIdentity<User>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddGoogle("Google", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                options.ClientId = "<insert here>";
                options.ClientSecret = "<insert here>";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:7006";

                options.ClientId = "almanime";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.Scope.Add("profile");
                options.Scope.Add("almanime");
                options.Scope.Add("offline_access");
                    //options.ClaimActions.MapUniqueJsonKey("myclaim1", "myclaim1");
                    options.GetClaimsFromUserInfoEndpoint = true;

                options.SaveTokens = true;
            });

        return services;
    }

    //private static IServiceCollection AddIdentity(this IServiceCollection services)
    //{
    //    services
    //        .AddIdentity<IdentityUser, IdentityRole>()
    //        .AddEntityFrameworkStores<SecurityContext>()
    //        .AddDefaultTokenProviders();

    //    services.Configure<IdentityOptions>(options =>
    //    {
    //        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    //        options.Password.RequiredUniqueChars = 0;

    //        options.User.RequireUniqueEmail = true;
    //    });

    //    services.ConfigureApplicationCookie(options =>
    //    {
    //        options.Cookie.HttpOnly = true;
    //        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    //        options.LoginPath = "/Identity/Account/Login";
    //        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    //        options.SlidingExpiration = true;
    //    });

    //    services.AddDbContext<SecurityContext>(options => options.UseSqlServer("Name=SecurityConnection", b => b.MigrationsAssembly("Migrations.Security")));

    //    return services;
    //}

}
