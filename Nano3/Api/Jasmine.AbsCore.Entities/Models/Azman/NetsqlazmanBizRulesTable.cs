using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanBizRulesTable:TrackableEntityBase
    {
        public NetsqlazmanBizRulesTable()
        {
            NetsqlazmanItemsTables = new HashSet<NetsqlazmanItemsTable>();
        }

        public int BizRuleId { get; set; }
        public string BizRuleSource { get; set; }
        public byte BizRuleLanguage { get; set; }
        public byte[] CompiledAssembly { get; set; }

        public ICollection<NetsqlazmanItemsTable> NetsqlazmanItemsTables { get; set; }
    }
}
