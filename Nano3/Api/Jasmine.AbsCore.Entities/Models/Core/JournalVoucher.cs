using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class JournalVoucher
    {
        public JournalVoucher()
        {
            JournalVoucherLines = new HashSet<JournalVoucherLine>();
        }

        [Key]
        public int Id { get; set; }
        public int PartnerId { get; set; }
        [Required]
        [StringLength(50)]
        public string VoucherNo { get; set; }
        [Column("JVNo")]
        public int Jvno { get; set; }
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
        [InverseProperty("JournalVouchers")]
        public virtual Partner Partner { get; set; }
        [InverseProperty("Voucher")]
        public virtual ICollection<JournalVoucherLine> JournalVoucherLines { get; set; }
    }
}
