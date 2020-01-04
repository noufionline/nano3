using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevExpress.Blazor.Server.Data
{
    public class LcDocumentService : ILcDocumentService
    {
        private readonly AbsCoreContext _context;

        public LcDocumentService(AbsCoreContext context)
        {
            _context = context;
        }

        public Task<List<LcDocumentList>> GetLcDocumentsAsync()
        {
            return _context.LcDocuments.ToListAsync();
        }
    }

}
