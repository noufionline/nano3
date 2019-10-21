using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanApplicationGroupsTable:TrackableEntityBase
    {
        public NetsqlazmanApplicationGroupsTable()
        {
            NetsqlazmanApplicationGroupMembersTables = new HashSet<NetsqlazmanApplicationGroupMembersTable>();
        }

        public int ApplicationGroupId { get; set; }
        public int ApplicationId { get; set; }
        public byte[] ObjectSid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LdapQuery { get; set; }
        public byte GroupType { get; set; }

        public NetsqlazmanApplicationsTable Application { get; set; }
        public ICollection<NetsqlazmanApplicationGroupMembersTable> NetsqlazmanApplicationGroupMembersTables { get; set; }
    }
}
