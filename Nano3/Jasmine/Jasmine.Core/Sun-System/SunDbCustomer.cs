using Dapper.FluentMap.Mapping;
using Jasmine.Core.Odoo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Jasmine.Core
{
    //public class SunDbCustomerLookup : IEquatable<SunDbCustomerLookup>
    //{
    //    public string CustomerCode { get; set; }
    //    public string ProjectName { get; set; }
    //    public string GroupName { get; set; }

    //    public string Name { get; set; }

    //    public string NameDisplay => GetAddress();

    //    public string AddressCode { get; set; }

    //    public string Line1 { get; set; }

    //    public string Line2 { get; set; }

    //    public string Line3 { get; set; }

    //    public string Line4 { get; set; }

    //    public string Line5 { get; set; }

    //    public string TownOrCity { get; set; }

    //    public string State { get; set; }

    //    public string PostalCode { get; set; }

    //    public string Country { get; set; }

    //    public string TelephoneNumber { get; set; }

    //    public string FaxNumber { get; set; }

    //    public string WebPageAddress { get; set; }

    //    public string Comment { get; set; }

    //    public string ShortHeading { get; set; }

    //    public string LookupCode { get; set; }

    //    public string Area { get; set; }

    //    public string CountryCode { get; set; }

    //    public string StateCode { get; set; }

    //    public string TaxIdentificationCode { get; set; }


    //    public string ActualAccount { get; set; }
    //    public int? PartnerId { get; set; }
    //    public int? ProjectId { get; set; }
    //    public int? AccountGroupId { get; set; }

    //    //public string Type { get; set; }
    //    //public string Code { get; set; }
    //    //public string Analysis { get; set; }
    //    //public string Name { get; set; }
    //    //public string NameDisplay => GetAddress();
    //    //public string Line1 { get; set; }
    //    //public string Line2 { get; set; }
    //    //public string Line3 { get; set; }
    //    //public string Line4 { get; set; }
    //    //public string Line5 { get; set; }
    //    //public string Address6 { get; set; }
    //    //public string Telephone { get; set; }
    //    //public string Contact { get; set; }
    //    //public string Telex { get; set; }
    //    //public string Comment1 { get; set; }
    //    //public string Comment2 { get; set; }
    //    //public string Email { get; set; }
    //    //public string Webpage { get; set; }
    //    //public string Lookup { get; set; }
    //    //public string Comments { get; set; }
    //    //public string CreditLimit { get; set; }
    //    //public string PaymentDays { get; set; }
    //    //public string PaymentTerm { get; set; }
    //    //public string VatCode { get; set; }


    //    public string GetAddress()
    //    {
    //        var stringBuilder = new StringBuilder();
    //        stringBuilder.Append(GetCleanString(Line1)).Append(" ")
    //            .Append(GetCleanString(Line2)).Append(" ")
    //            .Append(GetCleanString(Line3)).Append(" ")
    //            .Append(GetCleanString(Line4)).Append(" ")
    //            .Append(GetCleanString(Line5)).Append(" ");

    //        var address = stringBuilder.ToString();

    //        if (TryGetAddress(address, "PHONE", out var newAddress)) return newAddress;
    //        if (TryGetAddress(address, "FAX", out newAddress)) return newAddress;
    //        if (TryGetAddress(address, "P.O.BOX", out newAddress)) return newAddress;
    //        if (TryGetAddress(address, "P.O. BOX", out newAddress)) return newAddress;
    //        if (TryGetAddress(address, "TELE", out newAddress)) return newAddress;
    //        if (TryGetAddress(address, "TEL:", out newAddress)) return newAddress;
    //        
    //        return address;
    //    }

    //    private bool TryGetAddress(string address,string value, out string newAddress)
    //    {
    //        var length = address.IndexOf(value, StringComparison.InvariantCulture);
    //        if (length > 0)
    //        {
    //            newAddress = address.Substring(0, length).Trim();
    //            return true;
    //        }

    //        newAddress = string.Empty;
    //        return false;
    //    }

    //    private string GetCleanString(string value)
    //    {
    //        if (value == null) return string.Empty;
    //        
    //        RegexOptions options = RegexOptions.None;
    //        Regex regex = new Regex("[ ]{2,}", options);     
    //        return regex.Replace(value.TrimEnd(), " ");
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        return Equals(obj as SunDbCustomerLookup);
    //    }

    //    public bool Equals(SunDbCustomerLookup other)
    //    {
    //        return other != null &&
    //               CustomerCode == other.CustomerCode;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return -434485196 + EqualityComparer<string>.Default.GetHashCode(CustomerCode);
    //    }

    //    public static bool operator ==(SunDbCustomerLookup customer1, SunDbCustomerLookup customer2)
    //    {
    //        return EqualityComparer<SunDbCustomerLookup>.Default.Equals(customer1, customer2);
    //    }

    //    public static bool operator !=(SunDbCustomerLookup customer1, SunDbCustomerLookup customer2)
    //    {
    //        return !(customer1 == customer2);
    //    }
    //}

    public class SunDbCustomerInfoMap : EntityMap<SunAccountInfo>
    {
        public SunDbCustomerInfoMap()
        {
            Map(u => u.AccountCode).ToColumn("ACCNT_CODE");
            Map(u => u.AccountName).ToColumn("ACCNT_NAME");
        }
    }
}