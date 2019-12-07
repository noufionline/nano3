using System.Net.Http;
using System.Threading.Tasks;
using Jasmine.Core.Contracts;
using Jasmine.Core.Repositories;

namespace Jasmine.Core.Audit
{
    public class AuditService :RestApiRepositoryBase, IAuditService
    {
        private readonly IHttpClientFactory _factory;

        public AuditService(IHttpClientFactory factory):base(factory)
        {
            _factory = factory;
            
        }
        
        public async Task SaveAuditLogAsync(AuditLog auditLog)
        {
            string request = "audit";
            await PostAsStreamsAsync(request, auditLog);
        }
    }
}