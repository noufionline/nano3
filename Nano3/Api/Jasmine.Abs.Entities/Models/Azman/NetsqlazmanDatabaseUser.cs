using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanDatabaseUser
    {
        public byte[] DbuserSid { get; set; }
        public string DbuserName { get; set; }
        public string FullName { get; set; }
        public string OtherFields { get; set; }
    }
}
