using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanApplicationAttributesTable : TrackableEntityBase
    {
        public int ApplicationAttributeId { get; set; }
        public int ApplicationId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }

        public virtual NetsqlazmanApplicationsTable Application { get; set; }
    }
}
