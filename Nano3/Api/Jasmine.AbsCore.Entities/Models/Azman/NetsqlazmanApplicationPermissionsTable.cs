using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanApplicationPermissionsTable:TrackableEntityBase
    {
        public int ApplicationPermissionId { get; set; }
        public int ApplicationId { get; set; }
        public string SqlUserOrRole { get; set; }
        public bool IsSqlRole { get; set; }
        public byte NetSqlAzManFixedServerRole { get; set; }

        public NetsqlazmanApplicationsTable Application { get; set; }
    }
}
