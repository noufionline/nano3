using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class ClientRedirectUri:TrackableEntityBase
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string RedirectUri { get; set; }

        public Client Client { get; set; }
    }
}
