using Jasmine.Blazor.Server.Pages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jasmine.Blazor.Server
{
    public interface ILcDocumentService
    {
        Task<List<LcDocumentList>> GetDocumentsAsync();
        Task<string> GetAccessTokenAsync();

        List<Claim> GetClaims();
    }
}