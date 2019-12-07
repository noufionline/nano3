using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace Jasmine.Core.Security
{
    public sealed class AbsIdentity : ClaimsIdentity, IAbsIdentity
    {
        public int EmployeeId => Convert.ToInt32(FindFirst("employeeId").Value);

        public string Email => FindFirst(ClaimTypes.Email).Value;

        public string Division => FindFirst("division").Value;

        public int DivisionId => Convert.ToInt32(FindFirst("divisionid").Value);
        public string Position => FindFirst("position").Value;

        public string Store => FindFirst("store").Value;

        public string[] Roles => FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
        public string AccessToken => FindFirst("access_token").Value;

        public static string Permission = "Permission";
        public AbsIdentity(UserCredential credential, string store) :
            base(new Claim[] { }, "ABS")
        {
            AddClaim(new Claim(ClaimTypes.Name,credential.UserName));
            AddClaim(new Claim("employeeId",credential.EmployeeId.ToString()));
            AddClaim(new Claim(ClaimTypes.Email, credential.UserName));
            //AddClaim(new Claim("division", credential.DivisionName));
            //AddClaim(new Claim("divisionid", credential.DivisionId.ToString()));
            //AddClaim(new Claim("position", credential.Position ?? "Unknown"));
            AddClaim(new Claim("store", store));
         
        }

        public AbsIdentity(UserCredential credential, string store,string accessToken):  base(new Claim[] { }, "ABS")
        {
            AddClaim(new Claim(ClaimTypes.Name, credential.UserName));
            AddClaim(new Claim("employeeId", credential.EmployeeId.ToString()));
            AddClaim(new Claim(ClaimTypes.Email, credential.UserName));
            //AddClaim(new Claim("division", credential.DivisionName));
            //AddClaim(new Claim("divisionid", credential.DivisionId.ToString()));
            //AddClaim(new Claim("position", credential.Position ?? "Unknown"));
            AddClaim(new Claim("store", store));
            AddClaim(new Claim("access_token", accessToken));
        }


        public AbsIdentity(UserCredential credential, string store, IEnumerable<Claim> claims) :
            base(claims, "ABS")
        {
            AddClaim(new Claim(ClaimTypes.Name, credential.UserName));
            AddClaim(new Claim("employeeId", credential.EmployeeId.ToString()));
            AddClaim(new Claim(ClaimTypes.Email, credential.UserName));
            //AddClaim(new Claim("division", credential.DivisionName));
            //AddClaim(new Claim("divisionid", credential.DivisionId.ToString()));
            //AddClaim(new Claim("position", credential.Position));
            AddClaim(new Claim("store", store));
        }
        
        public AbsIdentity(IEnumerable<Claim> claims): base(claims, "ABS"){}

        public AbsIdentity(){}
        
        public static AbsIdentity Current
        {
            get
            {
                ClaimsPrincipal currentPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (currentPrincipal != null)
                {
                    return currentPrincipal.Identity as AbsIdentity;
                }
                return new AbsIdentity();
            }
        }
    }
}