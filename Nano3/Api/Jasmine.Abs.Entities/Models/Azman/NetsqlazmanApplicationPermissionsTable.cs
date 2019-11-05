using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanApplicationPermissionsTable : TrackableEntityBase
    {
        public int ApplicationPermissionId { get; set; }
        public int ApplicationId { get; set; }
        public string SqlUserOrRole { get; set; }
        public bool IsSqlRole { get; set; }
        public byte NetSqlAzManFixedServerRole { get; set; }

        public virtual NetsqlazmanApplicationsTable Application { get; set; }
    }
}
