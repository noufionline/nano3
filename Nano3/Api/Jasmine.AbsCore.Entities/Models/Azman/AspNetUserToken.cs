﻿using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class AspNetUserToken:TrackableEntityBase
    {
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
