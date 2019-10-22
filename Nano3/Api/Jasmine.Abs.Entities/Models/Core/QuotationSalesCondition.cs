using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class QuotationSalesCondition : TrackableEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool? AllowEdit { get; set; }
        [Required]
        [StringLength(500)]
        public string Condition { get; set; }
        public int QuotationId { get; set; }
        [StringLength(500)]
        public string ViewName { get; set; }
        public int? SalesConditionId { get; set; }
        public byte[] RowVersion { get; set; }
        public int SlNo { get; set; }

        [ForeignKey("QuotationId")]
        [InverseProperty("QuotationSalesConditions")]
        public virtual Quotation Quotation { get; set; }
    }
}
