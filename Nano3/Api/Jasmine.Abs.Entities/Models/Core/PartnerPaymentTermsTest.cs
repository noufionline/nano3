using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
[Table("PartnerPaymentTermsTest")]
    public partial class PartnerPaymentTermsTest
    {
        [Key]
        public int Id { get; set; }
        public int? PaymentTermId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int PartnerId { get; set; }
        public int Months { get; set; }
        public int Days { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
    }
}
