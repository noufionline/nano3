using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace Jasmine.Core.Security
{
    //public sealed class JIdentity : ClaimsIdentity
    //{
    //    internal JIdentity(string authenticationType) : base(authenticationType,"Email",ClaimTypes.Role)
    //    {
    //    }

    //    public override void AddClaim(Claim claim)
    //    {
    //        if (Claims.Any(x => x.Type == claim.Type && x.Value == claim.Value && x.Issuer == claim.Issuer))
    //        {
    //            throw new NotSupportedException("Claims Duplicated");
    //        }
    //        base.AddClaim(claim);
    //    }

    //    public int EmployeeId => Convert.ToInt32(FindFirst("employeeId").Value);

    //    public string Email => FindFirst(ClaimTypes.Email).Value;

    //    public string Division => FindFirst("division").Value;

    //    public int DivisionId => Convert.ToInt32(FindFirst("divisionid").Value);

    //    public string Store => FindFirst("store").Value;
    //    public string AccessToken => FindFirst("accesstoken").Value;
    //    public string RefreshToken => FindFirst("refreshtoken").Value;
    //    public string[] Roles => FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();

    //    public static string Permission = "Permission";
    //    public JIdentity(UserCredential credential, string store) :
    //        base(new Claim[] { }, "ABS")
    //    {
    //        AddClaim(new Claim(ClaimTypes.Name, credential.UserName));
    //        AddClaim(new Claim("employeeId", credential.EmployeeId.ToString()));
    //        AddClaim(new Claim(ClaimTypes.Email, credential.UserName));
    //        AddClaim(new Claim("store", store));
    //    }


    //    public void ResetClaims(IEnumerable<UserClaim> claims=null)
    //    {
    //        foreach (Claim claim in Claims.ToList())
    //        {
    //            RemoveClaim(claim);
    //        }

    //        if (claims!=null)
    //        {
    //            foreach (UserClaim claim in claims)
    //            {
    //                AddClaim(new Claim(claim.Type, claim.Value, null, claim.Issuer));
    //            }
    //        }

    //    }


    //    public void ReplaceClaims(IEnumerable<UserClaim> claims = null)
    //    {
    //        if (claims != null)
    //        {
    //            foreach (UserClaim claim in claims)
    //            {
    //                RemoveClaim(FindFirst(claim.Type));
    //                AddClaim(new Claim(claim.Type, claim.Value, null, claim.Issuer));
    //            }
    //        }

    //    }

    //    public JIdentity(UserCredential credential, string store, IEnumerable<Claim> claims) :
    //        base(claims, "ABS")
    //    {
    //        AddClaim(new Claim(ClaimTypes.Name, credential.UserName));
    //        AddClaim(new Claim("employeeId", credential.EmployeeId.ToString()));
    //        AddClaim(new Claim(ClaimTypes.Email, credential.UserName));
    //        AddClaim(new Claim("store", store));
    //    }

    //    public JIdentity(IEnumerable<Claim> claims): base(claims, "ABS"){ }

    //    public JIdentity(string authenticationType, string nameType, string roleType) : base(authenticationType, nameType, roleType)
    //    {
    //    }

    //    public JIdentity()
    //    {
    //    }

    //    public static JIdentity Current
    //    {
    //        get
    //        {
    //            ClaimsPrincipal currentPrincipal = ClaimsPrincipal.Current;
    //            return currentPrincipal?.Identity as JIdentity;
    //        }
    //    }
    //}
}