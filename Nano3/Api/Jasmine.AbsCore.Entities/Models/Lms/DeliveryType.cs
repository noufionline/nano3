using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class DeliveryType:TrackableEntityBase
    {
        public DeliveryType()
        {
            OutboundDocuments = new HashSet<OutboundDocument>();
        }

        public int DeliveryTypeId { get; set; }
        [Required]
        [Column("DeliveryType")]
        [StringLength(250)]
        public string DeliveryType1 { get; set; }

        [InverseProperty("DeliveryType")]
        public ICollection<OutboundDocument> OutboundDocuments { get; set; }
    }
}
