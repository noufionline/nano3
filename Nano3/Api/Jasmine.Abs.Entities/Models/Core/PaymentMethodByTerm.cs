using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class PaymentMethodByTerm
    {
        [Key]
        public int PaymentTermId { get; set; }
        [Key]
        public int PaymentMethodId { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("PaymentMethodId")]
        [InverseProperty("PaymentMethodByTerms")]
        public virtual PaymentMethod PaymentMethod { get; set; }
        [ForeignKey("PaymentTermId")]
        [InverseProperty("PaymentMethodByTerms")]
        public virtual PaymentTerm PaymentTerm { get; set; }
    }
}
