using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;

namespace Jasmine.Core.Odoo
{
    [XmlRpcUrl("object")]
    public interface IOdooObjectRpc : IXmlRpcProxy
    {
        [XmlRpcMethod("execute")]
        int Create(string database, int userId, string password, string model, string method, XmlRpcStruct fieldValues);

        [XmlRpcMethod("execute")]
        int[] Search(string database, int userId, string password, string model, string method, object[] filter);

        [XmlRpcMethod("execute")]
        bool Write(string database, int userId, string password, string model, string method, int[] ids, XmlRpcStruct fieldValues);

        [XmlRpcMethod("execute")]
        bool Unlink(string database, int userId, string password, string model, string method, int[] ids);

        [XmlRpcMethod("execute")]
        object[] Read(string database, int userId, string password, string model, string method, int[] ids, object[] fields);

        [XmlRpcMethod("exec_workflow")]
        bool ExecuteWorkflow(string dbName, int userId, string password, string model, string action, int ids);

        [XmlRpcMethod("execute")]
        ValidationInfo ValidatePartner(string dbName, int userId, string pwd, string model, string method, string sunAccountCode, string sunDb);

        [XmlRpcMethod("execute")]
        SunAccountInfo[] ValidateTrn(string dbName, int userId, string pwd, string model, string method,
            string trn);

        [XmlRpcMethod("execute")]
        TaxCodeWithAccountName[] ValidateAccountCode(string dbName, int userId, string pwd, string model, string method,
            string sunAccountCode);
    }

    public class SunAccountInfo : IEquatable<SunAccountInfo>
    {
        [XmlRpcMember("ACCNT_CODE")]
        public string AccountCode { get; set; }
        [XmlRpcMember("ACCNT_NAME")]
        public string AccountName { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SunAccountInfo);
        }

        public bool Equals(SunAccountInfo other)
        {
            return other != null &&
                   AccountCode == other.AccountCode &&
                   AccountName == other.AccountName;
        }

        public override int GetHashCode()
        {
            var hashCode = 1950441272;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AccountCode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AccountName);
            return hashCode;
        }

        public static bool operator ==(SunAccountInfo info1, SunAccountInfo info2)
        {
            return EqualityComparer<SunAccountInfo>.Default.Equals(info1, info2);
        }

        public static bool operator !=(SunAccountInfo info1, SunAccountInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
