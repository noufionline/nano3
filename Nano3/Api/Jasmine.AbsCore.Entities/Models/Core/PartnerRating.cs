using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class PartnerRating
    {
        public PartnerRating()
        {
            Partners = new HashSet<Partner>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(3)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public bool AllowCashTransactions { get; set; }
        public bool AllowCreditTransactions { get; set; }
        [StringLength(50)]
        public string Color { get; set; }
        public byte[] RowVersion { get; set; }

        [InverseProperty("Rating")]
        public virtual ICollection<Partner> Partners { get; set; }
    }
}
