using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Sms
{
    public partial class UserSessions
    {
        public UserSessions()
        {
            ReportRepository = new HashSet<ReportRepository>();
        }

        public int SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime RegDate { get; set; }

        public virtual ICollection<ReportRepository> ReportRepository { get; set; }
    }
}
