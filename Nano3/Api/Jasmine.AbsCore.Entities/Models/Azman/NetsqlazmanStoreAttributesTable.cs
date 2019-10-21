using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanStoreAttributesTable:TrackableEntityBase
    {
        public int StoreAttributeId { get; set; }
        public int StoreId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }

        public NetsqlazmanStoresTable Store { get; set; }
    }
}
