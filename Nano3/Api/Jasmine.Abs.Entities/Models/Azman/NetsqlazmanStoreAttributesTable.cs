using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanStoreAttributesTable : TrackableEntityBase
    {
        public int StoreAttributeId { get; set; }
        public int StoreId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }

        public virtual NetsqlazmanStoresTable Store { get; set; }
    }
}
