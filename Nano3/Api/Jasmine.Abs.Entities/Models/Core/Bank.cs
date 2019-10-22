using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class Bank : TrackableEntityBase
    {
        public Bank()
        {
            AccountReceivables = new HashSet<AccountReceivable>();
            PartnerBankers = new HashSet<PartnerBanker>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
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

        [InverseProperty("Bank")]
        public virtual ICollection<AccountReceivable> AccountReceivables { get; set; }
        [InverseProperty("Bank")]
        public virtual ICollection<PartnerBanker> PartnerBankers { get; set; }
    }
}
