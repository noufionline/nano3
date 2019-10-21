using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
[Table("BankDocumentTransactionHistory")]
    public partial class BankDocumentTransactionHistory
    {
        [Key]
        public int Id { get; set; }
        public int BankDocumentId { get; set; }
        public int? DepositSlipId { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ReSubmissionDate { get; set; }
        [StringLength(250)]
        public string Reason { get; set; }
        [Column(TypeName = "text")]
        public string Remarks { get; set; }
        public PaymentStatusTypes PaymentStatusId { get; set; }
        public bool IsReplacedWithCash { get; set; }

        [ForeignKey("BankDocumentId")]
        [InverseProperty("BankDocumentTransactionHistories")]
        public virtual AccountReceivable BankDocument { get; set; }
        [ForeignKey("DepositSlipId")]
        [InverseProperty("BankDocumentTransactionHistories")]
        public virtual BankDepositSlip DepositSlip { get; set; }
    }
}
