using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class ClientIdPrestriction:TrackableEntityBase
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Provider { get; set; }

        public Client Client { get; set; }
    }
}
