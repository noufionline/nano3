using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class AllocatedInvoice
    {
        [Key]
        public int Id { get; set; }
        public int BankDocumentId { get; set; }
        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; }
        [Column(TypeName = "date")]
        public DateTime InvoiceDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal InvoiceAmount { get; set; }
        [Required]
        public byte[] RowVersion { get; set; }

        [ForeignKey("BankDocumentId")]
        [InverseProperty("AllocatedInvoices")]
        public virtual AccountReceivable BankDocument { get; set; }
    }
}
