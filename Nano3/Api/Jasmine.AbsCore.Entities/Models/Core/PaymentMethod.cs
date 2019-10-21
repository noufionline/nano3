using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            PaymentMethodByTerms = new HashSet<PaymentMethodByTerm>();
            Quotations = new HashSet<Quotation>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
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

        [InverseProperty("PaymentMethod")]
        public virtual ICollection<PaymentMethodByTerm> PaymentMethodByTerms { get; set; }
        [InverseProperty("PaymentMethod")]
        public virtual ICollection<Quotation> Quotations { get; set; }
    }
}
