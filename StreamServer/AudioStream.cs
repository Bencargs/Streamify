using System;
using System.IO;

namespace StreamServer
{
    public class AudioStream
    {
        private string _filepath;

        public AudioStream(string filepath)
        {
            _filepath = filepath;
        }

        public void WriteAudioStream(Stream outputStream)
        {
            var buffer = new byte[32768];

            using (var stream = File.OpenRead(_filepath))
            {
                var length = (int)stream.Length;
                var bytesRead = 1;

                while (length > 0 && bytesRead > 0)
                {
                    bytesRead = stream.Read(buffer, 0, Math.Min(length, buffer.Length));
                    outputStream.Write(buffer, 0, bytesRead);
                    length -= bytesRead;
                }
            }
        }
    }
}
