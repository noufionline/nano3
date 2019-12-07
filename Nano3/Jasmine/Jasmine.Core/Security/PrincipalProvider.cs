using System.Security.Claims;
using System.Security.Principal;

namespace Jasmine.Core.Security
{
    public sealed class PrincipalProvider : IPrincipal
    {
        private PrincipalProvider()
        {
        }

        public static PrincipalProvider Current { get; } = new PrincipalProvider();


        public ClaimsPrincipal Principal { get;  set; } = new ClaimsPrincipal(new ClaimsIdentity());

        public  IIdentity Identity => Principal?.Identity;

        public bool IsInRole(string role) => Principal?.IsInRole(role) == true;
    }
}