using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Jasmine.Core.Contracts;
using Jasmine.Core.Exceptions;
using Jasmine.Core.Odoo;

namespace Jasmine.Core.Repositories
{
    //public class LocalSunSystemRepository : ILocalSunSystemRepository
    //{
    //    private readonly ISunSystemConnectionProvider _connectionProvider;

    //    public LocalSunSystemRepository(ISunSystemConnectionProvider connectionProvider)
    //    {
    //        _connectionProvider = connectionProvider;
    //    }

    //    public async Task<(string AccountName, string Address, string VatCode)?> GetCustomer(string accountCode)
    //    {


    //        var connectionString = _connectionProvider.GetConnectionString();

    //        using (var con = new SqlConnection(connectionString))
    //        {
    //            try
    //            {
    //                await con.OpenAsync();
    //            }
    //            catch (SqlException sqlException)
    //            {
    //                switch (sqlException.Number)
    //                {
    //                    case 4060:
    //                    case 53:
    //                        throw new ConnectionFailedException("Connection failed to Sun System", sqlException);
    //                    default:
    //                        throw;
    //                }
    //            }

    //            var customer = await
    //                con.QuerySingleOrDefaultAsync<SunDbCustomerLookup>("SELECT * FROM View_Customers_ABS WHERE CODE=@Code",
    //                    new {Code = accountCode});

    //            if (customer == null)
    //            {
    //                return null;
    //            }

    //            return (customer.Name, customer.GetAddress(),customer.TaxIdentificationCode);
    //        }
    //    }


    //    public async Task<List<SunDbCustomerLookup>> GetCustomersAsync(string property, string value)
    //    {
    //        var connectionString = _connectionProvider.GetConnectionString();

    //        using (var con = new SqlConnection(connectionString))
    //        {
    //            try
    //            {
    //                await con.OpenAsync();
    //            }
    //            catch (SqlException sqlException)
    //            {
    //                switch (sqlException.Number)
    //                {
    //                    case 4060:
    //                    case 53:
    //                        throw new ConnectionFailedException("Connection failed to Sun System", sqlException);
    //                    default:
    //                        throw;
    //                }
    //            }

    //            var commandText = $@"SELECT * FROM View_Customers_ABS WHERE ";
    //            switch (property)
    //            {
    //                case "name":
    //                    commandText=commandText + $"NAME LIKE '%{value}%'";
    //                    break;
    //                case "lookup":
    //                    commandText=commandText + $"LOOKUP LIKE '%{value}%'";
    //                    break;
    //                case "trnNo":
    //                    commandText=commandText + $"LTRIM(RTRIM(VatCode)) ='%{value}%'";
    //                    break;
    //            }

    //           return  (await con.QueryAsync<SunDbCustomerLookup>(commandText, commandType: CommandType.Text)).ToList();
    //        }
    //    }


    //    public async Task<(bool success, string trnNo, string accountName)> ValidateSunAccountCodeAsync(string accountCode,string sunDb=null)
    //    {
    //        var connectionString = _connectionProvider.GetConnectionString();
    //        var commandText = @"[dbo].[GetSunVATID]";
    //        using (var con = new SqlConnection(connectionString))
    //        {
    //            con.Open();
    //            object parameter;
    //            if (sunDb == null)
    //                parameter = new { SunAccountNo = accountCode};
    //            else
    //                parameter = new { SunAccountNo = accountCode, SunDb = sunDb};

    //            var item = await con.QuerySingleOrDefaultAsync<VatInfo>(commandText, param: parameter,
    //                commandType: CommandType.StoredProcedure);
    //            if (item != null)
    //            {
    //                return (true, item.VatRegistrationNo, item.AccountName);
    //            }

    //            return (false, string.Empty, string.Empty);
    //        }
    //    }

    //    public Task<IEnumerable<SunAccountInfo>> GetSunAccountInfoAsync(string trnNo)
    //    {
    //        var connectionString = _connectionProvider.GetConnectionString();
    //        var commandText = @"[dbo].[GetSunByTRN]";
    //        using (var con = new SqlConnection(connectionString))
    //        {
    //            con.Open();
    //            return  con.QueryAsync<SunAccountInfo>(commandText, param:new {TrnNo=trnNo}, commandType: CommandType.StoredProcedure);
    //        }
    //    }

    //   
    //}
}