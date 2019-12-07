using GrpcService.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SqlConnectionStringBuilder = Microsoft.Data.SqlClient.SqlConnectionStringBuilder;

namespace GrpcService
{
    public class AbsConnectionStringProvider : IAbsConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public AbsConnectionStringProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("db", out var db))
            {
                var connectionString = configuration.GetConnectionString("CICONABS");
                var builder = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = db };
                ConnectionString = builder.ToString();
            }

            _configuration = configuration;
        }

        public string GetConnectionString(string db)
        {
            var connectionString = _configuration.GetConnectionString("CICONABS");
            var builder = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = db };
            return builder.ToString();
        }

        public string ConnectionString { get; }
    }
}
