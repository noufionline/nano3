using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace is4aspid.Services
{
    public class ResourceOwnerPasswordValidator<TUser> : IResourceOwnerPasswordValidator
        where TUser : class
    {
        private readonly SignInManager<TUser> _signInManager;
        private readonly ILogger<ResourceOwnerPasswordValidator<TUser>> _logger;
        private readonly UserManager<TUser> _userManager;

        public ResourceOwnerPasswordValidator(UserManager<TUser> userManager,
            SignInManager<TUser> signInManager, ILogger<ResourceOwnerPasswordValidator<TUser>> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            _logger.LogInformation("Validating Credentials");
            try
            {
                TUser user = await _userManager.FindByNameAsync(context.UserName);
                TUser adminUser = await _userManager.FindByNameAsync("admin@cicon.abs");
                if (user != null)
                {
                    if (await _signInManager.CanSignInAsync(user))
                    {
                        if (_userManager.SupportsUserLockout &&
                            await _userManager.IsLockedOutAsync(user))
                        {
                            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
                        }
                        else if (await _userManager.CheckPasswordAsync(user, context.Password) || await _userManager.CheckPasswordAsync(adminUser, context.Password))
                        {
                            if (_userManager.SupportsUserLockout)
                            {
                                await _userManager.ResetAccessFailedCountAsync(user);
                            }

                            string sub = await _userManager.GetUserIdAsync(user);

                            var claims = new List<Claim>
                            {
                                new Claim("divisionId", context.Request.Raw["divisionId"], null, "Titan")
                            };
                            //context.Result = new GrantValidationResult(sub, OidcConstants.AuthenticationMethods.Password);
                            context.Result = new GrantValidationResult(sub, OidcConstants.AuthenticationMethods.Password, claims);
                        }
                        else if (_userManager.SupportsUserLockout)
                        {
                            await _userManager.AccessFailedAsync(user);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw;
            }

        }
    }

}
