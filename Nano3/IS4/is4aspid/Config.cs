// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace is4aspid
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("abscoreapi", "Abs Core Api")
                {
                    DisplayName = "Abs Core Api",
                    UserClaims =
                    {
                        "EmployeeId",
                        "EmployeeName",
                        "DivisionId",
                        "Email"
                    }
                }
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "abseROP",
                    ClientName = "Abs Enterprise",
                    AllowedGrantTypes ={ GrantType.ResourceOwnerPassword,"jasmine" },

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("e18ab171-8233-447b-bcb0-1e879613d141".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "abscoreapi",
                        "absodataapi",
                        "absreportsapi",
                        "absclaimsapi",
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.Email
                    },
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true
                },
                new Client
                {
                    ClientId = "abscoreapi",
                    ClientName = "Abs Enterprise Core Api",
                    AllowedGrantTypes = {"delegation"},


                    ClientSecrets = new List<Secret>
                    {
                        new Secret("e18ab171-8233-447b-bcb0-1e879613d141".Sha256())
                    },
                    AllowedScopes =   { "absclaimsapi" }
                },

                new Client
                {
                    ClientId ="abscoreblazor",
                    ClientName="Abs Core Blazor Server Project",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets={new Secret("5651841c-9615-4d1e-bf79-81a382faac81".Sha256())},

                    RedirectUris={"https://localhost:44309/signin-oidc"},
                    FrontChannelLogoutUri="https://localhost:44309/signout-oidc",
                    PostLogoutRedirectUris={"https://localhost:44309/signout-callback-oidc"},
                    
                    //RedirectUris={"https://abs.cicononline.com/kpi/signin-oidc"},
                    //FrontChannelLogoutUri="https://abs.cicononline.com/kpi/signout-oidc",
                    //PostLogoutRedirectUris={"https://abs.cicononline.com/kpi/signout-callback-oidc"},


                    EnableLocalLogin=false,
                    AllowOfflineAccess=true,
                    RequireConsent=false,
                    AllowedScopes={"openid","profile","email","abscoreapi"}
                }
            };
    }
}