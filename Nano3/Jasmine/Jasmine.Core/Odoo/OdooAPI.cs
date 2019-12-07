using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using CookComputing.XmlRpc;
using Jasmine.Core.Exceptions;

namespace Jasmine.Core.Odoo
{
    public class OdooApi
    {
        private readonly OdooCredentials _credentials;
        private readonly WebProxy _networkProxy;
        private readonly bool _serverCertificateValidation;
        private IOdooObjectRpc _objectRpc;

        public OdooApi(OdooCredentials credentials, bool immediateLogin = true, WebProxy networkProxy = null, bool serverCertificateValidation = true)
        {
            _serverCertificateValidation = serverCertificateValidation;
            _networkProxy = networkProxy;
            _credentials = credentials;

            if(immediateLogin)
            {
                Login();
            }
        }

        public bool Login()
        {
            IOdooCommonRpc loginRpc = XmlRpcProxyGen.Create<IOdooCommonRpc>();
            loginRpc.Url = _credentials.CommonUrl;

            if (_networkProxy != null)
            {
                loginRpc.Proxy = _networkProxy;
            }

            if (_serverCertificateValidation)
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            }


            int userId = loginRpc.Login(_credentials.Database, _credentials.Username, _credentials.Password);

            _credentials.UserId = userId;



            _objectRpc = XmlRpcProxyGen.Create<IOdooObjectRpc>();
            _objectRpc.Url = _credentials.ObjectUrl;

            return true;
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public int Create(string model, XmlRpcStruct fieldValues)
        {
            return _objectRpc.Create(_credentials.Database, _credentials.UserId, _credentials.Password, model, "create", fieldValues);
        }

        public int[] Search(string model, object[] filter)
        {
            return _objectRpc.Search(_credentials.Database, _credentials.UserId, _credentials.Password, model, "search", filter);
        }

        public object[] Read(string model, int[] ids, string[] fields)
        {
            return _objectRpc.Read(_credentials.Database, _credentials.UserId, _credentials.Password, model, "read", ids, fields);
        }

        public bool Write(string model, int[] ids, XmlRpcStruct fieldValues)
        {
            return _objectRpc.Write(_credentials.Database, _credentials.UserId, _credentials.Password, model, "write", ids, fieldValues);
        }

        public bool Remove(string model, int[] ids)
        {
            return _objectRpc.Unlink(_credentials.Database, _credentials.UserId, _credentials.Password, model, "unlink", ids);
        }

        public bool Execute_Workflow(string model, string action, int id)
        {
            return _objectRpc.ExecuteWorkflow(_credentials.Database, _credentials.UserId, _credentials.Password, model, action, id);
        }



        public ValidationInfo ValidatePartner(string model, string method, string accountCode, string db="CAD")
        {
            ValidationInfo result= _objectRpc.ValidatePartner(_credentials.Database,
                _credentials.UserId, _credentials.Password,
                model, method, accountCode, db);
            return result;
        }


        public TaxCodeWithAccountName ValidateAccountCode(string accountCode)
        {
            TaxCodeWithAccountName[] result= _objectRpc.ValidateAccountCode(_credentials.Database,
                _credentials.UserId, _credentials.Password,
                "cic.rating.category", "validate_sun_account_no", accountCode);

            return result.Length == 0 ? null : result.First();
        }


        public List<SunAccountInfo> GetSunAccountDetails(string trn)
        {
            var result= _objectRpc.ValidateTrn(_credentials.Database,
                _credentials.UserId, _credentials.Password,
                "cic.rating.category", "validate_trn",trn);
            return result.ToList();
        }
        public OdooModel GetModel(string model)
        {
            return new OdooModel(model, this);
        }

        public OdooModel GetModel<T>(bool populateFields=true) where T : class
        {
            Attribute attribute = typeof(T).GetCustomAttribute(typeof(OdooModelNameAttribute));
            if (attribute is OdooModelNameAttribute modelNameAttribute)
            {
                OdooModel model = GetModel(modelNameAttribute.Name);
                if (populateFields)
                {
                    model.AddFields(GetOdooFields(typeof(T)));
                }

                return model;
            }

            return null;
        }

        private string[] GetOdooFields(Type entity)
        {
            PropertyInfo[] props = entity.GetProperties();
            var list = new List<string>();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    if (attr is OdooFieldNameAttribute authAttr)
                    {
                        string auth = authAttr.Name;
                        list.Add(auth);

                    }
                }
            }

            return list.ToArray();
        }
    }

    public class ValidationInfo
    {
        // Fields
        [XmlRpcMember("allow_transaction")]
        public bool AllowTransaction;
        [XmlRpcMember("partner_id")]
        public int CustomerId;
        [XmlRpcMember("partner_name")]
        public string CustomerName;
        [XmlRpcMember("description")]
        public string Description;
        [XmlRpcMember("invalid_sun_code")]
        public bool InvalidSunAccountCode;
        [XmlRpcMember("is_project")]
        public bool IsProject;
        [XmlRpcMember("project_id")]
        public int ProjectId;
        [XmlRpcMember("sun_account_name")]
        public string ProjectName;
        [XmlRpcMember("rating_category")]
        public string RatingCategory;

        [XmlRpcMember("trn")]
        public string TaxRegistrationNo { get; set; }

        public override string ToString() =>
            $"Rating :({this.RatingCategory}) Descriptoin:({this.Description}) Allow Transaction:({this.AllowTransaction})";

    }

    public class TaxCodeWithAccountName
    {
        [XmlRpcMember("TAX_CODE")]
        public string TaxRegistationNo { get; set; }
        [XmlRpcMember("ACCNT_NAME")]
        public string AccountName { get; set; }


    }
}
