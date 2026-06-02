using Duende.IdentityServer.Models;

namespace IdentityService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("auctionApp","Auction app full access")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "postman",
                    ClientName = "Postman",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("NotASecret".Sha256()) },
                    RedirectUris = {"https://www.getpostman.com/oauth2/callback"},

                    AllowedScopes = { "openid","profile", "auctionApp" }
                },

                new Client
                {
                    ClientId = "nextApp",
                    ClientName = "nextApp",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    RedirectUris = {"http://localhost:3000/api/auth/callback/id-server"},
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid","profile", "auctionApp" },
                    AccessTokenLifetime = 3600 * 24 * 30
                },

            };
    }
}
