using CookComputing.XmlRpc;

namespace Jasmine.Core.Odoo
{
    [XmlRpcUrl("common")]
    public interface IOdooCommonRpc : IXmlRpcProxy
    {
        [XmlRpcMethod("login")]
        int Login(string database, string userName, string password);
    }
}
