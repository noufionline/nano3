using System;
using System.Collections.Generic;
using Jasmine.Core.Tracking;

namespace Jasmine.Core.Audit
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int PrimaryKey { get; set; }
        public string EntityName { get; set; }
        public string Before { get; set; }
        public string Changes { get; set; }
        public string After { get; set; }
        public TrackingState TrackingState { get; set; }
        public string RegUser { get; set; }
        public DateTime RegDate { get; set; }

        public List<AuditLogLine> Differences { get; set; } = new List<AuditLogLine>();
      
    }
}