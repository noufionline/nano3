using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class Partner : TrackableEntityBase
    {
        public Partner()
        {
            AccountReceivables = new HashSet<AccountReceivable>();
            CustomersFromSunSystems = new HashSet<CustomersFromSunSystem>();
            DebtorStatements = new HashSet<DebtorStatement>();
            InverseParentCompany = new HashSet<Partner>();
            JournalVoucherReceipts = new HashSet<JournalVoucherReceipt>();
            JournalVouchers = new HashSet<JournalVoucher>();
            LcDocuments = new HashSet<LcDocument>();
            PartnerAttachments = new HashSet<PartnerAttachment>();
            PartnerBankers = new HashSet<PartnerBanker>();
            PartnerContacts = new HashSet<PartnerContact>();
            PartnerGroupMappings = new HashSet<PartnerGroupMapping>();
            PartnerOwners = new HashSet<PartnerOwner>();
            PartnerProjects = new HashSet<PartnerProject>();
            PartnerTradeReferences = new HashSet<PartnerTradeReference>();
            QuotationContacts = new HashSet<QuotationContact>();
            Quotations = new HashSet<Quotation>();
            SalesOrders = new HashSet<SalesOrder>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        [StringLength(50)]
        public string LicenseNo { get; set; }
        [Required]
        [StringLength(15)]
        public string TaxRegistrationNo { get; set; }
        [StringLength(50)]
        public string SunAccountCode { get; set; }
        [Required]
        [StringLength(500)]
        public string LegalTitle { get; set; }
        public int? PartnerTypeId { get; set; }
        public int? BusinessTypeId { get; set; }
        public bool HasBranches { get; set; }
        public int? IssuancePlaceId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? EstablishmentDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? RegistrationDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpiryDate { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PaidUpCapital { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AnticipatedMonthlyBusiness { get; set; }
        [StringLength(150)]
        public string ApprovedBy { get; set; }
        public int? SalesPersonId { get; set; }
        public int? PaymentTermId { get; set; }
        [StringLength(250)]
        public string PaymentTermDisplay { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CreditLimit { get; set; }
        [StringLength(50)]
        public string Telephone { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        [StringLength(50)]
        public string PostalCode { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        public bool BlackListed { get; set; }
        public int? RatingId { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
        public string ProductsRequired { get; set; }
        [StringLength(250)]
        public string CreditApplicationSignedBy { get; set; }
        public int? OdooPartnerId { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsSupplier { get; set; }
        public bool IsActive { get; set; }
        public Guid? AlfrescoNodeId { get; set; }
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
        [StringLength(250)]
        public string SunAccountName { get; set; }
        public string CreditDeptRemarks { get; set; }
        public bool IsTemporary { get; set; }
        public int? ParentCompanyId { get; set; }
        [StringLength(500)]
        public string RatingRemarks { get; set; }

        [ForeignKey("BusinessTypeId")]
        [InverseProperty("Partners")]
        public virtual BusinessType BusinessType { get; set; }
        [ForeignKey("IssuancePlaceId")]
        [InverseProperty("Partners")]
        public virtual IssuancePlace IssuancePlace { get; set; }
        [ForeignKey("ParentCompanyId")]
        [InverseProperty("InverseParentCompany")]
        public virtual Partner ParentCompany { get; set; }
        [ForeignKey("PartnerTypeId")]
        [InverseProperty("Partners")]
        public virtual PartnerLegalFormType PartnerType { get; set; }
        [ForeignKey("PaymentTermId")]
        [InverseProperty("Partners")]
        public virtual PartnerPaymentTerm PaymentTerm { get; set; }
        [ForeignKey("RatingId")]
        [InverseProperty("Partners")]
        public virtual PartnerRating Rating { get; set; }
        [ForeignKey("SalesPersonId")]
        [InverseProperty("Partners")]
        public virtual SalesPerson SalesPerson { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<AccountReceivable> AccountReceivables { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<CustomersFromSunSystem> CustomersFromSunSystems { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<DebtorStatement> DebtorStatements { get; set; }
        [InverseProperty("ParentCompany")]
        public virtual ICollection<Partner> InverseParentCompany { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<JournalVoucherReceipt> JournalVoucherReceipts { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<JournalVoucher> JournalVouchers { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<LcDocument> LcDocuments { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<PartnerAttachment> PartnerAttachments { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<PartnerBanker> PartnerBankers { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<PartnerContact> PartnerContacts { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<PartnerGroupMapping> PartnerGroupMappings { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<PartnerOwner> PartnerOwners { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<PartnerProject> PartnerProjects { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<PartnerTradeReference> PartnerTradeReferences { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<QuotationContact> QuotationContacts { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<Quotation> Quotations { get; set; }
        [InverseProperty("Partner")]
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}
