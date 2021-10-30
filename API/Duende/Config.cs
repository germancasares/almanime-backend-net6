using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace API.Duende;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("api", "Almanime API")
        };

    public static IEnumerable<Client> Clients => new List<Client>
        {
            // machine to machine client
            new Client
            {
                ClientId = "machine",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "almanime" }
            },
            // interactive ASP.NET Core MVC client
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
            
                // where to redirect to after login
                RedirectUris = { "https://localhost:7006/signin-oidc" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:7006/signout-callback-oidc" },

                AllowOfflineAccess = true,

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "almanime"
                }
            }
        };
}
