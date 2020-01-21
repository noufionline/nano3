using Jasmine.Blazor.Server.Pages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AbsCore.Blazor.Server.Data
{
    public interface ILcDocumentService
    {
        Task<IEnumerable<LcDocumentList>> GetDocumentsAsync();
    }
}