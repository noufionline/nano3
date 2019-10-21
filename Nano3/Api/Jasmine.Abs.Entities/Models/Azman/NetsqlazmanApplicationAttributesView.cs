using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanApplicationAttributesView
    {
        public int ApplicationId { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ApplicationAttributeId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }
    }
}
