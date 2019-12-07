using System;
using System.Configuration;
using System.Security.Claims;

namespace Jasmine.Core.Odoo
{
    public class OdooCredentials
    {

        private string _commonUrl = "common";
        private string _objectUrl = "object";

        public OdooCredentials():this(
            ConfigurationManager.AppSettings.Get("OdooDomain"),
            ConfigurationManager.AppSettings.Get("OdooDatabase"))
        {
        }

        public OdooCredentials(string server, string database)
        {
            var principal = ClaimsPrincipal.Current;
            var serverPrefix = principal.FindFirst("OdooServerPrefix")?.Value;
            Server =string.Format(server,serverPrefix);
            Database = database;
            Username =principal.FindFirst("OdooUserName")?.Value;
            Password = principal.FindFirst("OdooPassword")?.Value;
        }


        public OdooCredentials(string server, string database,string userName,string password)
        {
            var principal = ClaimsPrincipal.Current;
            Server = server;
            Database = database;
            Username = userName;
            Password = password;
        }
        public string Server { get; }
        public string Database { get; }
        public string Username { get; }
        public string Password { get; }
        public int UserId { get; set; }

        public string CommonUrl => $"{Server}/xmlrpc/{_commonUrl}";

        public string ObjectUrl => $"{Server}/xmlrpc/{_objectUrl}";
    }
}
