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
using System.Security.Claims;
using System.Linq;

namespace AbsCore.Blazor.Server.Data
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

        public async Task<IEnumerable<LcDocumentList>> GetDocumentsAsync()
        {
            var items = await JsonSerializer.DeserializeAsync<List<LcDocumentList>>
               (await _httpClient.GetStreamAsync("lc-documents"), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return items;
        }

    
    }
}