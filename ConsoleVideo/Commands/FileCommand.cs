using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVideo.Commands
{
    internal class FileCommand : ICommand
    {
        public string Name { get; set; } = "file";

        public bool Run()
        {
            Console.Clear();
            DirectoryInfo d = new DirectoryInfo(AsciiApp.Directory);
            string[]? extensions = { "*.bin", "*.mp4", "*.webm" };

            IEnumerable<FileInfo> files = extensions.SelectMany(ext => d.GetFiles(ext));

            foreach (FileInfo file in files)
            {
                Console.WriteLine(file.Name + "\n");
            }

            string? fileInput = Console.ReadLine();

            if (files.Any(f => f.Name == fileInput))
            {
                string path = Path.Combine(AsciiApp.Directory, fileInput);

                if (Path.GetExtension(fileInput) is ".mp4" or ".webm")
                {
                    Console.Clear();
                    string fn = Path.GetFileNameWithoutExtension(fileInput);

                    AsciiVideo vid = new AsciiVideo(path);
                    AsciiApp.CurrentVideo = vid;
                    Ascii.SaveAsciiVideoToFile(Path.ChangeExtension(path, ".bin"), vid);
                    return true;
                }

                AsciiApp.CurrentVideo = Ascii.GetVideoFromExistingFile(path);
                return false;
            }

            Console.WriteLine("File not found! Please try again!");
            Thread.Sleep(500);
            return false;
        }
    }
}
