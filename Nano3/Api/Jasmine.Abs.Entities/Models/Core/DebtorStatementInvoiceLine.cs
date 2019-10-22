using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class DebtorStatementInvoiceLine : TrackableEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; }
        [Column(TypeName = "date")]
        public DateTime InvoiceDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [StringLength(20)]
        public string InvoiceStatus { get; set; }
        [Required]
        [StringLength(50)]
        public string AccountCode { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public int DebtorStatementId { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("DebtorStatementId")]
        [InverseProperty("DebtorStatementInvoiceLines")]
        public virtual DebtorStatement DebtorStatement { get; set; }
    }
}
