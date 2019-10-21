using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class DebtorStatement
    {
        public DebtorStatement()
        {
            DebtorStatementChequesInHandLines = new HashSet<DebtorStatementChequesInHandLine>();
            DebtorStatementInvoiceLines = new HashSet<DebtorStatementInvoiceLine>();
        }

        [Key]
        public int Id { get; set; }
        public int? PartnerId { get; set; }
        public int? PartnerGroupId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime StatementDate { get; set; }
        [StringLength(8)]
        public string AccountingPeriod { get; set; }
        public int? AccountGroupId { get; set; }
        [Required]
        public string SunAccounts { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        public byte[] RowVersion { get; set; }
        public bool Active { get; set; }

        [ForeignKey("PartnerId")]
        [InverseProperty("DebtorStatements")]
        public virtual Partner Partner { get; set; }
        [ForeignKey("PartnerGroupId")]
        [InverseProperty("DebtorStatements")]
        public virtual PartnerGroup PartnerGroup { get; set; }
        [InverseProperty("DebtorStatement")]
        public virtual ICollection<DebtorStatementChequesInHandLine> DebtorStatementChequesInHandLines { get; set; }
        [InverseProperty("DebtorStatement")]
        public virtual ICollection<DebtorStatementInvoiceLine> DebtorStatementInvoiceLines { get; set; }
    }
}
