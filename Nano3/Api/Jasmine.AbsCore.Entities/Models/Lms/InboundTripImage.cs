using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class InboundTripImage:TrackableEntityBase
    {
        public int Id { get; set; }
        public int InboundTripId { get; set; }
        [Required]
        [StringLength(250)]
        public string ImageName { get; set; }
        [Required]
        [StringLength(500)]
        public string LocalPath { get; set; }
        [StringLength(500)]
        public string ServerPath { get; set; }
        public Guid? FileId { get; set; }
        public bool Missing { get; set; }

        [ForeignKey("InboundTripId")]
        [InverseProperty("InboundTripImages")]
        public InboundTripMaster InboundTrip { get; set; }
    }
}
