﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
[Table("PaymentTermsWithPartnerId")]
    public partial class PaymentTermsWithPartnerId : TrackableEntityBase
    {
        [Key]
        [StringLength(15)]
        public string AccountCode { get; set; }
        public int PartnerId { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        [StringLength(50)]
        public string PaymentMethod { get; set; }
        [Required]
        [StringLength(50)]
        public string PaymentTermsGroupCode { get; set; }
        public int Months { get; set; }
        public int Days { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        public int? PaymentTermId { get; set; }
    }
}
