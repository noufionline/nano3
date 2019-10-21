using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class Approver
    {
        public Approver()
        {
            Quotations = new HashSet<Quotation>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
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

        [InverseProperty("ApprovedBy")]
        public virtual ICollection<Quotation> Quotations { get; set; }
    }
}
