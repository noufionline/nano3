using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanApplicationsTable:TrackableEntityBase, IEntity
    {
        public NetsqlazmanApplicationsTable()
        {
            NetsqlazmanApplicationAttributesTables = new HashSet<NetsqlazmanApplicationAttributesTable>();
            NetsqlazmanApplicationGroupsTables = new HashSet<NetsqlazmanApplicationGroupsTable>();
            NetsqlazmanApplicationPermissionsTables = new HashSet<NetsqlazmanApplicationPermissionsTable>();
            NetsqlazmanItemsTables = new HashSet<NetsqlazmanItemsTable>();
        }

        [Column("ApplicationId")]
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public NetsqlazmanStoresTable Store { get; set; }
        public ICollection<NetsqlazmanApplicationAttributesTable> NetsqlazmanApplicationAttributesTables { get; set; }
        public ICollection<NetsqlazmanApplicationGroupsTable> NetsqlazmanApplicationGroupsTables { get; set; }
        public ICollection<NetsqlazmanApplicationPermissionsTable> NetsqlazmanApplicationPermissionsTables { get; set; }
        public ICollection<NetsqlazmanItemsTable> NetsqlazmanItemsTables { get; set; }
    }
}
