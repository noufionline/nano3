using System.Collections.Generic;
using GraphQL.Authorization;
using System.Security.Claims;

namespace Jasmine.Abs.Api.GraphQL
{
    public class GraphQlUserContext : Dictionary<string, object>, IProvideClaimsPrincipal
    {
        public GraphQlUserContext(ClaimsPrincipal principal)
        {
            User = principal;
        }
        public ClaimsPrincipal User { get; }
    }
}
