using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jasmine.Core.Chat.Extensions
{
    public static class FileExt
    {
        /// <summary>
        /// This is the same default buffer size as
        /// <see cref="StreamReader"/> and <see cref="FileStream"/>.
        /// </summary>
        private const int DefaultBufferSize = 4096;

        /// <summary>
        /// Indicates that
        /// 1. The file is to be used for asynchronous reading.
        /// 2. The file is to be accessed sequentially from beginning to end.
        /// </summary>
        private const FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;

        public static Task<List<string>> ReadAllLinesAsync(string path)
        {
            return ReadAllLinesAsync(path, Encoding.UTF8);
        }

        public static async Task<List<string>> ReadAllLinesAsync(string path, Encoding encoding)
        {
            var lines = new List<string>();

            // Open the FileStream with the same FileMode, FileAccess
            // and FileShare as a call to File.OpenText would've done.
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions))
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        public static async Task WriteLinesAsync(string path, List<string> lines)
        {
            foreach (var line in lines)
            {
                await WriteLineAsync(path, line, Encoding.UTF8);
            }
            
        }

        public static async Task WriteLineAsync(string path, string line)
        {
            await WriteLineAsync(path, line, Encoding.UTF8);
        }

        public static async Task WriteLineAsync(string path, string line, Encoding encoding)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write, DefaultBufferSize, DefaultOptions))
                {
                    using (var writer = new StreamWriter(stream))
                    {   
                        await writer.WriteLineAsync(line);                        
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        public static async Task UpdateAsync(string path, string line)
        {
            await UpdateAsync(path, line, Encoding.UTF8);
        }

        public static async Task UpdateAsync(string path, string line, Encoding encoding)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, DefaultBufferSize, DefaultOptions))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        await writer.WriteLineAsync(line);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
