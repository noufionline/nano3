using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class BankDepositSlip : TrackableEntityBase
    {
        public BankDepositSlip()
        {
            BankDocumentTransactionHistories = new HashSet<BankDocumentTransactionHistory>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string SlipNo { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime Date { get; set; }
        [Required]
        [StringLength(50)]
        public string Depositor { get; set; }
        [StringLength(50)]
        public string Telephone { get; set; }
        public int? CompanyId { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("CompanyId")]
        [InverseProperty("BankDepositSlips")]
        public virtual Company Company { get; set; }
        [InverseProperty("DepositSlip")]
        public virtual ICollection<BankDocumentTransactionHistory> BankDocumentTransactionHistories { get; set; }
    }
}
