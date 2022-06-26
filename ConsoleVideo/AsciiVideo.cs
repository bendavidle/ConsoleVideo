using Emgu.CV;
using Emgu.CV.CvEnum;
using ShellProgressBar;
using System.Net;
using System.Text.Json.Serialization;

namespace ConsoleVideo
{
    [Serializable]
    public class AsciiVideo
    {
        public List<AsciiArt> Frames;
        public double Fps;
        public int Duration;

        //Generate Video from URL.
        public AsciiVideo(string urlPath, bool url)
        {
            Frames = new List<AsciiArt>();
            if (url)
            {
                Ascii.DownloadVideo(urlPath);



                string ext = Path.GetExtension(urlPath);

                while (!File.Exists(AsciiApp.Directory + "temp" + ext)) { } //Wait for Video to download.

                if (File.Exists(AsciiApp.Directory + "temp" + ext))
                {
                    GenerateAsciiVideo(AsciiApp.Directory + "temp" + ext);
                }
                File.Delete(AsciiApp.Directory + "temp" + ext);
            }
        }

        //Generate Video from Filepath.
        public AsciiVideo(string filename)
        {
            Frames = new List<AsciiArt>();
            GenerateAsciiVideo(filename);
        }

        private void GenerateAsciiVideo(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("This file was not found.");
            }

            ProgressBarOptions? options = new ProgressBarOptions
            {
                ProgressCharacter = '-',
                ProgressBarOnBottom = true
            };

            using VideoCapture? video = new VideoCapture(filename);

            int totalFrames = (int)Math.Floor(video.Get(CapProp.FrameCount));


            Fps = video.Get(CapProp.Fps);
            Duration = (int)(totalFrames / Fps);


            using Mat? img = new Mat();

            using ProgressBar? pbar = new ProgressBar(totalFrames, "Loading Video", options);
            while (video.Grab())
            {
                Console.SetCursorPosition(0, 0);
                video.Retrieve(img);
                Frames.Add(new AsciiArt(img.ToBitmap(), 200));
                pbar.Tick();
            }
        }

        //Play video.
        public void Play()
        {

            Console.Clear();
            int currentFrame = 0;

            TimeSpan dur = TimeSpan.FromSeconds(Duration);
            TimeSpan curDur = TimeSpan.FromSeconds(0);

            foreach (AsciiArt? frame in Frames)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(curDur.ToString(@"hh\:mm\:ss") + " | " + dur.ToString(@"hh\:mm\:ss"));
                Console.SetCursorPosition(0, 1);
                Console.WriteLine(frame.Art);
                Thread.Sleep((int)(1000 / Fps));
                currentFrame++;
                curDur = TimeSpan.FromSeconds((int)(currentFrame / Fps));

            }
        }

    }
}
