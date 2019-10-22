using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class QuotationContact : TrackableEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        [StringLength(150)]
        public string Mobile { get; set; }
        public int PartnerId { get; set; }
        [StringLength(50)]
        public string Salutation { get; set; }
        [StringLength(150)]
        public string Designation { get; set; }
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

        [ForeignKey("PartnerId")]
        [InverseProperty("QuotationContacts")]
        public virtual Partner Partner { get; set; }
    }
}
