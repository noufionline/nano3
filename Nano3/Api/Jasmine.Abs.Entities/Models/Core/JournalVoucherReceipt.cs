using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class JournalVoucherReceipt
    {
        public JournalVoucherReceipt()
        {
            JournalVoucherReceiptLines = new HashSet<JournalVoucherReceiptLine>();
        }

        [Key]
        public int Id { get; set; }
        public int PartnerId { get; set; }
        [Column("JVRNo")]
        public int Jvrno { get; set; }
        [Required]
        [StringLength(50)]
        public string VoucherNo { get; set; }
        public int Period { get; set; }
        [Required]
        [StringLength(15)]
        public string Branch { get; set; }
        [StringLength(50)]
        public string JournalRef { get; set; }
        [StringLength(250)]
        public string Purpose { get; set; }
        [StringLength(250)]
        public string Reason { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("PartnerId")]
        [InverseProperty("JournalVoucherReceipts")]
        public virtual Partner Partner { get; set; }
        [InverseProperty("Voucher")]
        public virtual ICollection<JournalVoucherReceiptLine> JournalVoucherReceiptLines { get; set; }
    }
}
