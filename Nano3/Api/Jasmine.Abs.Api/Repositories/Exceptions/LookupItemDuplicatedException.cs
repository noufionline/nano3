using System;
using System.Runtime.Serialization;

namespace Jasmine.Abs.Api.Repositories.Exceptions
{
    [Serializable]
    public class LookupItemDuplicatedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public LookupItemDuplicatedException()
        {
        }

        public LookupItemDuplicatedException(string message) : base(message)
        {
        }

        public LookupItemDuplicatedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected LookupItemDuplicatedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
