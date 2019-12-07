using System.Net;
using Google.Apis.Auth.OAuth2.Responses;

namespace Jasmine.Core.Exceptions
{
    public class GmailTokenResponseException:TokenResponseException
    {
        public GmailTokenResponseException(TokenErrorResponse error) : base(error)
        {
          
        }

        public GmailTokenResponseException(TokenErrorResponse error, HttpStatusCode? statusCode) : base(error, statusCode)
        {

        }

    }
}