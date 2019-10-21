using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanItemsTable
    {
        public NetsqlazmanItemsTable()
        {
            NetsqlazmanAuthorizationsTables = new HashSet<NetsqlazmanAuthorizationsTable>();
            NetsqlazmanItemAttributesTables = new HashSet<NetsqlazmanItemAttributesTable>();
        }

        public int ItemId { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte ItemType { get; set; }
        public int? BizRuleId { get; set; }

        public virtual NetsqlazmanApplicationsTable Application { get; set; }
        public virtual NetsqlazmanBizRulesTable BizRule { get; set; }
        public virtual ICollection<NetsqlazmanAuthorizationsTable> NetsqlazmanAuthorizationsTables { get; set; }
        public virtual ICollection<NetsqlazmanItemAttributesTable> NetsqlazmanItemAttributesTables { get; set; }
    }
}
