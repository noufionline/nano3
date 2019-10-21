using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class JournalVoucherLine
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
        [InverseProperty("JournalVoucherLines")]
        public virtual JournalVoucher Voucher { get; set; }
    }
}
