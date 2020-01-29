using Microsoft.AspNetCore.Authorization;

namespace AbsCore.Blazor.PolicyServer
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