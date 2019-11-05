using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanStoreAttributesView : TrackableEntityBase
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StoreAttributeId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }
    }
}
