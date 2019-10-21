using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PostSharp.Patterns.Model;

namespace Jasmine.Abs.Lookup
{
    [NotifyPropertyChanged]
    [Serializable]
    public class QuotationLookup
    {

        public List<ApplicationSettingLookup> ApplicationSettings { get; set; }

        public List<Lookup> ApprovedOrigins { get; set; }

        public List<Lookup> Approvers { get; set; }
        public List<Lookup> Currencies { get; set; }
        public List<SalesPersonByDivisionLookup> SalesPersons { get; set; }

        public List<CustomerLookup> Customers { get; set; }
        public List<DeliveryPointLookup> DeliveryPointList { get; set; }

        public List<PaymentTermsDetailLookup> PaymentTerms { get; set; }

        public List<ProductForQuotationLookup> Products { get; set; }

        public List<SalesConditionLookup> SalesConditions { get; set; }

        public List<Lookup> TechnicalContacts { get; set; }

        public ObservableCollection<Lookup> QuotationEmails { get; set; } = new ObservableCollection<Lookup>();


        public List<SalesPersonByDivisionLookup> SalesPersonsByDivisions { get; set; }

        public List<SalesPersonByDivisionLookup> GetSalesPersons(int divisionId)
        {
            if (SalesPersonsByDivisions != null)
            {
                return SalesPersonsByDivisions.Where(x => x.DivisionId == divisionId).ToList();
            }
            return new List<SalesPersonByDivisionLookup>();
        }
    }
    [Serializable]
    public class PartnerRatingLookup : Lookup
    {

        public string Description { get; set; }
        public bool AllowCashTransactions { get; set; }
        public bool AllowCreditTransactions { get; set; }
    }
    [Serializable]
    public class AttachmentTypeLookup : Lookup
    {
        public bool HasExpiry { get; set; } = false;
    }
    [Serializable]
    public class SalesConditionLookup : IEntity
    {
        public bool? AllowEdit { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SalesConditionId { get; set; }
        public string ViewName { get; set; }


    }

    [Serializable]
    public class SunDbCustomerLookup : IEquatable<SunDbCustomerLookup>
    {
        public string CustomerCode { get; set; }
        public string ProjectName { get; set; }
        public string GroupName { get; set; }
        public int? GroupId { get; set; }
        public string Name { get; set; }

        public string NameDisplay => GetAddress();

        public string AddressCode { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }

        public string Line4 { get; set; }

        public string Line5 { get; set; }

        public string TownOrCity { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string TelephoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string WebPageAddress { get; set; }

        public string Comment { get; set; }

        public string ShortHeading { get; set; }

        public string LookupCode { get; set; }

        public string Area { get; set; }

        public string CountryCode { get; set; }

        public string StateCode { get; set; }

        public string TaxIdentificationCode { get; set; }


        public string ActualAccount { get; set; }
        public int? PartnerId { get; set; }
        public string PartnerName { get; set; }
        public int? ProjectId { get; set; }

        public int? AccountGroupId { get; set; }

        public string TagName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        //public string Type { get; set; }
        //public string Code { get; set; }
        //public string Analysis { get; set; }
        //public string Name { get; set; }
        //public string NameDisplay => GetAddress();
        //public string Line1 { get; set; }
        //public string Line2 { get; set; }
        //public string Line3 { get; set; }
        //public string Line4 { get; set; }
        //public string Line5 { get; set; }
        //public string Address6 { get; set; }
        //public string Telephone { get; set; }
        //public string Contact { get; set; }
        //public string Telex { get; set; }
        //public string Comment1 { get; set; }
        //public string Comment2 { get; set; }
        //public string Email { get; set; }
        //public string Webpage { get; set; }
        //public string Lookup { get; set; }
        //public string Comments { get; set; }
        //public string CreditLimit { get; set; }
        //public string PaymentDays { get; set; }
        //public string PaymentTerm { get; set; }
        //public string VatCode { get; set; }


        public string GetAddress()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(GetCleanString(Line1)).Append(" ")
                .Append(GetCleanString(Line2)).Append(" ")
                .Append(GetCleanString(Line3)).Append(" ")
                .Append(GetCleanString(Line4)).Append(" ")
                .Append(GetCleanString(Line5)).Append(" ");

            var address = stringBuilder.ToString();

            if (TryGetAddress(address, "PHONE", out var newAddress)) return newAddress;
            if (TryGetAddress(address, "FAX", out newAddress)) return newAddress;
            if (TryGetAddress(address, "P.O.BOX", out newAddress)) return newAddress;
            if (TryGetAddress(address, "P.O. BOX", out newAddress)) return newAddress;
            if (TryGetAddress(address, "TELE", out newAddress)) return newAddress;
            if (TryGetAddress(address, "TEL:", out newAddress)) return newAddress;
            address = Name;

            return address;
        }

        private bool TryGetAddress(string address, string value, out string newAddress)
        {
            var length = address.IndexOf(value, StringComparison.InvariantCulture);
            if (length > 0)
            {
                newAddress = address.Substring(0, length).Trim();
                return true;
            }

            newAddress = string.Empty;
            return false;
        }

        private string GetCleanString(string value)
        {
            if (value == null) return string.Empty;

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            return regex.Replace(value.TrimEnd(), " ");
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SunDbCustomerLookup);
        }

        public bool Equals(SunDbCustomerLookup other)
        {
            return other != null &&
                   CustomerCode == other.CustomerCode;
        }

        public override int GetHashCode()
        {
            return -434485196 + EqualityComparer<string>.Default.GetHashCode(CustomerCode);
        }

        public static bool operator ==(SunDbCustomerLookup customer1, SunDbCustomerLookup customer2)
        {
            return EqualityComparer<SunDbCustomerLookup>.Default.Equals(customer1, customer2);
        }

        public static bool operator !=(SunDbCustomerLookup customer1, SunDbCustomerLookup customer2)
        {
            return !(customer1 == customer2);
        }
    }
    [Serializable]
    public class SalesOrderAttachmentTypeLookup : Lookup
    {
        public bool HasExpiry { get; set; } = false;
    }
    public interface IEntity
    {
        int Id { get; set; }
    }

    [Serializable]
    public class ApplicationSettingLookup
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class PaymentTermsDetailLookup : Lookup
    {
        public int PaymentTermId { get; set; }
        public int? Sequence { get; set; }

    }

    [Serializable]
    public class ProductCategoryUnitLookup : Lookup
    {
        public int ProductCategoryId { get; set; }
    }

    [Serializable]
    public class ProductCategorySalesTermLookup : Lookup
    {
        public int ProductCategoryId { get; set; }
    }

    [Serializable]
    public class DeliveryPointLookup : Lookup
    {
        public bool LocationRequired { get; set; }
        public bool PaidByCicon { get; set; }
    }

    [Serializable]
    public class SalesTermLookup : Lookup
    {
        public string Description { get; set; }
    }



    [Serializable]
    public class ProductForQuotationLookup
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Unit { get; set; }
    }

    [Serializable]
    public class CompanyBanksLookup : Lookup
    {
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string Branch { get; set; }
    }

    [Serializable]
    public class PaymentMethodByPaymentTermLookup : Lookup
    {
        public int PaymentTermId { get; set; }
        public string Type { get; set; }

    }

    [Serializable]
    public class ProductSpecificationLookup
    {
        public string Description { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }

    }



    [Serializable]
    public class SalesPersonByDivisionLookup : Lookup
    {
        public string DivisionAbbr { get; set; }
        public int DivisionId { get; set; }
        public string SalesPersonAbbr { get; set; }


    }

    [Serializable]
    public class PaymentMethodWithTypeLookup : Lookup
    {
        public string Type { get; set; }
    }

    [Serializable]
    public class CustomerLookup : Lookup
    {
        public string TrnNo{get;set;}
        public bool BlackListed { get; set; }
    }

    

    [Serializable]
    public class QuotationContactByPartnerLookup : Lookup
    {
        public string Email { get; set; }
        public string Mobile { get; set; }

        public int PartnerId { get; set; }
    }
}