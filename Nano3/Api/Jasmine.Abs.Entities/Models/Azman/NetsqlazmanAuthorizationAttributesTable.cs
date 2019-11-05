using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanAuthorizationAttributesTable : TrackableEntityBase
    {
        public int AuthorizationAttributeId { get; set; }
        public int AuthorizationId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }

        public virtual NetsqlazmanAuthorizationsTable Authorization { get; set; }
    }
}
