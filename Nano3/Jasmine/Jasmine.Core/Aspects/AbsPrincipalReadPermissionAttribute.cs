using System;

namespace Jasmine.Core.Aspects
{
    public class AbsPrincipalReadPermissionAttribute : Attribute
    {
        public string ViewName { get; }

        public AbsPrincipalReadPermissionAttribute(string viewName) => ViewName = viewName;
    }
}