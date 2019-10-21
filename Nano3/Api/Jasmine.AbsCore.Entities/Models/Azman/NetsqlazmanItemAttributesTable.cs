using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanItemAttributesTable:TrackableEntityBase
    {
        public int ItemAttributeId { get; set; }
        public int ItemId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }

        public NetsqlazmanItemsTable Item { get; set; }
    }
}
