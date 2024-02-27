using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace NPS.Helpers
{
    internal static class Utils
    {
        private static readonly string[] Units =
        {
            "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"
        };

        /// <summary>
        /// Returns short string with content size info.
        /// </summary>
        /// <param name="sizeInBytes"> Size of content in bytes. </param>
        /// <returns> Short rounded size and unit. </returns>
        [NotNull]
        public static (double, string) GetSizeShort(double sizeInBytes)
        {
            var i = 0;
            while (sizeInBytes >= 1024.0 && i < 8)
            {
                sizeInBytes /= 1024.0;
                ++i;
            }
            return (Math.Round(sizeInBytes, 2), Units[i]);
        }

        /// <summary>
        /// Returns string with size of file at specified URI.
        /// </summary>
        /// <param name="uri"> URI of remote file. </param>
        /// <returns> String with size of file e.g. "1.23 MiB". </returns>
        [NotNull]
        public static string GetSize([NotNull] string uri)
        {
            try
            {
                var webRequest = WebRequest.Create(uri);
                webRequest.Proxy = Settings.Instance.proxy;
                webRequest.Method = "HEAD";

                string s;
                using (var webResponse = webRequest.GetResponse())
                {
                    var fileSize = webResponse.Headers.Get("Content-Length");
                    webResponse.Close();
                    var fileSizeInMegaByte = GetSizeShort(Convert.ToDouble(fileSize));
                    s = $"{fileSizeInMegaByte.Item1} {fileSizeInMegaByte.Item2}";
                }
                return s;
            }
            catch (WebException e)
            {
                if (!(e.Response is HttpWebResponse errorResponse))
                {
                    // errorResponse not of type HttpWebResponse
                    Console.WriteLine($"Error: {e.Message}");
                    return string.Empty;
                }

                string responseContent = string.Empty;

                using (StreamReader r = new StreamReader(errorResponse.GetResponseStream()))
                {
                    responseContent = r.ReadToEnd();
                }

                Console.WriteLine("The server at {0} returned {1}", errorResponse.ResponseUri, errorResponse.StatusCode);

                Console.WriteLine("With headers:");
                foreach (string key in errorResponse.Headers.AllKeys)
                {
                    Console.WriteLine("\t{0}:{1}", key, errorResponse.Headers[key]);
                }

                Console.WriteLine(responseContent);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return string.Empty;
        }


        /// <summary>
        /// Get thumb image for specified one.
        /// </summary>
        /// <param name="image"> Big image to create thumbnail. </param>
        /// <returns> Small thumb image. </returns>
        [NotNull]
        public static Bitmap GetThumb([NotNull] Image image)
        {
            int tw, th;
            int w = image.Width;
            int h = image.Height;
            double whRatio = (double)w / h;

            if (image.Width >= image.Height)
            {
                tw = 100;
                th = (int)(tw / whRatio);
            }
            else
            {
                th = 100;
                tw = (int)(th * whRatio);
            }
            int tx = (100 - tw) / 2;
            int ty = (100 - th) / 2;
            Bitmap thumb = new Bitmap(100, 100, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(thumb);
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(
                image,
                new Rectangle(tx, ty, tw, th),
                new Rectangle(0, 0, w, h),
                GraphicsUnit.Pixel);
            return thumb;
        }
    }
}
