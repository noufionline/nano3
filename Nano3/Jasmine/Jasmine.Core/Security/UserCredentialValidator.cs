using System;
using System.Text.RegularExpressions;
using FluentValidation;

namespace Jasmine.Core.Security
{
    public class UserCredentialValidator : AbstractValidator<UserCredential>
    {
        public UserCredentialValidator()
        {
            RuleFor(i => i.UserName).NotEmpty().WithMessage("Select the user name");
            RuleFor(i => i.Password).NotEmpty().WithMessage("Enter the password")
                .Matches(new Regex(@"[a-z]")).WithMessage("Password should contain at least one Lower Case Letter")
                .Matches(new Regex(@"[A-Z]")).WithMessage("Password should contain at least one Upper Case Letter")
                .Matches(new Regex(@"[0-9]")).WithMessage("Password should contain at least one Number")
                .Length(5, int.MaxValue).WithMessage("Password should contain minimum 5 characters");
                //.Must((vm, password) => vm.PasswordStrength == PasswordStrength.Strong).WithMessage("Only strong password is allowed")
              //  .Must(BeValid).WithMessage("Password Incorrect");

        }

        private bool BeValid(UserCredential userCredential, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            if (ValidatePasswordFunc != null)
                return ValidatePasswordFunc(userCredential, password);
            return true;
        }


        public Func<UserCredential, string, bool> ValidatePasswordFunc;


    }

    //public interface IJaguarAuthenticationManager
    //{
    //    Task TransformClaimsAsync();
    //    Task<JPrincipal> TransformClaimsAsync(ClaimsPrincipal incomingPrincipal, string store);
    //}

    //public class JaguarAuthenticationManager : IJaguarAuthenticationManager
    //{
    //    private readonly HttpClient _client;

    //    public JaguarAuthenticationManager(HttpClient client) => _client = client;

    //    public async Task<JPrincipal> TransformClaimsAsync(ClaimsPrincipal incomingPrincipal, string store)
    //    {
    //        if (incomingPrincipal.Identity != null && incomingPrincipal.Identity.IsAuthenticated)
    //        {
    //            HttpClient httpClient =new HttpClient { BaseAddress =new Uri(@"http://localhost:5051") };

    //            string accessToken = incomingPrincipal.FindFirst("accesstoken").Value;

    //            httpClient.SetBearerToken(accessToken);

    //            HttpResponseMessage response = await httpClient.GetAsync($"api/claims/{store}");

    //            response.EnsureSuccessStatusCode();

    //            List<UserClaim> claims = await response.Content.ReadAsAsync<List<UserClaim>>();

    //            foreach (UserClaim claim in claims)
    //            {
    //                ((JIdentity) incomingPrincipal.Identity).AddClaim(new Claim(claim.Type, claim.Value,"string",claim.Issuer));
    //            }
    //        }
    //        return incomingPrincipal as JPrincipal;
    //    }

    //    public async Task TransformClaimsAsync()
    //    {
    //        JIdentity identity = ClaimsPrincipal.Current.Identity as JIdentity;
    //        if (identity != null)
    //        {
    //            await TransformClaimsAsync(ClaimsPrincipal.Current, identity.Store);
    //        }
    //    }
    //}



    public class UserClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Issuer { get; set; }

    }

    //public class JaguarAuthenticationManager : ClaimsAuthenticationManager, IJaguarAuthenticationManager
    //{
    //    public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
    //    {
    //        //string connectionString =
    //        //    ConfigurationManager.ConnectionStrings["NetSqlAzManStorageContext"].ConnectionString;
    //        //JIdentity currentIdentity = incomingPrincipal.Identity as JIdentity;
    //        //if (currentIdentity != null && currentIdentity.IsAuthenticated)
    //        //{
    //        //    string userName = currentIdentity.Name;
    //        //    string store = currentIdentity.Store;
    //        //    SqlAzManStorage storage = new SqlAzManStorage(connectionString);
    //        //    IAzManDBUser dbUser = storage.GetDBUser(userName);
    //        //    List<KeyValuePair<string, IAzManApplication>> apps = storage.GetStore(store).Applications.ToList();

    //        //    StorageCache sc = new StorageCache(connectionString);
    //        //    sc.BuildStorageCache();

    //        //    List<Claim> claims = new List<Claim>();
    //        //    foreach (KeyValuePair<string, IAzManApplication> app in apps)
    //        //    {
    //        //        List<AuthorizedItem> items =
    //        //            new List<AuthorizedItem>(sc.GetAuthorizedItems(store, app.Value.Name,
    //        //                dbUser.CustomSid.ToString(), DateTime.MinValue));
    //        //        var permissions =
    //        //             items.Where(
    //        //                 i =>
    //        //                     i.Type == ItemType.Operation && i.Authorization == AuthorizationType.Allow ||
    //        //                     i.Authorization == AuthorizationType.AllowWithDelegation).ToList();

    //        //        var roles = items.Where(
    //        //                i =>
    //        //                    i.Type == ItemType.Role && i.Authorization == AuthorizationType.Allow ||
    //        //                    i.Authorization == AuthorizationType.AllowWithDelegation).ToList();
    //        //        claims.AddRange(permissions.Select(x => new Claim("Permission", x.Name, null, "NetSqlAzman")).ToList());
    //        //        claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x.Name, null, "NetSqlAzman")).ToList());
    //        //    }
    //        //    claims.AddRange(incomingPrincipal.Claims);
    //        //    return new JPrincipal(new JIdentity(claims));
    //        //}
    //        return incomingPrincipal;
    //    }


    //    public ClaimsPrincipal TransformClaims(ClaimsPrincipal incomingPrincipal)
    //    {
    //        var identity = Authenticate("none", incomingPrincipal);
    //        return identity;
    //    }
    //}
}