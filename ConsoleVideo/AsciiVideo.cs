using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Net;
using System.Text.Json.Serialization;

namespace ConsoleVideo
{
    [Serializable]
    internal class AsciiVideo
    {
        public List<AsciiArt> Frames;
        public double Fps;
        public int Duration;

        public AsciiVideo(string urlPath, bool url)
        {
            Frames = new List<AsciiArt>();
            if (url)
            {
                DownloadVideo(urlPath);

                string ext = Path.GetExtension(urlPath);

                while (!File.Exists(@"C:\Users\Ben David\Documents\consolevideo\" + "temp" + ext)) { }

                if (File.Exists(@"C:\Users\Ben David\Documents\consolevideo\" + "temp" + ext))
                {
                    GenerateAsciiVideo(@"C:\Users\Ben David\Documents\consolevideo\" + "temp" + ext);
                }
                File.Delete(@"C:\Users\Ben David\Documents\consolevideo\" + "temp" + ext);
            }
        }

        public AsciiVideo(string filename)
        {
            Frames = new List<AsciiArt>();
            GenerateAsciiVideo(filename);
        }

        private void GenerateAsciiVideo(string filename)
        {
            string path = filename;

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("This file was not found.");
            }

            using var video = new VideoCapture(path);


            Fps = video.Get(CapProp.Fps);
            int totalFrames = (int)Math.Floor(video.Get(CapProp.FrameCount));
            int currentFrames = 0;

            Duration = (int)(totalFrames / Fps);

            using var img = new Mat();
            while (video.Grab())
            {
                Console.SetCursorPosition(0, 0);
                video.Retrieve(img);
                Frames.Add(new AsciiArt(img.ToBitmap(), 200));
                currentFrames++;
                Console.Write($"Generating Ascii Video: {currentFrames} of {totalFrames} total frames.");
            }
        }

        public void Play()
        {

            Console.Clear();
            int currentFrame = 0;

            TimeSpan dur = TimeSpan.FromSeconds(Duration);
            TimeSpan curDur = TimeSpan.FromSeconds(0);

            foreach (var frame in Frames)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(curDur.ToString(@"hh\:mm\:ss") + " | " + dur.ToString(@"hh\:mm\:ss"));
                Console.SetCursorPosition(0, 1);
                Console.WriteLine(frame.Ascii);
                Thread.Sleep((int)(1000 / Fps));
                currentFrame++;
                curDur = TimeSpan.FromSeconds((int)(currentFrame / Fps));

            }
        }

        private void DownloadVideo(string url)
        {
            string ext = Path.GetExtension(url);

            if (ext is ".mp4" or ".webm")
            {

                using (var client = new WebClient())
                {
                    var content = client.DownloadData(url);
                    var stream = new MemoryStream(content);

                    string videoFile = Path.Combine(@"C:\Users\Ben David\Documents\consolevideo", "temp" + ext);

                    if (!File.Exists(videoFile))
                    {
                        using (FileStream outputStream = File.Create(videoFile))
                        {
                            stream.CopyTo(outputStream);
                        }
                    }
                }
            }



        }

    }
}
