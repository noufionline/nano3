using System.Data.SqlClient;
using System.Security.Claims;

namespace Jasmine.Core.Contracts
{
    public interface ISunSystemConnectionProvider
    {
        string GetConnectionString();
        string GetConnectionString(string dataSource,string initialCatalog,string userId,string password);
    }

    public class SunSystemConnectionProvider : ISunSystemConnectionProvider
    {
        public string GetConnectionString()
        {
            var dataSource = ClaimsPrincipal.Current.FindFirst("SunDbIpAddress").Value;
            return GetConnectionString(dataSource,"SUNDB","Cheque","Bajisanibm26");

        }

        public string GetConnectionString(string dataSource, string initialCatalog, string userId, string password)
        {
            var connectionBuilder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                UserID = userId,
                Password = password,
                InitialCatalog = initialCatalog
            };
            return connectionBuilder.ToString();
        }
    }
}