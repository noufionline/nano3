using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanStoreGroupsTable:TrackableEntityBase
    {
        public NetsqlazmanStoreGroupsTable()
        {
            NetsqlazmanStoreGroupMembersTables = new HashSet<NetsqlazmanStoreGroupMembersTable>();
        }

        public int StoreGroupId { get; set; }
        public int StoreId { get; set; }
        public byte[] ObjectSid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LdapQuery { get; set; }
        public byte GroupType { get; set; }

        public NetsqlazmanStoresTable Store { get; set; }
        public ICollection<NetsqlazmanStoreGroupMembersTable> NetsqlazmanStoreGroupMembersTables { get; set; }
    }
}
