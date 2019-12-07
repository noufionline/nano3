using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Jasmine.Core.Contracts;

namespace Jasmine.Core.Services
{
    public class ApiTokenProvider : IApiTokenProvider
    {
        private readonly HttpClient _client;

        public ApiTokenProvider(HttpClient client)
        {
            _client = client;
        }
        public bool HasToken => !string.IsNullOrWhiteSpace(AccessToken);
        public void ReSet(string accessToken, string refreshToken, string expiresAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
        }

       
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        public string ExpiresAt { get; private set; }

       
        public async Task<string> GetTokenAsync()
        {
            

            bool expired = string.IsNullOrWhiteSpace(ExpiresAt) ||
                           DateTime.Parse(ExpiresAt).AddSeconds(-60).ToUniversalTime() < DateTime.UtcNow;

            //Debug.WriteLine($"Current Time : {DateTime.UtcNow}");
            //Debug.WriteLine($"Expires at : {ExpiresAt}");

            if (expired)
            {
                DiscoveryResponse disco = await _client.GetDiscoveryDocumentAsync();


                string refreshtoken = RefreshToken;

                var divisionId = Convert.ToInt32(ClaimsPrincipal.Current.FindFirst("divisionId")?.Value);

                if (divisionId == 0)
                    throw new InvalidOperationException("Division cannot be null. Please re login to continue...");


                var response = await _client.RequestRefreshTokenAsync(new RefreshTokenRequest()
                {
                    Address = disco.TokenEndpoint,
                  
                    ClientId = "abseROP",
                    ClientSecret = "e18ab171-8233-447b-bcb0-1e879613d141",
                    Parameters = { { "DivisionId", divisionId.ToString() } },
                    RefreshToken = refreshtoken
                    
                });

              

                if (!response.IsError)
                {
                    ExpiresAt = (DateTime.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn)).ToString("o",CultureInfo.InvariantCulture);

                    ReSet(response.AccessToken, response.RefreshToken, ExpiresAt);
                }
            }

            return AccessToken;
        }


        public string GetAuthority()
        {
#if DEBUG
            string environment = ConfigurationManager.AppSettings.Get("Environment");

            if (environment == "Production")
            {
                return ConfigurationManager.AppSettings.Get("IdServerEndPoint");
            }

            return ConfigurationManager.AppSettings.Get("LocalIdServerEndPoint");
#else
            return ConfigurationManager.AppSettings["IdServerEndPoint"];
#endif
        }



        public string GetApiEndPoint()
        {
#if DEBUG
            string enviorment = ConfigurationManager.AppSettings.Get("Environment");
            if (enviorment == "Production")
            {
                return ConfigurationManager.AppSettings.Get("HttpBaseAddress");
            }

            return ConfigurationManager.AppSettings.Get("LocalHttpBaseAddress");
#else
            return ConfigurationManager.AppSettings.Get("HttpBaseAddress");
#endif
        }
        
    }
}