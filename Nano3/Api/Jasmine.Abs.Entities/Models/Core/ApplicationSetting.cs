using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class ApplicationSetting : TrackableEntityBase
    {
        [Key]
        [StringLength(250)]
        public string Property { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        [StringLength(10)]
        public string PropertyType { get; set; }
    }
}
