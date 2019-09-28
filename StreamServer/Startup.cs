using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace StreamServer
{
    public class Startup
    {
        private const string MusicPath = @"C:\Source\Streamify";

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var stream = StreamAudio();
                await stream.ExecuteResultAsync(context);
            });
        }

        private PushStreamResult StreamAudio()
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(MusicPath);
            foreach (System.IO.FileInfo fi in di.GetFiles("*.mp3"))
            {
                var audio = new AudioStream(fi.FullName);
                return new PushStreamResult(audio.WriteAudioStream, "audio/mpeg");
            }
            return null;
        }
    }
}
