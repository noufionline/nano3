using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class PartnerTradeReference
    {
        [Key]
        public int Id { get; set; }
        public int? PartnerId { get; set; }
        public int TradeReferenceTypeId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        [StringLength(50)]
        public string ContactNo { get; set; }
        [StringLength(500)]
        public string Comments { get; set; }
        [StringLength(250)]
        public string Period { get; set; }
        [StringLength(250)]
        public string PaymentTerms { get; set; }
        [StringLength(250)]
        public string PaymentStatus { get; set; }
        [StringLength(500)]
        public string ProductsPurchased { get; set; }
        [StringLength(250)]
        public string PersonInterviewed { get; set; }
        [StringLength(150)]
        public string Designation { get; set; }
        public int Rating { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("PartnerId")]
        [InverseProperty("PartnerTradeReferences")]
        public virtual Partner Partner { get; set; }
        [ForeignKey("TradeReferenceTypeId")]
        [InverseProperty("PartnerTradeReferences")]
        public virtual TradeReferenceType TradeReferenceType { get; set; }
    }
}
