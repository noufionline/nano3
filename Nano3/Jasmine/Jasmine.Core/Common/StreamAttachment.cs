using System.IO;

namespace Jasmine.Core.Common
{
    public class StreamAttachment
    {
        public string FileName { get; set; }
        public Stream File { get; set; }
    }
}