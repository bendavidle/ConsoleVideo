using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVideo.Commands
{
    internal class UrlCommand : ICommand
    {
        public string Name { get; set; } = "url";

        public bool Run()
        {
            Console.Clear();
            Console.WriteLine("Enter url: ");

            string? url = Console.ReadLine();

            string? extension = Path.GetExtension(url);

            if (extension is ".mp4" or ".webm")
            {
                Console.Clear();
                string path = Path.Combine(AsciiApp.Directory, Path.GetFileName(url));

                AsciiVideo vid = new AsciiVideo(url, true);
                AsciiApp.CurrentVideo = vid;

                Console.WriteLine("If this goes its bad");
                Ascii.SaveAsciiVideoToFile(Path.ChangeExtension(path, ".bin"), vid);
                //chosen = true;
                return true;
            }

            Console.WriteLine("Not a valid url! Please try again! (ending with .mp4 or .webm)");
            Thread.Sleep(500);
            //chosen = false;
            return false;
        }
    }
}
