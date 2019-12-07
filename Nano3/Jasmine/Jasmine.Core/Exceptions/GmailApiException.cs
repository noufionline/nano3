using System;

namespace Jasmine.Core.Exceptions
{
    public class GmailApiException:Google.GoogleApiException
    {
        public GmailApiException(string serviceName, string message, Exception inner) : base(serviceName, message, inner)
        {
        }

        public GmailApiException(string serviceName, string message) : base(serviceName, message)
        {
        }
    }
}