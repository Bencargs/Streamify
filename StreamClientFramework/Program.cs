namespace StreamClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"http://localhost:53018/";
            var audioClient = new AudioClient();
            audioClient.PlayAudioStream(path).Wait();
        }
    }
}
