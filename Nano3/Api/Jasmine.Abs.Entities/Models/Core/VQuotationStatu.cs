using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
[Table("vQuotationStatus")]
    public partial class VQuotationStatu
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Division { get; set; }
        public int QuotationNo { get; set; }
        public int RevisionNo { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime QuotationDate { get; set; }
        [Required]
        [StringLength(500)]
        public string PartnerName { get; set; }
        [Required]
        [StringLength(150)]
        public string QuotationRef { get; set; }
        [Required]
        [StringLength(2000)]
        public string JobSite { get; set; }
        public string Emails { get; set; }
        [Required]
        [StringLength(150)]
        public string SalesPerson { get; set; }
        [Required]
        [StringLength(250)]
        public string PayementTerm { get; set; }
        [Required]
        [StringLength(500)]
        public string PaymentMethod { get; set; }
        [Required]
        [StringLength(150)]
        public string PreparedBy { get; set; }
        public int QuotationState { get; set; }
    }
}
