using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanApplicationGroupMembersView : TrackableEntityBase
    {
        public int StoreId { get; set; }
        public int ApplicationId { get; set; }
        public int ApplicationGroupMemberId { get; set; }
        public int ApplicationGroupId { get; set; }
        public string ApplicationGroup { get; set; }
        public string Name { get; set; }
        public byte[] ObjectSid { get; set; }
        public string WhereDefined { get; set; }
        public string MemberType { get; set; }
    }
}
