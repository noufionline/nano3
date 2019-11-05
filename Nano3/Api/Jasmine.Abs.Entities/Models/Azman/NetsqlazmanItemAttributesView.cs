using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanItemAttributesView : TrackableEntityBase
    {
        public int ItemId { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ItemType { get; set; }
        public int ItemAttributeId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }
    }
}
