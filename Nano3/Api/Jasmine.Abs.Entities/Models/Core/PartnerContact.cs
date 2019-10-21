using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class PartnerContact
    {
        [Key]
        public int Id { get; set; }
        public int PartnerId { get; set; }
        [StringLength(50)]
        public string ContactType { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(150)]
        public string Designation { get; set; }
        [Column(TypeName = "image")]
        public byte[] Signature { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("PartnerId")]
        [InverseProperty("PartnerContacts")]
        public virtual Partner Partner { get; set; }
    }
}
