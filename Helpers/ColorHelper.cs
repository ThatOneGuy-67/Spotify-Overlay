using System;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpotifyOverlay.Helpers
{
    public static class ColorHelper
    {
        /// <summary>
        /// Converts RGB values to a WPF Color.
        /// </summary>
        public static Color FromRgb(byte r, byte g, byte b)
        {
            return Color.FromRgb(r, g, b);
        }

        /// <summary>
        /// Lightens a color.
        /// </summary>
        public static Color Lighten(Color color, double amount)
        {
            amount = Math.Clamp(amount, 0, 1);

            return Color.FromRgb(
                (byte)(color.R + (255 - color.R) * amount),
                (byte)(color.G + (255 - color.G) * amount),
                (byte)(color.B + (255 - color.B) * amount));
        }

        /// <summary>
        /// Darkens a color.
        /// </summary>
        public static Color Darken(Color color, double amount)
        {
            amount = Math.Clamp(amount, 0, 1);

            return Color.FromRgb(
                (byte)(color.R * (1 - amount)),
                (byte)(color.G * (1 - amount)),
                (byte)(color.B * (1 - amount)));
        }

        /// <summary>
        /// Gets the dominant color from album art.
        /// </summary>
        public static Color GetDominantColor(byte[] imageBytes)
        {
            try
            {
                using var stream = new MemoryStream(imageBytes);

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.DecodePixelWidth = 50;
                bitmap.EndInit();
                bitmap.Freeze();

                var wb = new WriteableBitmap(bitmap);

                int stride = wb.PixelWidth * 4;
                byte[] pixels = new byte[stride * wb.PixelHeight];

                wb.CopyPixels(pixels, stride, 0);

                long r = 0;
                long g = 0;
                long b = 0;
                int count = 0;

                for (int i = 0; i < pixels.Length; i += 4)
                {
                    b += pixels[i];
                    g += pixels[i + 1];
                    r += pixels[i + 2];
                    count++;
                }

                return Color.FromRgb(
                    (byte)(r / count),
                    (byte)(g / count),
                    (byte)(b / count));
            }
            catch
            {
                return Color.FromRgb(29, 185, 84); // Spotify green fallback
            }
        }

        /// <summary>
        /// Creates a SolidColorBrush.
        /// </summary>
        public static SolidColorBrush Brush(Color color)
        {
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            return brush;
        }
    }
}