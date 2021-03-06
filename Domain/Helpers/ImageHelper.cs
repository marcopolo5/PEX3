using System;
using System.IO;

namespace Domain.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
        /// Convert memory image into a binary array
        /// </summary>
        /// <param name="imagePath">Path to the image</param>
        /// <returns>A binary array corresponding to the image</returns>
        public static byte[] GetImageBytes(string imagePath)
        {
            byte[] _imageBytes = null;
            using (var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                _imageBytes = new byte[fileStream.Length];
                _ = fileStream.Read(_imageBytes, 0, Convert.ToInt32(fileStream.Length));
            }
            return _imageBytes;
        }
    }
}
