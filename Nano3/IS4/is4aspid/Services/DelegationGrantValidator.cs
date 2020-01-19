using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace is4aspid.Services
{
    public class DelegationGrantValidator : IExtensionGrantValidator
    {
        private readonly ITokenValidator _validator;

        public DelegationGrantValidator(ITokenValidator validator)
        {
            _validator = validator;
            
        }

        public string GrantType => "delegation";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            string userToken = context.Request.Raw.Get("token");
            string divisionId = context.Request.Raw.Get("DivisionId");

            if (string.IsNullOrEmpty(userToken))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            TokenValidationResult result = await _validator.ValidateAccessTokenAsync(userToken);
            if (result.IsError)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            // get user's identity
            string sub = result.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var claims = result.Claims.ToList();
            claims.Add(new Claim("DivisionId", divisionId, null, "Titan"));
            context.Result = new GrantValidationResult(sub, "delegation", claims.ToArray());
        }
    }

}
