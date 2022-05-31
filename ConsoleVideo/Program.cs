using ConsoleVideo;
using System.Runtime.Serialization.Formatters.Binary;

Console.CursorVisible = false;
Console.SetWindowSize(Console.LargestWindowWidth - 20, Console.LargestWindowHeight);


const string dir = @"C:\Users\Ben David\Documents\consolevideo";

AsciiVideo? currentVideo = null;
bool chosen = false;



while (!chosen)
{
    Console.Clear();
    Console.WriteLine("Enter command: url, file");
    string? input = Console.ReadLine();
    switch (input)
    {
        case "url":
            Console.Clear();
            Console.WriteLine("Enter url: ");

            string? url = Console.ReadLine();

            string? extension = Path.GetExtension(url);
            string? filename = Path.GetFileNameWithoutExtension(url);
            if (extension is ".mp4" or ".webm")
            {
                Console.Clear();
                string path = Path.Combine(dir, Path.GetFileName(url));

                AsciiVideo vid = new AsciiVideo(url, true);
                currentVideo = vid;

                Console.WriteLine("If this goes its bad");
                SaveAsciiVideoToFile(Path.ChangeExtension(path, ".bin"), vid);
                chosen = true;
            }
            else
            {
                Console.WriteLine("Not a valid url! Please try again! (ending with .mp4 or .webm)");
                Thread.Sleep(500);
                chosen = false;
            }

            break;
        case "file":
            Console.Clear();
            DirectoryInfo d = new DirectoryInfo(dir);
            string[]? extensions = { "*.bin", "*.mp4", "*.webm" };

            IEnumerable<FileInfo> files = extensions.SelectMany(ext => d.GetFiles(ext));

            foreach (FileInfo file in files)
            {
                Console.WriteLine(file.Name + "\n");
            }

            string? fileInput = Console.ReadLine();

            if (files.Any(f => f.Name == fileInput))
            {
                string path = Path.Combine(dir, fileInput);

                if (Path.GetExtension(fileInput) is ".mp4" or ".webm")
                {
                    Console.Clear();
                    string fn = Path.GetFileNameWithoutExtension(fileInput);

                    AsciiVideo vid = new AsciiVideo(path);
                    currentVideo = vid;
                    SaveAsciiVideoToFile(Path.ChangeExtension(path, ".bin"), vid);
                    chosen = true;
                }
                else
                {
                    currentVideo = GetVideoFromExistingFile(path);
                    chosen = true;
                }
            }
            else
            {
                Console.WriteLine("File not found! Please try again!");
                Thread.Sleep(500);
                chosen = false;
            }
            break;
        default:
            Console.WriteLine("Not a valid command.");
            Thread.Sleep(500);
            break;
    }
    Console.Clear();
}

currentVideo?.Play();


































#pragma warning disable SYSLIB0011
void SaveAsciiVideoToFile(string path, AsciiVideo video)
{
    using Stream stream = File.Open(path, FileMode.Create);
    BinaryFormatter formatter = new BinaryFormatter();

    formatter.Serialize(stream, video);
}

AsciiVideo GetVideoFromExistingFile(string path)
{
    using Stream stream = File.Open(path, FileMode.Open);
    BinaryFormatter formatter = new BinaryFormatter();

    return (AsciiVideo)formatter.Deserialize(stream);
}
#pragma warning restore SYSLIB0011



