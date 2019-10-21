using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class Quotation
    {
        public Quotation()
        {
            QuotationAttachments = new HashSet<QuotationAttachment>();
            QuotationLines = new HashSet<QuotationLine>();
            QuotationSalesConditions = new HashSet<QuotationSalesCondition>();
            QuotationStateLogs = new HashSet<QuotationStateLog>();
            SalesOrderQuotations = new HashSet<SalesOrderQuotation>();
            SalesOrders = new HashSet<SalesOrder>();
        }

        [Key]
        public int Id { get; set; }
        public int DivisionId { get; set; }
        public int QuotationNo { get; set; }
        public int RevisionNo { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime QuotationDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? EnquiryDate { get; set; }
        [StringLength(250)]
        public string EnquiryRefNo { get; set; }
        [StringLength(150)]
        public string RemarksForFollowup { get; set; }
        public int PartnerId { get; set; }
        [Required]
        [StringLength(2000)]
        public string JobSite { get; set; }
        public int SalesPersonId { get; set; }
        public int PaymentTermId { get; set; }
        public int PaymentMethodId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime OfferValidity { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? PriceValidity { get; set; }
        public int DeliveryPointId { get; set; }
        [StringLength(250)]
        public string DeliveryLocation { get; set; }
        [Required]
        [StringLength(50)]
        public string QuotationHeader { get; set; }
        [Required]
        [StringLength(250)]
        public string Attention { get; set; }
        [Required]
        [StringLength(250)]
        public string Subject { get; set; }
        [Required]
        public string Opening { get; set; }
        public string Body { get; set; }
        public string AdditionalNotes { get; set; }
        public bool SuppressLineItems { get; set; }
        public bool Canceled { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ExpectedGrossMargin { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal Discount { get; set; }
        public int QuotationState { get; set; }
        public int ApprovedById { get; set; }
        public bool HideSalesTerms { get; set; }
        public bool HideSalesConditions { get; set; }
        [StringLength(500)]
        public string ApprovedOrigins { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? NewBbsCharge { get; set; }
        [Required]
        [StringLength(10)]
        public string BbsChargeType { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? BbsRevisionCharge { get; set; }
        public bool IncludedInCbCharges { get; set; }
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
        [StringLength(50)]
        public string EnquiryType { get; set; }
        [StringLength(150)]
        public string EnquiryContact { get; set; }
        public string Emails { get; set; }
        [StringLength(50)]
        public string BbsBy { get; set; }
        public bool BbsFreeOfCharge { get; set; }
        public int? ChargingTypeId { get; set; }
        public int? ScrapLimit { get; set; }
        public string ProductSpecifications { get; set; }
        [Required]
        [StringLength(50)]
        public string UniqueQuotationNo { get; set; }
        public int? TechnicalContactId { get; set; }
        public string EnquiryRemarks { get; set; }
        public string Remarks { get; set; }
        public bool IsOverseasEnquiry { get; set; }
        public bool EndOfProject { get; set; }
        public int? CurrencyId { get; set; }

        [ForeignKey("ApprovedById")]
        [InverseProperty("Quotations")]
        public virtual Approver ApprovedBy { get; set; }
        [ForeignKey("CurrencyId")]
        [InverseProperty("Quotations")]
        public virtual Currency Currency { get; set; }
        [ForeignKey("DeliveryPointId")]
        [InverseProperty("Quotations")]
        public virtual DeliveryPoint DeliveryPoint { get; set; }
        [ForeignKey("DivisionId")]
        [InverseProperty("Quotations")]
        public virtual Division Division { get; set; }
        [ForeignKey("PartnerId")]
        [InverseProperty("Quotations")]
        public virtual Partner Partner { get; set; }
        [ForeignKey("PaymentMethodId")]
        [InverseProperty("Quotations")]
        public virtual PaymentMethod PaymentMethod { get; set; }
        [ForeignKey("PaymentTermId")]
        [InverseProperty("Quotations")]
        public virtual PaymentTerm PaymentTerm { get; set; }
        [ForeignKey("SalesPersonId")]
        [InverseProperty("Quotations")]
        public virtual SalesPerson SalesPerson { get; set; }
        [ForeignKey("TechnicalContactId")]
        [InverseProperty("Quotations")]
        public virtual TechnicalContact TechnicalContact { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<QuotationAttachment> QuotationAttachments { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<QuotationLine> QuotationLines { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<QuotationSalesCondition> QuotationSalesConditions { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<QuotationStateLog> QuotationStateLogs { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<SalesOrderQuotation> SalesOrderQuotations { get; set; }
        [InverseProperty("Quotation")]
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}
