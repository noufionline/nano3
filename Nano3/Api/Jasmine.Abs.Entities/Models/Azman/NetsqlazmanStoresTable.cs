using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanStoresTable : TrackableEntityBase
    {
        public NetsqlazmanStoresTable()
        {
            NetsqlazmanApplicationsTables = new HashSet<NetsqlazmanApplicationsTable>();
            NetsqlazmanStoreAttributesTables = new HashSet<NetsqlazmanStoreAttributesTable>();
            NetsqlazmanStoreGroupsTables = new HashSet<NetsqlazmanStoreGroupsTable>();
            NetsqlazmanStorePermissionsTables = new HashSet<NetsqlazmanStorePermissionsTable>();
        }

        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<NetsqlazmanApplicationsTable> NetsqlazmanApplicationsTables { get; set; }
        public virtual ICollection<NetsqlazmanStoreAttributesTable> NetsqlazmanStoreAttributesTables { get; set; }
        public virtual ICollection<NetsqlazmanStoreGroupsTable> NetsqlazmanStoreGroupsTables { get; set; }
        public virtual ICollection<NetsqlazmanStorePermissionsTable> NetsqlazmanStorePermissionsTables { get; set; }
    }
}
