using System;

namespace Jasmine.Core.Security
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string message):base(message)
        {
            
        }
    }
}