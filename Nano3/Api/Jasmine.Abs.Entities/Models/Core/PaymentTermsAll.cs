using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
[Table("PaymentTermsAll")]
    public partial class PaymentTermsAll
    {
        [Key]
        [Column("Account_Code")]
        [StringLength(15)]
        public string AccountCode { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        [Column("Payment_Method")]
        [StringLength(50)]
        public string PaymentMethod { get; set; }
        [Required]
        [Column("Payment_Terms_Group_Code")]
        [StringLength(50)]
        public string PaymentTermsGroupCode { get; set; }
    }
}
