using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class PartnerGroup
    {
        public PartnerGroup()
        {
            DebtorStatements = new HashSet<DebtorStatement>();
            PartnerGroupMappings = new HashSet<PartnerGroupMapping>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int PartnerGroupTypeId { get; set; }
        [StringLength(50)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string Telephone { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }
        public int? PaymentTermId { get; set; }
        public int? SalesPersonId { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("PartnerGroupTypeId")]
        [InverseProperty("PartnerGroups")]
        public virtual PartnerGroupType PartnerGroupType { get; set; }
        [ForeignKey("PaymentTermId")]
        [InverseProperty("PartnerGroups")]
        public virtual PartnerPaymentTerm PaymentTerm { get; set; }
        [ForeignKey("SalesPersonId")]
        [InverseProperty("PartnerGroups")]
        public virtual SalesPerson SalesPerson { get; set; }
        [InverseProperty("PartnerGroup")]
        public virtual ICollection<DebtorStatement> DebtorStatements { get; set; }
        [InverseProperty("PartnerGroup")]
        public virtual ICollection<PartnerGroupMapping> PartnerGroupMappings { get; set; }
    }
}
