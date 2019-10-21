using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
[Table("CommercialInvoiceTransactionHistory")]
    public partial class CommercialInvoiceTransactionHistory
    {
        [Key]
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public CommercialInvoiceStatusTypes CommercialInvoiceStatus { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime Date { get; set; }
        [StringLength(250)]
        public string Remarks { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("InvoiceId")]
        [InverseProperty("CommercialInvoiceTransactionHistories")]
        public virtual CommercialInvoice Invoice { get; set; }
    }
}
