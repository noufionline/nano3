using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class Log : TrackableEntityBase
    {
        public int Id { get; set; }
        public string Application { get; set; }
        public DateTime Logged { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
    }
}
