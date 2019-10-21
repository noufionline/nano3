using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class LcDocumentRevision
    {
        [Key]
        public int Id { get; set; }
        public int DocumentId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime DeliveryDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime NegotiationDate { get; set; }
        public bool Active { get; set; }
        [StringLength(250)]
        public string Remarks { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("DocumentId")]
        [InverseProperty("LcDocumentRevisions")]
        public virtual LcDocument Document { get; set; }
    }
}
