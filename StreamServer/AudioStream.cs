using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StreamServer
{
    public class FileDataStream
    {
        private readonly string _filepath;
        private readonly string _extension;
        private const int _bufferSize = 32768;
        private readonly Random _random = new Random();

        public FileDataStream(string filepath, string extension)
        {
            _filepath = filepath;
            _extension = extension;
        }

        public async Task WriteToStream(Stream outputStream)
        {
            using (outputStream)
            {
                foreach (var inputStream in GetInputStreams(_filepath, _extension))
                using (inputStream)
                {
                    await CopyStreamAsync(inputStream, outputStream);
                }
            }
        }

        private IEnumerable<Stream> GetInputStreams(string path, string extension)
        {
            var files = Directory.EnumerateFiles(path, $"*.{extension}")
                .OrderBy(x => _random.Next());

            foreach (var file in files)
            {
                yield return WithRetry(() => new FileStream(file,
                            FileMode.Open, FileAccess.Read, FileShare.None,
                            _bufferSize, useAsync: true));
            }
        }

        private async Task CopyStreamAsync(Stream readStream, Stream outputStream)
        {
            var buffer = new byte[_bufferSize];
            var length = (int)readStream.Length;
            
            var bytesRead = 1;
            while (length > 0 && bytesRead > 0)
            {
                bytesRead = await readStream.ReadAsync(buffer, 0, Math.Min(length, buffer.Length));
                await outputStream.WriteAsync(buffer, 0, bytesRead);
                length -= bytesRead;
            }
            await outputStream.FlushAsync();
        }

        private T WithRetry<T>(Func<T> action)
            where T : class
        {
            var retries = 0;
            T result = default;
            while (result == default && retries < 5)
            {
                try
                {
                    result = action();
                }
                catch (IOException)
                {
                    Task.Delay(200);
                    retries++;
                }
            }
            return result;
        }
    }
}
