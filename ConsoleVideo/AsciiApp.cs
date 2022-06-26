using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVideo
{
    internal class AsciiApp
    {
        public static AsciiVideo? CurrentVideo;
        public static string Directory = @"C:\Users\Ben David\Documents\consolevideo";


        public static void Start()
        {
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
