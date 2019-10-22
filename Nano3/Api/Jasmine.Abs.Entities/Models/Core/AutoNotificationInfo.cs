using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
[Table("AutoNotificationInfo")]
    public partial class AutoNotificationInfo : TrackableEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public int Interval { get; set; }
        [Required]
        [StringLength(500)]
        public string CommandText { get; set; }
        public string Parameters { get; set; }
        [StringLength(250)]
        public string CollectionViewName { get; set; }
        public string Filter { get; set; }
        [Required]
        public bool? AutoDismiss { get; set; }
    }
}
