using System;

namespace Jasmine.Core.Mvvm
{
    public class RouteNotFoundException : Exception
    {
        public RouteNotFoundException(string message) : base(message)
        {

        }
    }
}