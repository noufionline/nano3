using System.Threading.Tasks;
using Jasmine.Core.Audit;

namespace Jasmine.Core.Contracts
{
    public interface IAuditService
    {
        Task SaveAuditLogAsync(AuditLog auditLog);
    }
}