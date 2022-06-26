using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleVideo
{
    internal class AsciiApp
    {
        public static AsciiVideo? CurrentVideo;
        public static string? Directory;


        private static void Setup()
        {
            Console.SetWindowSize(Console.LargestWindowWidth - 20, Console.LargestWindowHeight);

            Directory = ConfigurationManager.AppSettings.Get("Directory");

            while (Directory.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("Enter a Directory for to use. Has to be in Documents folder.");
                string? input = Console.ReadLine();


                if (input != null && input.ToLower().Contains("documents"))
                {
                    if (System.IO.Directory.Exists(Path.GetDirectoryName(input))) { }
                    {
                        var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        configFile.AppSettings.Settings.Remove("Directory");
                        configFile.AppSettings.Settings.Add("Directory", input);
                        configFile.Save();
                        ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                        Directory = input;

                        Console.CursorVisible = false;

                    }
                }


            }
        }

        public static void Start()
        {
            Setup();

            bool chosen = false;
            while (!chosen)
            {
                Console.Clear();
                Console.WriteLine("Enter command: url, file");
                string? input = Console.ReadLine();

                chosen = CommandManager.Send(input);
            }
            Console.Clear();

            while (true)
            {
                CurrentVideo?.Play();
            }
        }

    }
}
