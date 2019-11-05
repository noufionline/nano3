using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanStorePermissionsTable : TrackableEntityBase
    {
        public int StorePermissionId { get; set; }
        public int StoreId { get; set; }
        public string SqlUserOrRole { get; set; }
        public bool IsSqlRole { get; set; }
        public byte NetSqlAzManFixedServerRole { get; set; }

        public virtual NetsqlazmanStoresTable Store { get; set; }
    }
}
