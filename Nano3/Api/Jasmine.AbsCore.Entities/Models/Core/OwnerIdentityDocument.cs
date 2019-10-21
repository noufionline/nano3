using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class OwnerIdentityDocument
    {
        public OwnerIdentityDocument()
        {
            PartnerOwners = new HashSet<PartnerOwner>();
        }

        [Key]
        public int Id { get; set; }
        public int OwnerShipType { get; set; }
        [Required]
        [StringLength(250)]
        public string PassportNo { get; set; }
        public int NationalityId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime PassportExpiryDate { get; set; }
        [Required]
        [StringLength(250)]
        public string NameOnPassport { get; set; }
        [StringLength(500)]
        public string PassportAttachmentPath { get; set; }
        public Guid? PassportAttachmentFileId { get; set; }
        [Required]
        [StringLength(250)]
        public string PassportIssuedPlace { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime PassportIssuedDate { get; set; }
        [StringLength(250)]
        public string EmiratesIdNo { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? EmiratesIdExpiryDate { get; set; }
        [StringLength(500)]
        public string EmiratesIdAttachmentPath { get; set; }
        public Guid? EmiratesIdAttachmentFileId { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string Telephone { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? FollowUpDate { get; set; }
        public string FollowUpNote { get; set; }
        public string FollowUpHistory { get; set; }
        [Column("EIDFollowUpDate", TypeName = "smalldatetime")]
        public DateTime? EidfollowUpDate { get; set; }
        [Column("EIDFollowUpNote")]
        public string EidfollowUpNote { get; set; }
        [Column("EIDFollowUpHistory")]
        public string EidfollowUpHistory { get; set; }
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

        [ForeignKey("NationalityId")]
        [InverseProperty("OwnerIdentityDocuments")]
        public virtual Nationality Nationality { get; set; }
        [InverseProperty("IdentityDocument")]
        public virtual ICollection<PartnerOwner> PartnerOwners { get; set; }
    }
}
