using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class JournalVoucherReceiptLine
    {
        [Key]
        public int Id { get; set; }
        public int VoucherId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Date { get; set; }
        [Required]
        [StringLength(15)]
        public string AccountNo { get; set; }
        [Required]
        [StringLength(150)]
        public string Description { get; set; }
        [Required]
        [StringLength(3)]
        public string TransactionType { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Debit { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Credit { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("VoucherId")]
        [InverseProperty("JournalVoucherReceiptLines")]
        public virtual JournalVoucherReceipt Voucher { get; set; }
    }
}
