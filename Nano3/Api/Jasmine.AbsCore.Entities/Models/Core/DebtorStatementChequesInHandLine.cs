using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class DebtorStatementChequesInHandLine
    {
        [Key]
        public int Id { get; set; }
        public int DebtorStatementId { get; set; }
        [Required]
        [StringLength(50)]
        public string SunAccountCode { get; set; }
        [Required]
        [StringLength(50)]
        public string DocumentNo { get; set; }
        public AccountReceivableTypes DocumentType { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime DocumentDate { get; set; }
        public PaymentStatusTypes PaymentStatusId { get; set; }
        public bool LcConfirmed { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [Required]
        [StringLength(50)]
        public string Bank { get; set; }
        [StringLength(250)]
        public string Remarks { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("DebtorStatementId")]
        [InverseProperty("DebtorStatementChequesInHandLines")]
        public virtual DebtorStatement DebtorStatement { get; set; }
    }
}
