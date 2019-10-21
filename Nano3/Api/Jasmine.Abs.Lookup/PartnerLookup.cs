using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jasmine.Abs.Lookup
{
    [NotifyPropertyChanged]
    public class PartnerLookup
    {
        public List<SunDbCustomerLookup> Accounts { get; set; } = new List<SunDbCustomerLookup>();

        public List<Lookup> Products { get; set; }
        public List<AttachmentTypeLookup> AttachmentTypes { get; set; }
        public List<Lookup> Nationalities { get; set; }
        public List<Lookup> BusinessTypes { get; set; }
        public List<Lookup> PaymentTerms { get; set; }
        public List<Lookup> Banks { get; set; }
        public List<Lookup> EmiratesOrCountries { get; set; }
        public List<Lookup> CompanyTypes { get; set; }
        public List<Lookup> SalesPersons { get; set; }
        public List<Lookup> IssuancePlaces { get; set; }
        public List<CustomerLookup> Partners { get; set; }
        public List<Lookup> TradeReferenceTypes { get; set; }
        public List<string> ContactTypes { get; set; }
        public List<PartnerRatingLookup> Ratings { get; set; }
        public List<Lookup> Approvers { get; set; }

        public List<ApplicationSettingLookup> ApplicationSettings { get; set; }
    }


    public class PartnerContactInfo
    {
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public int? PaymentTermId { get; set; }
        public string PaymentTerm { get; set; }
    }

     public class AccountInfoWithProject
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string PartnerName { get; set; }
        public string ProjectName { get; set; }
    }

    public class SunAccountContactInfo
    {
        public string CustomerCode { get; set; }
        public string Name { get; set; }

        public string PostalCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Address => GetAddress();


        public string ExtractPostalCode()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(GetCleanString(Line1)).Append(" ")
                .Append(GetCleanString(Line2)).Append(" ")
                .Append(GetCleanString(Line3)).Append(" ")
                .Append(GetCleanString(Line4)).Append(" ")
                .Append(GetCleanString(Line5)).Append(" ");

            var address = stringBuilder.ToString().Replace(" ", "")
            .Replace(".", "").Replace(":", "");

            if (address.ToLower().Contains("pobox"))
            {
                int index = address.ToLower().IndexOf("pobox");
                var pobox = address.Substring(index, address.Length - index);

                if (int.TryParse(new string(pobox
                                     .SkipWhile(x => !char.IsDigit(x))
                                     .TakeWhile(x => char.IsDigit(x))
                                     .ToArray()), out int result))
                {
                    return result.ToString();
                }

            }

            return string.Empty;
        }

        public void CleanPostalCode()
        {
           var address = PostalCode.Replace(" ", "")
          .Replace(".", "").Replace(":", "");

            if (address.ToLower().Contains("pobox"))
            {
                int index = address.ToLower().IndexOf("pobox");
                var pobox = address.Substring(index, address.Length - index);

                if (int.TryParse(new string(pobox
                                     .SkipWhile(x => !char.IsDigit(x))
                                     .TakeWhile(x => char.IsDigit(x))
                                     .ToArray()), out int result))
                {
                    PostalCode=result.ToString();
                }
            }
        }

        public string AddressCode { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }

        public string Line4 { get; set; }

        public string Line5 { get; set; }
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

    }
}

