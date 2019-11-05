using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanAuthorizationView : TrackableEntityBase
    {
        public int AuthorizationId { get; set; }
        public int ItemId { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public byte[] ObjectSid { get; set; }
        public string SidWhereDefined { get; set; }
        public string AuthorizationType { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
