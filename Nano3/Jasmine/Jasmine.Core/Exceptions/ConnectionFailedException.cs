using System;
using System.Runtime.Serialization;

namespace Jasmine.Core.Exceptions
{
    [Serializable]
    public class ConnectionFailedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ConnectionFailedException()
        {
        }

        public ConnectionFailedException(string message) : base(message)
        {
        }

        public ConnectionFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConnectionFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}