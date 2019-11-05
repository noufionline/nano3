using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanApplicationsTable : TrackableEntityBase
    {
        public NetsqlazmanApplicationsTable()
        {
            NetsqlazmanApplicationAttributesTables = new HashSet<NetsqlazmanApplicationAttributesTable>();
            NetsqlazmanApplicationGroupsTables = new HashSet<NetsqlazmanApplicationGroupsTable>();
            NetsqlazmanApplicationPermissionsTables = new HashSet<NetsqlazmanApplicationPermissionsTable>();
            NetsqlazmanItemsTables = new HashSet<NetsqlazmanItemsTable>();
        }

        public int ApplicationId { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual NetsqlazmanStoresTable Store { get; set; }
        public virtual ICollection<NetsqlazmanApplicationAttributesTable> NetsqlazmanApplicationAttributesTables { get; set; }
        public virtual ICollection<NetsqlazmanApplicationGroupsTable> NetsqlazmanApplicationGroupsTables { get; set; }
        public virtual ICollection<NetsqlazmanApplicationPermissionsTable> NetsqlazmanApplicationPermissionsTables { get; set; }
        public virtual ICollection<NetsqlazmanItemsTable> NetsqlazmanItemsTables { get; set; }
    }
}
