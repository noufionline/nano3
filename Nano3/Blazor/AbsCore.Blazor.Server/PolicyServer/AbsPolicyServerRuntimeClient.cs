using Jasmine.Abs.Entities.Models.Azman;
using Jasmine.Abs.Entities.Models.Zeon;
using JasmineCacheService;
using Microsoft.Extensions.Logging;
using PolicyServer.Runtime.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AbsCore.Blazor.PolicyServer
{
    public class AbsPolicyServerRuntimeClient : IPolicyServerRuntimeClient
    {
        private readonly ZeonContext _context;
        private readonly NetSqlAzmanContext _azmanContext;
        private readonly ILogger<AbsPolicyServerRuntimeClient> _logger;


        public AbsPolicyServerRuntimeClient(ZeonContext context,
                                            NetSqlAzmanContext azmanContext,
                                            ILogger<AbsPolicyServerRuntimeClient> logger)
        {
            _context = context;
            _azmanContext = azmanContext;
            _logger = logger;
        }
        public async Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {
            CacheServiceClient csc = CreateCacheServiceClient();

            await csc.OpenAsync().ConfigureAwait(false);

            List<string> permissions;
            List<string> roles;
            try
            {
                var sid = GetSid(user);

                var store = GetStore(user);

                (permissions, roles) = await GetAuthorizedItems(csc, sid, store);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                await csc.CloseAsync().ConfigureAwait(false);
            }


            var result = new PolicyResult
            {
                Permissions = permissions,
                Roles = roles
            };

            return result;
        }

        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var sid = GetSid(user);
            var store = GetStore(user);

            return await HasPermission(sid, store, permission);

        }
        public async Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            var sid = GetSid(user);
            var store = GetStore(user);
            return await HasRole(sid, store, role);
        }

        private async Task<(List<string> permissions, List<string> roles)> GetAuthorizedItems(CacheServiceClient authorizationCache,string sid,string store)
        {

            var permissions = new List<string>();
            var roles = new List<string>();

            var storeNames = new[] {store, "General", "Reports"};


            foreach (var storeName in storeNames)
            {
                var applications =
                    _azmanContext.NetsqlazmanApplicationsTables
                        .Where(x => x.Store.Name == store)
                        .Select(x => x.Name)
                        .ToList();

                foreach (var application in applications)
                {
                    permissions.AddRange((await authorizationCache.GetAuthorizedItemsForDatabaseUsersAsync(storeName,
                                application, sid,
                                DateTime.MinValue,
                                null)
                            .ConfigureAwait(false))
                        .Where(x =>
                            x.Type == ItemType.Operation && x.Authorization == AuthorizationType.Allow ||
                            x.Authorization == AuthorizationType.AllowWithDelegation).Select(x => x.Name));


                    roles.AddRange((await authorizationCache.GetAuthorizedItemsForDatabaseUsersAsync(storeName,
                                application, sid,
                                DateTime.MinValue,
                                null)
                            .ConfigureAwait(false))
                        .Where(x =>
                            x.Type == ItemType.Role && x.Authorization == AuthorizationType.Allow ||
                            x.Authorization == AuthorizationType.AllowWithDelegation).Select(x => x.Name));

                }
            }

            return (permissions, roles);
        }

        private async Task<bool> HasPermission(string sid, string store, string permission)
        {
            var csc = CreateCacheServiceClient();
            await csc.OpenAsync().ConfigureAwait(false);

            var permissions = new HashSet<AuthorizationType>();

            var storeNames = new[] { store, "General", "Reports" };

            try
            {
                var items = _azmanContext.NetsqlazmanItemsTables
                    .Where(n => storeNames.Contains(n.Application.Store.Name) && n.ItemType == 2 && n.Name == permission)
                    .Select(x => new
                    {
                        Store = x.Application.Store.Name,
                        Application = x.Application.Name
                    }).Distinct().ToList();



                foreach (var item in items)
                {
                    var authorizationType = await csc.CheckAccessForDatabaseUsersWithoutAttributesRetrieveAsync(
                            item.Store,
                            item.Application,
                            permission,
                            sid,
                            DateTime.MinValue,
                            true, null)
                        .ConfigureAwait(false);
                    permissions.Add(authorizationType);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
            finally
            {
                await csc.CloseAsync().ConfigureAwait(false);
            }

            //if any application has denied permission the straight away return false.

            if (permissions.Contains(AuthorizationType.Deny))
            {
                return false;
            }

            var allowed = permissions.Contains(AuthorizationType.Allow) ||
                          permissions.Contains(AuthorizationType.AllowWithDelegation);


            return allowed;

        }


        private async Task<bool> HasRole(string sid, string store, string role)
        {
            var csc = CreateCacheServiceClient();
            await csc.OpenAsync().ConfigureAwait(false);

            var roles = new HashSet<AuthorizationType>();

            try
            {
              
                var items = _azmanContext.NetsqlazmanItemsTables
                    .Where(n =>  n.Application.Store.Name==store &&
                                n.ItemType == 0 && n.Name == role)
                    .Select(x => new
                    {
                        Store = x.Application.Store.Name,
                        Application = x.Application.Name
                    }).Distinct().ToList();

                foreach (var item in items)
                {

                    var authorizationType = await csc.CheckAccessForDatabaseUsersWithoutAttributesRetrieveAsync(
                            item.Store,
                            item.Application,
                            role,
                            sid,
                            DateTime.MinValue,
                            false, null)
                        .ConfigureAwait(false);
                    roles.Add(authorizationType);

                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
            finally
            {
                await csc.CloseAsync().ConfigureAwait(false);
            }

          

            //if any application has denied permission the straight away return false.

            if (roles.Contains(AuthorizationType.Deny))
            {
                return false;
            }

            var allowed = roles.Contains(AuthorizationType.Allow) ||
                          roles.Contains(AuthorizationType.AllowWithDelegation);


            return allowed;

        
        }

        private string GetStore(ClaimsPrincipal user)
        {
            if (int.TryParse(user.FindFirstValue("divisionId"), out var divisionId))
            {
                var division = _context.AbsDivisions.SingleOrDefault(x => x.Id == divisionId);
                if (division != null)
                {
                    return division.StoreName;
                }
            }

            return null;
        }



        private string GetSid(ClaimsPrincipal user)
        {
            var employeeId =
                Convert.ToInt32(user.FindFirstValue("EmployeeId"));


            var intBytes = BitConverter.GetBytes(employeeId);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            var result = intBytes;

            var hex = new StringBuilder(result.Length * 2);
            foreach (var b in result)
                hex.AppendFormat("{0:x2}", b);

            return ByteArrayToString(result).ToUpper();
        }

        private string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }




        private CacheServiceClient CreateCacheServiceClient()
        {
            var csc = new CacheServiceClient();
            if (csc.ClientCredentials != null)
            {
                csc.ClientCredentials.Windows.ClientCredential.Domain = "WEBSERVER";
                csc.ClientCredentials.Windows.ClientCredential.UserName = "Noufal";
                csc.ClientCredentials.Windows.ClientCredential.Password = "MtpsF42";
            }

            return csc;
        }

    }

    public class TestAuthorizationService:IAuthorizationService
    {
        #region Implementation of IAuthorizationService

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return Task.FromResult(AuthorizationResult.Success());
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            return Task.FromResult(AuthorizationResult.Success());
        }

        #endregion
    }
}
