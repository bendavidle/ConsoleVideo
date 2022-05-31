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
    internal class AsciiArt
    {

        public string? Ascii;
        private static readonly string[] AsciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };


        public AsciiArt(string path, int width)
        {
            var image = GetReSizedImage(BitmapFromPath(path), width);
            Ascii = ConvertToAscii(image);
        }

        public AsciiArt(Bitmap image, int width)
        {
            var newImage = GetReSizedImage(image, width);
            Ascii = ConvertToAscii(newImage);
        }

        public AsciiArt(string urlPath, int width, bool url)
        {
            if (url)
            {
                var image = GetReSizedImage(GetBitmapFromUrl(urlPath), width);
                Ascii = ConvertToAscii(image);
            }
        }

        private static Bitmap GetReSizedImage(Bitmap inputBitmap, int asciiWidth)
        {
            //Calculate the new Height of the image from its width
            int asciiHeight = (int)Math.Ceiling((double)inputBitmap.Height * asciiWidth / inputBitmap.Width);

            //Create a new Bitmap and define its resolution
            Bitmap result = new Bitmap(asciiWidth, asciiHeight);
            Graphics g = Graphics.FromImage((Image)result);
            //The interpolation mode produces high quality images 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(inputBitmap, 0, 0, asciiWidth, asciiHeight);
            g.Dispose();
            return result;
        }


        private static string ConvertToAscii(Bitmap image)
        {
            bool toggle = false;
            StringBuilder sb = new StringBuilder();

            for (int h = 0; h < image.Height; h++)
            {
                for (int w = 0; w < image.Width; w++)
                {
                    Color pixelColor = image.GetPixel(w, h);
                    //Average out the RGB components to find the Gray Color
                    int red = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    int green = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    int blue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    Color grayColor = Color.FromArgb(red, green, blue);

                    //Use the toggle flag to minimize height-wise stretch
                    if (!toggle)
                    {
                        int index = (grayColor.R * 10) / 255;
                        sb.Append(AsciiChars[index]);
                    }
                }

                if (!toggle)
                {
                    sb.Append(Environment.NewLine);
                    toggle = true;
                }
                else
                {
                    toggle = false;
                }
            }

            return sb.ToString();
        }

        private static Bitmap BitmapFromPath(string path)
        {
            return new Bitmap(path);
        }

        private static Bitmap GetBitmapFromUrl(string remoteImageUrl)
        {
            WebRequest request = WebRequest.Create(remoteImageUrl);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            Bitmap bitmap = new Bitmap(responseStream);
            return bitmap;
        }
    }
}
