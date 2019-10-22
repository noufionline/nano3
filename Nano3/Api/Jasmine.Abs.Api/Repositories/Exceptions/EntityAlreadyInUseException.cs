using System;
using System.Runtime.Serialization;

namespace Jasmine.Abs.Api.Repositories.Exceptions
{
    [Serializable]
    public class EntityAlreadyInUseException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public EntityAlreadyInUseException()
        {
        }

        public EntityAlreadyInUseException(string message) : base(message)
        {
        }

        public EntityAlreadyInUseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected EntityAlreadyInUseException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
