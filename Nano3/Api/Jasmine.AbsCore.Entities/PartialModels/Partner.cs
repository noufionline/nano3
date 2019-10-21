// ReSharper disable once CheckNamespace

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class Partner : ILookupItemModel, IAuditable
    {

    }

    public partial class LcDocument : ILookupItemModel, IAuditable
    {

    }

    public partial class Quotation : ILookupItemModel, IAuditable
    {

    }

    public partial class Currency : ILookupItemModel, IAuditable
    {

    }


    public partial class OwnerIdentityDocument : IAuditable, IEntity, IConcurrency
    { }

    public partial class SalesOrder : ILookupItemModel, IAuditable
    { }


    public partial class AuditLog : IEntity
    { }

    public partial class Bank : ILookupItemModel, IAuditable
    {

    }

    public partial class AccountReceivableCollector : ILookupItemModel, IAuditable
    {

    }

    public partial class Approver : ILookupItemModel, IAuditable
    {

    }


    public partial class Division : ILookupItemModel, IAuditable { }

    public partial class ProductUnit : ILookupItemModel, IAuditable
    {

    }

    public partial class ProductCategory : ILookupItemModel, IAuditable
    {

    }
    public partial class PartnerProduct : ILookupItemModel, IAuditable
    {

    }

    public partial class PartnerPaymentTerm : ILookupItemModel, IAuditable
    {

    }

    public partial class BusinessType : ILookupItemModel, IAuditable { }

    public partial class AttachmentType : ILookupItemModel, IAuditable { }
    public partial class SalesOrderAttachmentType : ILookupItemModel, IAuditable { }
    public partial class QuotationAttachmentType : ILookupItemModel, IAuditable { }
    public partial class TaskAttachmentType : ILookupItemModel, IAuditable { }
    public partial class BankDocumentAttachmentType : ILookupItemModel, IAuditable { }
    public partial class CommercialInvoiceAttachmentType : ILookupItemModel, IAuditable { }

    public partial class ApprovedOriginsForQuotation : ILookupItemModel, IAuditable { }


    public partial class DeliveryPoint : ILookupItemModel, IAuditable { }
    public partial class PaymentMethod : ILookupItemModel, IAuditable { }

    public partial class IssuancePlace : ILookupItemModel, IAuditable { }

    public partial class PartnerProject : ILookupItem, IConcurrency { }

    public partial class Nationality : ILookupItemModel, IAuditable
    {

    }

    public partial class PartnerContact : ILookupItem, IConcurrency
    {

    }

    public partial class PartnerLegalFormType : ILookupItemModel, IAuditable { }

    public partial class PartnerTradeReference : ILookupItem, IConcurrency { }



    public partial class EmiratesOrCountry : ILookupItemModel, IAuditable
    {

    }

    public partial class PaymentTerm : ILookupItemModel, IAuditable
    { }


    public partial class SalesPerson : ILookupItemModel, IAuditable { }
    public partial class TechnicalContact : ILookupItemModel, IAuditable { }
    public partial class QuotationContact : ILookupItemModel, IAuditable { }

    public partial class SalesCondition : ILookupItemModel, IAuditable { }

    public partial class TradeReferenceType : ILookupItemModel, IAuditable { }

    public partial class AccountReceivable : IAuditable, IEntity
    {

    }

    public partial class BankDepositSlip : IEntity
    {

    }

    public partial class PaymentReceiptVoucher : IAuditable, IEntity
    {

    }

    public partial class JournalVoucherReceipt : IAuditable, IEntity
    {

    }
    public partial class JournalVoucher : IAuditable, IEntity
    {

    }

    public partial class PaymentReceiver : ILookupItemModel, IAuditable
    {

    }

    public partial class DebtorStatement : IEntity, IAuditable
    {

    }

    public partial class PartnerGroup : ILookupItemModel, IAuditable
    {

    }

    public partial class PartnerGroupType : ILookupItemModel, IAuditable
    {

    }

    public partial class UserTask : IEntity, IAuditable
    {

    }

    public partial class Log : IEntity, IAuditable
    {

    }

    public partial class CommercialInvoice:IEntity,IAuditable{}
}