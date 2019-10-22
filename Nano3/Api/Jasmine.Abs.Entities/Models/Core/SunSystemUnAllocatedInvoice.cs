using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class SunSystemUnAllocatedInvoice : TrackableEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(15)]
        public string AccountType { get; set; }
        [Required]
        [StringLength(15)]
        public string AccountCode { get; set; }
        [Required]
        [StringLength(250)]
        public string AccountName { get; set; }
        [Required]
        [StringLength(50)]
        public string DocumentNo { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime DocumentDate { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [StringLength(8)]
        public string AccountingPeriod { get; set; }
        [StringLength(15)]
        public string JournalSource { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? DueDate { get; set; }
        [StringLength(15)]
        public string PaymentMethod { get; set; }
        [StringLength(15)]
        public string PaymentTermGroupCode { get; set; }
        public int? PaymentTermId { get; set; }

        [ForeignKey("PaymentTermId")]
        [InverseProperty("SunSystemUnAllocatedInvoices")]
        public virtual PartnerPaymentTerm PaymentTerm { get; set; }
    }
}
