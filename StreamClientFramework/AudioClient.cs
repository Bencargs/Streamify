using NAudio.Wave;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace StreamClient
{
    public class AudioClient
    {
        public async Task PlayAudioStream(string url)
        {
            var response = await WebRequest.Create(url).GetResponseAsync();
            using (Stream ms = new MemoryStream())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                ms.Position = 0;
                using (WaveStream blockAlignedStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(ms))))
                using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                {
                    waveOut.Init(blockAlignedStream);
                    waveOut.Play();
                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }
    }
}
