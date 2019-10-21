using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanStoreGroupsTable
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

        public virtual NetsqlazmanStoresTable Store { get; set; }
        public virtual ICollection<NetsqlazmanStoreGroupMembersTable> NetsqlazmanStoreGroupMembersTables { get; set; }
    }
}
