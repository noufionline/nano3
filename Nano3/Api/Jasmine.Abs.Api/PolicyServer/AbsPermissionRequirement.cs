using Microsoft.AspNetCore.Authorization;

namespace Jasmine.Abs.Api.PolicyServer
{
    public class AbsPermissionRequirement:IAuthorizationRequirement
    {
        public string Name { get; }

        public AbsPermissionRequirement(string name)
        {
            Name = name;
        }
    }
}