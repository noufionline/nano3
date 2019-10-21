using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class PartnerPaymentTerm
    {
        public PartnerPaymentTerm()
        {
            PartnerGroups = new HashSet<PartnerGroup>();
            Partners = new HashSet<Partner>();
            SunSystemAllocatedInvoices = new HashSet<SunSystemAllocatedInvoice>();
            SunSystemUnAllocatedInvoices = new HashSet<SunSystemUnAllocatedInvoice>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int Months { get; set; }
        public int Days { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(50)]
        public string PaymentTermsGroupCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        [Required]
        public byte[] RowVersion { get; set; }

        [InverseProperty("PaymentTerm")]
        public virtual ICollection<PartnerGroup> PartnerGroups { get; set; }
        [InverseProperty("PaymentTerm")]
        public virtual ICollection<Partner> Partners { get; set; }
        [InverseProperty("PaymentTerm")]
        public virtual ICollection<SunSystemAllocatedInvoice> SunSystemAllocatedInvoices { get; set; }
        [InverseProperty("PaymentTerm")]
        public virtual ICollection<SunSystemUnAllocatedInvoice> SunSystemUnAllocatedInvoices { get; set; }
    }
}
