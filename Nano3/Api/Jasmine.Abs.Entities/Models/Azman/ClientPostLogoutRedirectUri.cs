using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class ClientPostLogoutRedirectUri : TrackableEntityBase
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string PostLogoutRedirectUri { get; set; }

        public virtual Client Client { get; set; }
    }
}
