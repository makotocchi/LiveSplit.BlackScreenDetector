using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LiveSplit.Mgs3LoadRemover
{
    public class BitmapFeatureDetector
    {
        public ImageFeatures GetFeaturesFromBitmap(Bitmap capture)
        {
            BitmapData bData = capture.LockBits(new Rectangle(0, 0, capture.Width, capture.Height), ImageLockMode.ReadWrite, capture.PixelFormat);
            int bmpStride = bData.Stride;
            int size = bData.Stride * bData.Height;

            byte[] data = new byte[size];

            Marshal.Copy(bData.Scan0, data, 0, size);

            int colorMax = 0;
            int colorMin = 9999;

            for (int xIndex = 0; xIndex < capture.Width; xIndex++)
            {
                for (int yIndex = 0; yIndex < capture.Height; yIndex++)
                {
                    int yAdd = yIndex * bmpStride;

                    //NOTE: while the pixel format is 32ARGB, reading byte-wise results in BGRA.
                    int b = data[(xIndex * 4) + yAdd + 0];
                    int g = data[(xIndex * 4) + yAdd + 1];
                    int r = data[(xIndex * 4) + yAdd + 2];

                    colorMax = Math.Max(b, colorMax);
                    colorMax = Math.Max(g, colorMax);
                    colorMax = Math.Max(r, colorMax);

                    colorMin = Math.Min(b, colorMin);
                    colorMin = Math.Min(g, colorMin);
                    colorMin = Math.Min(r, colorMin);
                }
            }

            capture.UnlockBits(bData);

            return new ImageFeatures(colorMax, colorMin);
        }
    }
}