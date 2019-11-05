using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanAuthorizationsTable : TrackableEntityBase
    {
        public NetsqlazmanAuthorizationsTable()
        {
            NetsqlazmanAuthorizationAttributesTables = new HashSet<NetsqlazmanAuthorizationAttributesTable>();
        }

        public int AuthorizationId { get; set; }
        public int ItemId { get; set; }
        public byte[] OwnerSid { get; set; }
        public byte OwnerSidWhereDefined { get; set; }
        public byte[] ObjectSid { get; set; }
        public byte ObjectSidWhereDefined { get; set; }
        public byte AuthorizationType { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual NetsqlazmanItemsTable Item { get; set; }
        public virtual ICollection<NetsqlazmanAuthorizationAttributesTable> NetsqlazmanAuthorizationAttributesTables { get; set; }
    }
}
