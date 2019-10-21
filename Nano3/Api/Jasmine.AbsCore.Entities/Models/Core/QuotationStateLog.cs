using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class QuotationStateLog
    {
        [Key]
        public int Id { get; set; }
        public int QuotationId { get; set; }
        public int QuotationState { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(50)]
        public string ModifiedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("QuotationId")]
        [InverseProperty("QuotationStateLogs")]
        public virtual Quotation Quotation { get; set; }
    }
}
