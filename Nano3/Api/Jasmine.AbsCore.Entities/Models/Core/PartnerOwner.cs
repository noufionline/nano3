using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class PartnerOwner
    {
        [Key]
        public int PartnerId { get; set; }
        [Key]
        public int IdentityDocumentId { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("IdentityDocumentId")]
        [InverseProperty("PartnerOwners")]
        public virtual OwnerIdentityDocument IdentityDocument { get; set; }
        [ForeignKey("PartnerId")]
        [InverseProperty("PartnerOwners")]
        public virtual Partner Partner { get; set; }
    }
}
