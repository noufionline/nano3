using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanSetting : TrackableEntityBase
    {
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }
}
