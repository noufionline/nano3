using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class Nationality
    {
        public Nationality()
        {
            OwnerIdentityDocuments = new HashSet<OwnerIdentityDocument>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
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

        [InverseProperty("Nationality")]
        public virtual ICollection<OwnerIdentityDocument> OwnerIdentityDocuments { get; set; }
    }
}
