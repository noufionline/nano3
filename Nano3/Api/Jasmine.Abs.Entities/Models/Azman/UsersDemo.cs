using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class UsersDemo : TrackableEntityBase
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public string FullName { get; set; }
        public string OtherFields { get; set; }
    }
}
