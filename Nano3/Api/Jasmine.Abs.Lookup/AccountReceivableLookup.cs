using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PostSharp.Patterns.Model;

namespace Jasmine.Abs.Lookup
{
    [NotifyPropertyChanged]
    public class AccountReceivableLookup
    {
        public List<CustomerLookup> Customers { get; set; }
        public List<Lookup> Banks { get; set; }
        public List<Lookup> Companies { get; set; }
        public List<Lookup> AccountReceivableCollectors { get; set; }
        public List<CompanyBanksLookup> CompanyBanks {get;set;}
        public List<Lookup> Projects { get; set; }
        public List<SunAccountLookup> SunAccounts { get; set; }

    }

    [Serializable]
    public class SunAccountLookup
    {
        public int? PartnerId {get;set;}
        public string PartnerName { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string TaxRegistrationCode { get; set; }
    }
}