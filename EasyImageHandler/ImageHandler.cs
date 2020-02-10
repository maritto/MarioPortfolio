using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyImageHandler
{
    public static class ImageHandler
    {
        public static class ImageFormat
        {
            public const string JPEG = "JPEG";
            public const string JPEG2 = "JPEG2";
            public const string JPEG3 = "JPEG3";
            public const string JPEG4 = "JPEG4";
            public const string BMP = "BMP";
            public const string GIF = "GIF";
            public const string PNG = "PNG";
            public const string TIFF = "TIFF";
            public const string TIFF2 = "TIFF2";
            public const string UNKNOWN = "UNKNOWN";
        }

        public static readonly Dictionary<string, byte[]> DictionaryOfImageFormats =
            new Dictionary<string, byte[]> {
                { ImageFormat.BMP, Encoding.ASCII.GetBytes("BM") },
                { ImageFormat.GIF, Encoding.ASCII.GetBytes("GIF")},
                { ImageFormat.PNG, new byte[] { 137, 80, 78, 71 } },
                { ImageFormat.TIFF, new byte[] { 73, 73, 42 } },
                { ImageFormat.TIFF2, new byte[] { 77, 77, 42 } },
                { ImageFormat.JPEG, new byte[] { 255, 216, 255, 224 } },
                { ImageFormat.JPEG2, new byte[] { 255, 216, 255, 225 } },
                { ImageFormat.JPEG3, new byte[] { 255, 216, 255, 219 } },
                { ImageFormat.JPEG4, new byte[] { 255, 216, 255, 238 } }
    };

        /// <summary>
        /// Verifies if file is an image type.
        /// </summary>
        /// <param name="bytes">File to test.</param>
        /// <param name="imageType">The format of the image.</param>
        /// <returns>If an image or not.</returns>
        internal static bool IsValidImage(this byte[] bytes, out string imageType)
        {
            imageType = ImageFormat.UNKNOWN;
            if (bytes == null)
            {
                return false;
            }

            foreach (var item in DictionaryOfImageFormats)
            {
                if (item.Value.SequenceEqual(bytes.Take(item.Value.Length)))
                {
                    imageType = item.Key;
                    return true;
                }
            }
            return false;
        }

        public static bool IsValidImage(this IFormFile file, out string imageType)
        {

            return file.ToByteArray4().IsValidImage(out imageType);
        }
        private static byte[] ToByteArray4(this IFormFile file)
        {
            if(file == null)
            {
                return null;
            }
            using (MemoryStream memory = new MemoryStream())
            {
                byte[] array4 = new byte[4];
                file.OpenReadStream().Read(array4, 0, 4);
                memory.Read(array4, 0, 4);
                return array4;
            }
        }
    }
}
