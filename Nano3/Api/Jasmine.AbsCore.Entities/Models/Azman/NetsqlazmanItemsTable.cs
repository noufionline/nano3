using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanItemsTable:TrackableEntityBase, IEntity
    {
        public NetsqlazmanItemsTable()
        {
            NetsqlazmanAuthorizationsTables = new HashSet<NetsqlazmanAuthorizationsTable>();
            NetsqlazmanItemAttributesTables = new HashSet<NetsqlazmanItemAttributesTable>();
        }
        [Column("ItemId")]
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte ItemType { get; set; }
        public int? BizRuleId { get; set; }

        public NetsqlazmanApplicationsTable Application { get; set; }
        public NetsqlazmanBizRulesTable BizRule { get; set; }
        public ICollection<NetsqlazmanAuthorizationsTable> NetsqlazmanAuthorizationsTables { get; set; }
        public ICollection<NetsqlazmanItemAttributesTable> NetsqlazmanItemAttributesTables { get; set; }
    }
}
