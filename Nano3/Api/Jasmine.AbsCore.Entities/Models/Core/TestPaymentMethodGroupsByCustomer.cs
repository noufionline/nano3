using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
[Table("TestPaymentMethodGroupsByCustomer")]
    public partial class TestPaymentMethodGroupsByCustomer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(15)]
        public string CustomerCode { get; set; }
        public int PartnerId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string CustomerName { get; set; }
        [StringLength(15)]
        public string PaymentMethodGroupCode { get; set; }
        [Required]
        [StringLength(15)]
        public string TaxRegistrationNo { get; set; }
        [StringLength(50)]
        public string TaxIdentificationCode { get; set; }
    }
}
