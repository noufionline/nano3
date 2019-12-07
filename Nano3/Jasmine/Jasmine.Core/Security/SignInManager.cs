using IdentityModel.Client;
using Jasmine.Core.Contracts;
using Jasmine.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jasmine.Core.Security
{
    public class SignInManager : RestApiRepositoryBase, ISignInManager
    {
        private readonly IApiTokenProvider _apiTokenProvider;
        private readonly HttpClient _discoveryClient;
        private readonly IHttpClientFactory _factory;
        public SignInManager(IHttpClientFactory factory, IApiTokenProvider apiTokenProvider) : base(factory)
        {
            _factory = factory;
            _apiTokenProvider = apiTokenProvider;
            _discoveryClient = _factory.CreateClient("zeon");
        }

        private async Task<ClaimsPrincipal> CreateClaimsPrincipal()
        {
            List<UserClaim> claims = await ReadAsStreamAsync<List<UserClaim>>("claims");

            ClaimsIdentity newidentity = new ClaimsIdentity("Abs", "Email", ClaimTypes.Role);
            foreach (UserClaim claim in claims)
            {
                newidentity.AddClaim(new Claim(claim.Type, claim.Value, null, claim.Issuer));
            }
            return new ClaimsPrincipal(newidentity);
        }


        private async Task<ClaimsPrincipal> SignIn(string userName, string password, int divisionId)
        {

            DiscoveryResponse disco = await _discoveryClient.GetDiscoveryDocumentAsync();

            var tokenResponse = await _discoveryClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                GrantType = "password",


                UserName = userName,
                Password = password,


                Scope = "abscoreapi  offline_access",
                ClientId = "abseROP",
                ClientSecret = "e18ab171-8233-447b-bcb0-1e879613d141",
                Parameters = { { "DivisionId", divisionId.ToString() } }
            });

            if (tokenResponse.IsError)
            {
                return new ClaimsPrincipal(new ClaimsIdentity());
            }

            DateTime expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            //Debug.WriteLine($"Current Time : {DateTime.UtcNow}");
            //Debug.WriteLine($"Expires at : {expiresAt}");

            _apiTokenProvider.ReSet(tokenResponse.AccessToken, tokenResponse.RefreshToken, expiresAt.ToString("o", CultureInfo.InvariantCulture));

            return await CreateClaimsPrincipal();

        }

        public async Task ChangeDivisionAsync(int divisionId)
        {

            DiscoveryResponse disco = await _discoveryClient.GetDiscoveryDocumentAsync();

            string token = _apiTokenProvider.AccessToken;

            var tokenResponse = await _discoveryClient.RequestTokenAsync(new TokenRequest()
            {
                Address = disco.TokenEndpoint,
                GrantType = "jasmine",
                ClientId = "abseROP",
                ClientSecret = "e18ab171-8233-447b-bcb0-1e879613d141",
                Parameters =
                {
                    { "DivisionId", divisionId.ToString() },
                    { "token" ,token} ,
                    { "scope", "abscoreapi  offline_access"}
                }
            });



            if (tokenResponse.IsError)
            {
                PrincipalProvider.Current.Principal = new ClaimsPrincipal(new ClaimsIdentity());
                return;
            }

            DateTime expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            Debug.WriteLine($"Current Time : {DateTime.UtcNow}");
            Debug.WriteLine($"Expires at : {expiresAt}");

            _apiTokenProvider.ReSet(tokenResponse.AccessToken, tokenResponse.RefreshToken, expiresAt.ToString("o", CultureInfo.InvariantCulture));

            PrincipalProvider.Current.Principal = await CreateClaimsPrincipal();

        }
        public async Task<bool> SavePasswordAsync(string email, string password)
        {
            string request = $"accounts";
            return await PutAndReturnStatusWithStreamAsync(request, new { Email = email, Password = password });
        }

        public async Task<ClaimsPrincipal> SignInAsync(UserCredential credential)
        {
            ClaimsPrincipal principal = await SignIn(credential.UserName, credential.Password, credential.DivisionId);
            return principal;
        }

        public async Task<ClaimsPrincipal> SignInAsync(string userName, string password, int divisionId) => await SignIn(userName, password, divisionId);

        public async Task<bool> ValidateLoginAsync(string email, string password)
        {
            string request = $"accounts/validate/{email}/{password}";
            return await ReadAsAsync<bool>(request);
        }


    }

}