﻿using System;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public class NetsqlazmanItemsHierarchyView
    {
        public int ItemId { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ItemType { get; set; }
        public int MemberItemId { get; set; }
        public int MemberApplicationId { get; set; }
        public string MemberName { get; set; }
        public string MemberDescription { get; set; }
        public string MemberType { get; set; }
    }
}
