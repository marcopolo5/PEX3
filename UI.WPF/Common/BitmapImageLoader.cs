using System.IO;
using System.Windows.Media.Imaging;

namespace UI.WPF.Common
{
    public static class BitmapImageLoader
    {
        /// <summary>
        /// Convert binary array into a BitmapImage
        /// </summary>
        /// <param name="imageData">The binary array with the image data</param>
        /// <returns>BitmapImage corresponding to the binary array given</returns>
        public static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var bi_image = new BitmapImage();
            using (var memoryStream = new MemoryStream(imageData))
            {
                memoryStream.Position = 0;
                bi_image.BeginInit();
                bi_image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bi_image.CacheOption = BitmapCacheOption.OnLoad;
                bi_image.UriSource = null;
                bi_image.StreamSource = memoryStream;
                bi_image.EndInit();
            }
            bi_image.Freeze();
            return bi_image;
        }
    }
}
