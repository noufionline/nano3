using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class PartnerProject
    {
        public PartnerProject()
        {
            AccountReceivables = new HashSet<AccountReceivable>();
            CustomersFromSunSystems = new HashSet<CustomersFromSunSystem>();
            SunAccountGroups = new HashSet<SunAccountGroup>();
        }

        [Key]
        public int Id { get; set; }
        public int PartnerId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public int? EmirateOrCountryId { get; set; }
        [StringLength(250)]
        public string MainContractor { get; set; }
        [StringLength(250)]
        public string ProjectEmployer { get; set; }
        [StringLength(250)]
        public string ProjectLocation { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? ProjectValue { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("EmirateOrCountryId")]
        [InverseProperty("PartnerProjects")]
        public virtual EmiratesOrCountry EmirateOrCountry { get; set; }
        [ForeignKey("PartnerId")]
        [InverseProperty("PartnerProjects")]
        public virtual Partner Partner { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<AccountReceivable> AccountReceivables { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<CustomersFromSunSystem> CustomersFromSunSystems { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<SunAccountGroup> SunAccountGroups { get; set; }
    }
}
