using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class LcDocument
    {
        public LcDocument()
        {
            CommercialInvoices = new HashSet<CommercialInvoice>();
            LcDocumentRevisions = new HashSet<LcDocumentRevision>();
        }

        [Key]
        public int Id { get; set; }
        public int PartnerId { get; set; }
        [StringLength(15)]
        public string SunAccountCode { get; set; }
        [Required]
        [StringLength(250)]
        public string ClientBankName { get; set; }
        [Required]
        [StringLength(50)]
        public string ClientLcNo { get; set; }
        public int CompanyId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime OpeningDate { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("CompanyId")]
        [InverseProperty("LcDocuments")]
        public virtual Company Company { get; set; }
        [ForeignKey("PartnerId")]
        [InverseProperty("LcDocuments")]
        public virtual Partner Partner { get; set; }
        [ForeignKey("SunAccountCode")]
        [InverseProperty("LcDocuments")]
        public virtual CustomersFromSunSystem SunAccountCodeNavigation { get; set; }
        [InverseProperty("LcDocument")]
        public virtual ICollection<CommercialInvoice> CommercialInvoices { get; set; }
        [InverseProperty("Document")]
        public virtual ICollection<LcDocumentRevision> LcDocumentRevisions { get; set; }
    }
}
