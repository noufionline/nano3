using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevExpress.Blazor.Server.Data
{
    public interface ILcDocumentService
    {
        Task<List<LcDocumentList>> GetLcDocumentsAsync();
    }
}