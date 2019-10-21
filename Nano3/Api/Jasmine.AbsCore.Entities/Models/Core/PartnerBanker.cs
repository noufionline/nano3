using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class PartnerBanker
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string AccountNo { get; set; }
        public int BankId { get; set; }
        [StringLength(250)]
        public string BranchLocation { get; set; }
        public int PartnerId { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("BankId")]
        [InverseProperty("PartnerBankers")]
        public virtual Bank Bank { get; set; }
        [ForeignKey("PartnerId")]
        [InverseProperty("PartnerBankers")]
        public virtual Partner Partner { get; set; }
    }
}
