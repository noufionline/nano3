using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using PostSharp.Patterns.Contracts;

namespace Jasmine.Core.Security
{
    public sealed class AbsPrincipal : ClaimsPrincipal
    {
        private AbsIdentity _identity;

        public override void AddIdentity([Required] ClaimsIdentity identity) => SetCurrentIdentity(identity);


        private void SetCurrentIdentity([Required] ClaimsIdentity identity)
        {
            if (!identity.IsAuthenticated)
                throw new NotSupportedException("Only authenticated Identity can be used with this constructor");
       
            _identity = identity as AbsIdentity;
             if (!Identities.Any())
            {
                base.AddIdentity(identity);
            }
            else
            {
                ClaimsIdentity primaryIdentity = Identities.First();
                primaryIdentity.Claims.ToList().ForEach(primaryIdentity.RemoveClaim);
                primaryIdentity.AddClaims(identity.Claims);
            }
        }


        public override void AddIdentities(IEnumerable<ClaimsIdentity> identities) => throw new NotSupportedException("Abs Principal does not support Multiple Identities");

        public AbsPrincipal([Required] AbsIdentity identity)
        {
            if (identity.IsAuthenticated)
                AddIdentity(identity);
            else
                throw new NotSupportedException("Only authenticated Identity can be used with this constructor");
        }

        public AbsPrincipal()
        {
        }


        public override IIdentity Identity => _identity;

        public static void SetIdentity([Required] IIdentity identity)
        {
            AbsPrincipal principal = Current as AbsPrincipal;
            principal?.SetCurrentIdentity(identity as AbsIdentity);
        }

        public static void ClearIdentity() => Thread.CurrentPrincipal = new AbsPrincipal();
    }


    
}
