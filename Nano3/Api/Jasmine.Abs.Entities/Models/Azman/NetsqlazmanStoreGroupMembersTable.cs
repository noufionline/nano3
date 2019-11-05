using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanStoreGroupMembersTable : TrackableEntityBase
    {
        public int StoreGroupMemberId { get; set; }
        public int StoreGroupId { get; set; }
        public byte[] ObjectSid { get; set; }
        public byte WhereDefined { get; set; }
        public bool IsMember { get; set; }

        public virtual NetsqlazmanStoreGroupsTable StoreGroup { get; set; }
    }
}
