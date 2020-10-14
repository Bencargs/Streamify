using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;

namespace StreamServer
{
    [ApiController]
    [Route("api/[controller]")]
    public class AudioController : ControllerBase
    {
        private const string MusicPath = @"C:\Source\Streamify";
        private const string Extension = "mp3";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AudioController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task PlayAudio()
        {
            var response = CreateAudioResponse();

            var stream = new FileDataStream(MusicPath, Extension);
            await stream.WriteToStream(response.Body);
        }

        private HttpResponse CreateAudioResponse()
        {
            var response = _httpContextAccessor.HttpContext.Response;

            response.StatusCode = 200;
            response.Headers.Add(HeaderNames.ContentDisposition, "inline");
            response.Headers.Add(HeaderNames.ContentType, "audio/mpeg");

            return response;
        }
    }
}
