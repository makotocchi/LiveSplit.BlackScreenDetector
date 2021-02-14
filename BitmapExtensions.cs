using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace LiveSplit.Mgs3LoadRemover
{
    internal static class BitmapExtensions
    {
        /// <summary>
        /// Crop the bitmap to the specified rectangle
        /// </summary>
        /// <param name="@this">The bitmap.</param>
        /// <param name="rect">The rectangle that will be used for cropping (size and position).</param>
        /// <returns></returns>
        public static Bitmap Crop(this Bitmap @this, Rectangle rect)
        {
            var nb = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(nb))
            {
                g.DrawImage(@this, -rect.X, -rect.Y);
                return nb;
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="@this">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(this Image @this, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(@this.HorizontalResolution, @this.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(@this, destRect, 0, 0, @this.Width, @this.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}