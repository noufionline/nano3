using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class NetsqlazmanItemsHierarchyTable:TrackableEntityBase
    {
        public int ItemId { get; set; }
        public int MemberOfItemId { get; set; }
    }
}
