using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class SalesOrderQuotation
    {
        [Key]
        public int SalesOrderId { get; set; }
        [Key]
        public int QuotationId { get; set; }

        [ForeignKey("QuotationId")]
        [InverseProperty("SalesOrderQuotations")]
        public virtual Quotation Quotation { get; set; }
        [ForeignKey("SalesOrderId")]
        [InverseProperty("SalesOrderQuotations")]
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
