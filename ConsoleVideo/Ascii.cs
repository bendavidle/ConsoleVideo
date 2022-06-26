using System.Drawing;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ConsoleVideo;

public class Ascii
{
    private static readonly string[] AsciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };

    public static Bitmap GetReSizedImage(Bitmap inputBitmap, int asciiWidth)
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


    public static string ConvertToAscii(Bitmap image)
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

    public static Bitmap BitmapFromPath(string path)
    {
        return new Bitmap(path);
    }

    public static Bitmap GetBitmapFromUrl(string remoteImageUrl)
    {
        WebRequest request = WebRequest.Create(remoteImageUrl);
        WebResponse response = request.GetResponse();
        Stream responseStream = response.GetResponseStream();
        Bitmap bitmap = new Bitmap(responseStream);
        return bitmap;
    }

    public static void DownloadVideo(string url)
    {
        string ext = Path.GetExtension(url);

        if (ext is ".mp4" or ".webm")
        {
            using WebClient? client = new WebClient();
            byte[]? content = client.DownloadData(url);
            MemoryStream? stream = new MemoryStream(content);

            string videoFile = Path.Combine(@"C:\Users\Ben David\Documents\consolevideo", "temp" + ext);

            if (!File.Exists(videoFile))
            {
                using FileStream outputStream = File.Create(videoFile);
                stream.CopyTo(outputStream);
            }
        }



    }

#pragma warning disable SYSLIB0011
    public static void SaveAsciiVideoToFile(string path, AsciiVideo video)
    {
        using Stream stream = File.Open(path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(stream, video);
    }

    public static AsciiVideo GetVideoFromExistingFile(string path)
    {
        using Stream stream = File.Open(path, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();

        return (AsciiVideo)formatter.Deserialize(stream);
    }
#pragma warning restore SYSLIB0011

}