using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Zeon
{
    public partial class ClientRedirectUri
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string RedirectUri { get; set; }

        public virtual Client Client { get; set; }
    }
}
