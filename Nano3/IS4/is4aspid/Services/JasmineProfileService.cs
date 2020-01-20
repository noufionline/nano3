using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using is4aspid.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace is4aspid.Services
{
    /// <summary>
    /// Jasmine profile service implementation.
    /// This implementation sources all claims from the current subject (e.g. the cookie) + divisionId from the request.
    /// </summary>
    /// <seealso cref="IdentityServer4.Services.IProfileService" />
    public class JasmineProfileService : IProfileService
    {
        private const string DivisionId = "DivisionId";
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProfileService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="userManager"></param>
        public JasmineProfileService(ILogger<JasmineProfileService> logger,
            IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(_logger);
            var claims = context.Subject.Claims.ToList();

            ApplicationUser user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());


            string[] claimTypes = { "EmployeeId", "EmployeeName", DivisionId, "Email" };

            claims.RemoveAll(claim => claimTypes.Contains(claim.Type));

            if (_httpContextAccessor.HttpContext.Request.HasFormContentType)
            {
                Microsoft.Extensions.Primitives.StringValues divisionId = _httpContextAccessor.HttpContext.Request.Form[DivisionId];
                claims.Add(new Claim(DivisionId, divisionId.ToString()));
            }


            claims.Add(new Claim("EmployeeId", user.EmployeeId.ToString()));
            claims.Add(new Claim("name", user.EmployeeName));
            claims.Add(new Claim("EmployeeName", user.EmployeeName));
            claims.Add(new Claim("Email", user.Email));

            context.AddRequestedClaims(claims);
            context.LogIssuedClaims(_logger);


        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            _logger.LogDebug("IsActive called from: {caller}", context.Caller);

            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
