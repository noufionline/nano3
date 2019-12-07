using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Sockets;
using System.Threading.Tasks;
using Jasmine.Core.Contracts;
using Jasmine.Core.Odoo;

namespace Jasmine.Core
{
    //public class SunSystemService:ISunSystemService
    //{
    //    private readonly ILocalSunSystemRepository _localRepository;
    //    private readonly IServerSunSystemRepository _repository;
    //  
    //    public SunSystemService(ILocalSunSystemRepository localRepository,IServerSunSystemRepository repository)
    //    {
    //        _localRepository = localRepository;
    //        _repository = repository;
    //    }


    //    public Task<(string AccountName, string Address, string VatCode)?> GetCustomer(string accountCode)
    //    {
    //        if (_repository.IsHeadOffice())
    //        {
    //            return  _localRepository.GetCustomer(accountCode);
    //        }

    //        return _repository.GetCustomer(accountCode);
    //    }

    //    public Task<List<SunDbCustomerLookup>> GetSunAccountsAsync(string property, string value)
    //    {
    //        if (IsHeadOffice())
    //        {
    //            return _localRepository.GetCustomersAsync(property,value);
    //        }

    //        return _repository.GetCustomersAsync(property, value);
    //    }

    //    public  Task<(bool success, string trnNo, string accountName)> ValidateSunAccountCodeAsync(string accountCode, string sunDb = null)
    //    {
    //        if (IsHeadOffice())
    //        {
    //            return  _localRepository.ValidateSunAccountCodeAsync(accountCode, sunDb);
    //        }

    //        return  _repository.ValidateSunAccountCodeAsync(accountCode, sunDb);
    //    }

    //    public Task<IEnumerable<SunAccountInfo>> GetSunAccountInfoAsync(string trnNo)
    //    {
    //        if (IsHeadOffice())
    //        {
    //            return _localRepository.GetSunAccountInfoAsync(trnNo);
    //        }

    //        return _repository.GetSunAccountInfoAsync(trnNo);
    //    }

    //   

    //    public bool IsHeadOffice() => _repository.IsHeadOffice();

    //}
}