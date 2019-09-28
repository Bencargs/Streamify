using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StreamServer
{
    public class PushStreamResult
    {
        private readonly Action<Stream> _onStreamAvailabe;
        private readonly string _contentType;

        public PushStreamResult(Action<Stream> onStreamAvailabe, string contentType)
        {
            _onStreamAvailabe = onStreamAvailabe;
            _contentType = contentType;
        }

        public Task ExecuteResultAsync(HttpContext context)
        {
            var stream = context.Response.Body;
            context.Response.GetTypedHeaders().ContentType = new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(_contentType);
            _onStreamAvailabe(stream);
            return Task.CompletedTask;
        }
    }
}
