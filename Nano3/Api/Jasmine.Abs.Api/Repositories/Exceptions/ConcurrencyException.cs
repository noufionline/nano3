using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Jasmine.Abs.Api.Repositories.Exceptions
{

    [Serializable]
    public class ConcurrencyException : Exception
    {
        public ConcurrencyErrorInfo ErrorInfo { get; }
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ConcurrencyException(ConcurrencyErrorInfo errorInfo)
        {
            ErrorInfo = errorInfo;
        }

        public ConcurrencyException()
        {

        }
        public ConcurrencyException(string message) : base(message)
        {
        }

        public ConcurrencyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConcurrencyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
