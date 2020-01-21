using DevExpress.Office.Utils;
using IdentityModel.Client;
using Jasmine.Blazor.Server.Pages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using DevExpress.Logify.Web;
using System;
using System.Security.Claims;
using System.Linq;

namespace Jasmine.Blazor.Server
{
    public class LcDocumentService : ILcDocumentService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public LcDocumentService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<LcDocumentList>> GetDocumentsAsync()
        {
            //var access_token = await _httpContextAccessor.HttpContext.GetUserAccessTokenAsync();
            //_httpClient.SetBearerToken(access_token);

            var items = await JsonSerializer.DeserializeAsync<List<LcDocumentList>>
               (await _httpClient.GetStreamAsync("lc-documents"), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return items;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            string access_token = string.Empty;
            //try
            //{
            //    LogifyAlert.Instance.ApiKey = _configuration.GetSection("LogifyAlert").Value;
            access_token = await _httpContextAccessor.HttpContext.GetUserAccessTokenAsync();

            //}
            //catch (Exception e)
            //{
            //    LogifyAlert.Instance.Send(e);
            //}

            return access_token;

        }

        public List<Claim> GetClaims()
        {
            var user = _httpContextAccessor.HttpContext.User;

            return user.Claims.ToList();

        }
    }
}