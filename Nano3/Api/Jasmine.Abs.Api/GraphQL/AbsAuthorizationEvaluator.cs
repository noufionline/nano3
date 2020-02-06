using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AuthorizationResult = GraphQL.Authorization.AuthorizationResult;
using IAuthorizationEvaluator = GraphQL.Authorization.IAuthorizationEvaluator;

namespace Jasmine.Abs.Api.GraphQL
{

    public class AbsAuthorizationEvaluator : IAuthorizationEvaluator
    {
        private readonly IAuthorizationService _authz;

        public AbsAuthorizationEvaluator(IAuthorizationService authz)
        {
            _authz = authz;
        }
        public async Task<AuthorizationResult> Evaluate(ClaimsPrincipal principal, object userContext, Dictionary<string, object> arguments, IEnumerable<string> requiredPolicies)
        {
            var resut = await _authz.AuthorizeAsync(principal, requiredPolicies.First());
            if (resut.Succeeded)
                return AuthorizationResult.Success();
            else
                return AuthorizationResult.Fail(new List<string> { "You are not authorized to access this resource!" });
        }
    }
}
