using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Jasmine.Core.Contracts;
using Jasmine.Core.Odoo;

namespace Jasmine.Core.Repositories
{
    //public class ServerSunSystemRepository:RestApiRepositoryBase,IServerSunSystemRepository
    //{
    //    private readonly IHttpClientFactory _factory;
    //    
    //    public ServerSunSystemRepository(IHttpClientFactory factory):base(factory)
    //    {
    //        _factory = factory;
    //    }


    //    public async Task<(string AccountName, string Address, string VatCode)?> GetCustomer(string accountCode)
    //    {
    //        var request = $"sun-system/customers/{accountCode}";
    //        var customers = await ReadAllAsStreamAsync<SunDbCustomerLookup>(request);
    //        
    //        var customer = customers.FirstOrDefault();
    //        if (customer != null)
    //        {
    //            return (customer.Name, customer.GetAddress(),customer.TaxIdentificationCode);
    //        }

    //        return null;
    //    }

    //    public async Task<List<SunDbCustomerLookup>> GetCustomersAsync(string property, string value)
    //    {
    //       var request = $"sun-system/customers/lookup?property={property}&value={value}";
    //       return await ReadAllAsStreamAsync<SunDbCustomerLookup>(request);
    //    }
    //    //TODO check in postman for the result accuracy
    //    public async Task<(bool success, string trnNo, string accountName)> ValidateSunAccountCodeAsync(string accountCode, string sunDb = null)
    //    {
    //        var request = $"sun-system/vatInfo/{accountCode}";
    //      
    //        var vatInfo = await ReadAsStreamAsync<VatInfo>(request);
    //        if (vatInfo == null)
    //        {
    //            return (false, string.Empty, string.Empty);
    //        }

    //        return (true, vatInfo.VatRegistrationNo, vatInfo.AccountName);
    //    }

    //    public async Task<IEnumerable<SunAccountInfo>> GetSunAccountInfoAsync(string trnNo)
    //    {
    //        var request = $"sun-system/CustomerInfo/{trnNo}";
    //        return await ReadAsStreamAsync<List<SunAccountInfo>>(request);
    //    }

    //    public bool IsHeadOffice()
    //    {
    //        return false;
    //    }
    //}
}