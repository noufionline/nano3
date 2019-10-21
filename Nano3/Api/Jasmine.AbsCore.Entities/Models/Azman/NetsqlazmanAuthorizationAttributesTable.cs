using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanAuthorizationAttributesTable:TrackableEntityBase
    {
        public int AuthorizationAttributeId { get; set; }
        public int AuthorizationId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }

        public NetsqlazmanAuthorizationsTable Authorization { get; set; }
    }
}
