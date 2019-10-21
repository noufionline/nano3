using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class SalesPerson
    {
        public SalesPerson()
        {
            PartnerGroups = new HashSet<PartnerGroup>();
            Partners = new HashSet<Partner>();
            Quotations = new HashSet<Quotation>();
            SalesPersonsByDivisions = new HashSet<SalesPersonsByDivision>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        [StringLength(10)]
        public string Abbr { get; set; }
        [StringLength(150)]
        public string Designation { get; set; }
        [Column(TypeName = "image")]
        public byte[] Signature { get; set; }
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

        [InverseProperty("SalesPerson")]
        public virtual ICollection<PartnerGroup> PartnerGroups { get; set; }
        [InverseProperty("SalesPerson")]
        public virtual ICollection<Partner> Partners { get; set; }
        [InverseProperty("SalesPerson")]
        public virtual ICollection<Quotation> Quotations { get; set; }
        [InverseProperty("SalesPerson")]
        public virtual ICollection<SalesPersonsByDivision> SalesPersonsByDivisions { get; set; }
    }
}
