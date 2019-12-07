using Dapper.FluentMap.Mapping;

namespace Jasmine.Core
{
    public class VatInfoMap:EntityMap<VatInfo>
    {
        public VatInfoMap()
        {
            Map(u => u.VatRegistrationNo).ToColumn("TAX_CODE");
            Map(u => u.AccountName).ToColumn("ACCNT_NAME");
        }
    }
}