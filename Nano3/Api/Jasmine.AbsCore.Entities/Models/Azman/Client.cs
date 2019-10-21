using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class Client:TrackableEntityBase
    {
        public Client()
        {
            ClientClaims = new HashSet<ClientClaim>();
            ClientCorsOrigins = new HashSet<ClientCorsOrigin>();
            ClientGrantTypes = new HashSet<ClientGrantType>();
            ClientIdPrestrictions = new HashSet<ClientIdPrestriction>();
            ClientPostLogoutRedirectUris = new HashSet<ClientPostLogoutRedirectUri>();
            ClientRedirectUris = new HashSet<ClientRedirectUri>();
            ClientScopes = new HashSet<ClientScope>();
            ClientSecrets = new HashSet<ClientSecret>();
        }

        public int Id { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int AccessTokenType { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUri { get; set; }
        public bool EnableLocalLogin { get; set; }
        public bool Enabled { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public bool IncludeJwtId { get; set; }
        public string LogoUri { get; set; }
        public bool LogoutSessionRequired { get; set; }
        public string LogoutUri { get; set; }
        public bool PrefixClientClaims { get; set; }
        public string ProtocolType { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public int RefreshTokenUsage { get; set; }
        public bool RequireClientSecret { get; set; }
        public bool RequireConsent { get; set; }
        public bool RequirePkce { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        public ICollection<ClientClaim> ClientClaims { get; set; }
        public ICollection<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public ICollection<ClientGrantType> ClientGrantTypes { get; set; }
        public ICollection<ClientIdPrestriction> ClientIdPrestrictions { get; set; }
        public ICollection<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        public ICollection<ClientRedirectUri> ClientRedirectUris { get; set; }
        public ICollection<ClientScope> ClientScopes { get; set; }
        public ICollection<ClientSecret> ClientSecrets { get; set; }
    }
}
