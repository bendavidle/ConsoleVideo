using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVideo
{
    [SupportedOSPlatform("windows")]
    [Serializable]
    public class AsciiArt
    {

        public string? Art;


        public AsciiArt(string path, int width)
        {
            Bitmap? image = Ascii.GetReSizedImage(Ascii.BitmapFromPath(path), width);
            Art = Ascii.ConvertToAscii(image);
        }

        public AsciiArt(Bitmap image, int width)
        {
            Bitmap? newImage = Ascii.GetReSizedImage(image, width);
            Art = Ascii.ConvertToAscii(newImage);
        }

        public AsciiArt(string urlPath, int width, bool url)
        {
            if (url)
            {
                Bitmap? image = Ascii.GetReSizedImage(Ascii.GetBitmapFromUrl(urlPath), width);
                Art = Ascii.ConvertToAscii(image);
            }
        }

    }
}
