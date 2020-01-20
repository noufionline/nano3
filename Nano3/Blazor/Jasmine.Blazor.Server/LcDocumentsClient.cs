using DevExpress.Office.Utils;
using IdentityModel.Client;
using Jasmine.Blazor.Server.Pages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
            var access_token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token"); //_httpContextAccessor.HttpContext.GetUserAccessTokenAsync();
            return access_token;
        }
    }
}