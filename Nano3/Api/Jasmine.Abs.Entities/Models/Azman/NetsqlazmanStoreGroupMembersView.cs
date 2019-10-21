using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanStoreGroupMembersView
    {
        public int StoreGroupMemberId { get; set; }
        public int StoreGroupId { get; set; }
        public string StoreGroup { get; set; }
        public string Name { get; set; }
        public byte[] ObjectSid { get; set; }
        public string WhereDefined { get; set; }
        public string MemberType { get; set; }
    }
}
