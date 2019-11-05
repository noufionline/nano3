using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanBizRuleView : TrackableEntityBase
    {
        public int ItemId { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte ItemType { get; set; }
        public string BizRuleSource { get; set; }
        public byte BizRuleLanguage { get; set; }
        public byte[] CompiledAssembly { get; set; }
    }
}
