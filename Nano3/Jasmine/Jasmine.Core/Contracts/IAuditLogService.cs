using System.Collections.Generic;
using Jasmine.Core.Audit;
using Newtonsoft.Json;

namespace Jasmine.Core.Contracts
{
    public interface IAuditLogService
    {
        void Compare<T>(T source, T target, List<AuditLogLine> logs);
        AuditLog CreateAuditLog<T>(T source, T changes, T target) where T : class, IEntity, ITrackable;
        Formatting GetFormatting();
    }
}