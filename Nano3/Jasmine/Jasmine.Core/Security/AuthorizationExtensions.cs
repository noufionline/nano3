using System;
using Humanizer;

namespace Jasmine.Core.Security
{
    public static class AuthorizationExtensions
    {
        public static string GetName(this Type item) => item.Name.Humanize(LetterCasing.Title);

        //public static bool HasReadPermission(this object commandParameter)
        //{
        //    if (string.IsNullOrWhiteSpace(commandParameter.ToString())) return false;

        //    if (commandParameter.ToString() == "Jaguar.Reports.Views.Criteria.CriteriaView") return true;
        //    Type parameter = commandParameter as Type;
        //    Type type = parameter ?? commandParameter.GetType();


        //    if (!(Attribute.IsDefined(type, typeof(AbsPrincipalReadPermissionAttribute))))
        //        throw new NotAuthorizedException($"ABS Principal Read Permission Attribute is not defined for {type.Name}");

        //    AbsPrincipalReadPermissionAttribute attribute = (AbsPrincipalReadPermissionAttribute)Attribute.GetCustomAttribute(type, typeof(AbsPrincipalReadPermissionAttribute));

        //    //return AbsClaimsAuthorization.CheckAccess(Operations.Read, attribute.ViewName);
        //    if (PrincipalProvider.Current.Principal is ClaimsPrincipal principal && attribute.ViewName != null)
        //    {
        //        if (principal.HasClaim("Permission", attribute.ViewName))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;

        //}
    }
}
