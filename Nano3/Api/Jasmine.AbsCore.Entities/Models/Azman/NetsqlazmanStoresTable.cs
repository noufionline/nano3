using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanStoresTable:TrackableEntityBase, IEntity
    {
        public NetsqlazmanStoresTable()
        {
            NetsqlazmanApplicationsTables = new HashSet<NetsqlazmanApplicationsTable>();
            NetsqlazmanStoreAttributesTables = new HashSet<NetsqlazmanStoreAttributesTable>();
            NetsqlazmanStoreGroupsTables = new HashSet<NetsqlazmanStoreGroupsTable>();
            NetsqlazmanStorePermissionsTables = new HashSet<NetsqlazmanStorePermissionsTable>();
        }

        [Column("StoreId")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<NetsqlazmanApplicationsTable> NetsqlazmanApplicationsTables { get; set; }
        public ICollection<NetsqlazmanStoreAttributesTable> NetsqlazmanStoreAttributesTables { get; set; }
        public ICollection<NetsqlazmanStoreGroupsTable> NetsqlazmanStoreGroupsTables { get; set; }
        public ICollection<NetsqlazmanStorePermissionsTable> NetsqlazmanStorePermissionsTables { get; set; }
    }
}
