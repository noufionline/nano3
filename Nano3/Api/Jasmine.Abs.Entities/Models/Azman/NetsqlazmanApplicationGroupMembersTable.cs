using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanApplicationGroupMembersTable : TrackableEntityBase
    {
        public int ApplicationGroupMemberId { get; set; }
        public int ApplicationGroupId { get; set; }
        public byte[] ObjectSid { get; set; }
        public byte WhereDefined { get; set; }
        public bool IsMember { get; set; }

        public virtual NetsqlazmanApplicationGroupsTable ApplicationGroup { get; set; }
    }
}
