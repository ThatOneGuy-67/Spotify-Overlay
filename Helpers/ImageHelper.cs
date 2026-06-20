using System.IO;
using System.Windows.Media.Imaging;

namespace SpotifyOverlay.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
        /// Converts a byte array into a BitmapImage.
        /// </summary>
        public static BitmapImage? FromBytes(byte[]? imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return null;

            using var stream = new MemoryStream(imageBytes);

            var image = new BitmapImage();

            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();

            return image;
        }

        /// <summary>
        /// Loads an image from a file.
        /// </summary>
        public static BitmapImage? FromFile(string path)
        {
            if (!File.Exists(path))
                return null;

            var image = new BitmapImage();

            image.BeginInit();
            image.UriSource = new System.Uri(path, System.UriKind.RelativeOrAbsolute);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            image.Freeze();

            return image;
        }

        /// <summary>
        /// Saves album art to a file.
        /// </summary>
        public static void Save(byte[] imageBytes, string path)
        {
            File.WriteAllBytes(path, imageBytes);
        }
    }
}