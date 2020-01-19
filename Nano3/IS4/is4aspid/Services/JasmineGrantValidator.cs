using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace is4aspid.Services
{
    public class JasmineGrantValidator : IExtensionGrantValidator
    {

        private readonly ITokenValidator _validator;

        public JasmineGrantValidator(ITokenValidator validator)
        {
            _validator = validator;
        }
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
            try
            {
                string sub = result.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                var claims = result.Claims.ToList();
                claims.Add(new Claim("DivisionId", divisionId, null, "Titan"));
                var gresult= new GrantValidationResult(sub, "jasmine",claims.ToArray());
                context.Result = gresult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GrantType => "jasmine";
    }

 

}
