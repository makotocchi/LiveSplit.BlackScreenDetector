using LiveSplit.BlackScreenDetector;
using LiveSplit.BlackScreenDetector.Native;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace LoadDetector
{
    public struct ImageCaptureInfo
    {
        public float FeatureVectorResolutionX;
        public float FeatureVectorResolutionY;
        public int CaptureSizeX;
        public int CaptureSizeY;
        public float CropOffsetX;
        public float CropOffsetY;
        public float CaptureAspectRatio;
        public float FrameCenterX;
        public float FrameCenterY;
        public float ActualOffsetX;
        public float ActualOffsetY;
        public float ActualCropSizeX;
        public float ActualCropSizeY;
        public float CropCoordinateLeft;
        public float CropCoordinateRight;
        public float CropCoordinateTop;
        public float CropCoordinateBottom;
    }

    //This class contains utilities for capturing from the screen / from a given window.
    internal class ImageCapture
    {
        //Should probably refactor this into some kind of struct, whatever...
        public static void SizeAdjustedCropAndOffset(int deviceWidth, int deviceHeight, ref ImageCaptureInfo info)
        {
            var resolution_factor_x = deviceWidth / info.FeatureVectorResolutionX;

            var resolution_factor_y = deviceHeight / info.FeatureVectorResolutionY;

            info.ActualCropSizeX = info.CaptureSizeX * resolution_factor_x;

            info.ActualCropSizeY = info.CaptureSizeY * resolution_factor_y;

            //Scale offset depending on resolution
            info.ActualOffsetX = info.CropOffsetX * resolution_factor_x;

            info.ActualOffsetY = info.CropOffsetY * resolution_factor_y;

            //Scale offset and sizes depending on actual vs. needed aspect ratio

            info.FrameCenterX = deviceWidth / 2;

            info.FrameCenterY = deviceHeight / 2;
        }

        public static Bitmap CaptureFromDisplay(ref ImageCaptureInfo info)
        {
            var b = new Bitmap((int)info.ActualCropSizeX, (int)info.ActualCropSizeY);

            //Full screen capture
            using (Graphics g = Graphics.FromImage(b))
            {
                g.CopyFromScreen((int)(info.FrameCenterX - info.ActualCropSizeX / 2 + info.ActualOffsetX),
                (int)(info.FrameCenterY - info.ActualCropSizeY / 2 + info.ActualOffsetY), 0, 0, new Size((int)info.ActualCropSizeX, (int)info.ActualCropSizeY), CopyPixelOperation.SourceCopy);
            }

            return b.ResizeImage(info.CaptureSizeX, info.CaptureSizeY);
        }

        public static Bitmap PrintWindow(IntPtr hwnd, ref ImageCaptureInfo info, bool full = false, bool useCrop = false, float scalingValueFloat = 1.0f)
        {
            try
            {
                NativeMethods.GetClientRect(hwnd, out Rectangle rc);

                if (rc.Width < 0)
                    return new Bitmap(1, 1);

                IntPtr hdcwnd = NativeMethods.GetDC(hwnd);
                IntPtr hdc = NativeMethods.CreateCompatibleDC(hdcwnd);

                rc.Width = (int)(rc.Width * scalingValueFloat);
                rc.Height = (int)(rc.Height * scalingValueFloat);

                if (useCrop)
                {
                    //Change size according to selected crop
                    rc.Width = (int)(info.CropCoordinateRight - info.CropCoordinateLeft);
                    rc.Height = (int)(info.CropCoordinateBottom - info.CropCoordinateTop);
                }

                //Compute crop coordinates and width/ height based on resoution
                SizeAdjustedCropAndOffset(rc.Width, rc.Height, ref info);

                float cropOffsetX = info.ActualOffsetX;
                float cropOffsetY = info.ActualOffsetY;

                if (full)
                {
                    info.ActualOffsetX = 0;
                    info.ActualOffsetY = 0;

                    info.ActualCropSizeX = 2 * info.FrameCenterX;
                    info.ActualCropSizeY = 2 * info.FrameCenterY;
                }

                if (useCrop)
                {
                    //Adjust for crop offset
                    info.FrameCenterX += info.CropCoordinateLeft;
                    info.FrameCenterY += info.CropCoordinateTop;
                }

                IntPtr hbmp = NativeMethods.CreateCompatibleBitmap(hdcwnd, (int)info.ActualCropSizeX, (int)info.ActualCropSizeY);

                NativeMethods.SelectObject(hdc, hbmp);

                NativeMethods.BitBlt(hdc, 0, 0, (int)info.ActualCropSizeX, (int)info.ActualCropSizeY, hdcwnd, (int)(info.FrameCenterX + info.ActualOffsetX - info.ActualCropSizeX / 2),
                    (int)(info.FrameCenterY + info.ActualOffsetY - info.ActualCropSizeY / 2), NativeEnums.TernaryRasterOperations.SRCCOPY);

                info.ActualOffsetX = cropOffsetX;
                info.ActualOffsetY = cropOffsetY;

                var ret = (Bitmap)Image.FromHbitmap(hbmp).Clone();

                NativeMethods.DeleteObject(hbmp);
                NativeMethods.ReleaseDC(hwnd, hdcwnd);
                NativeMethods.DeleteDC(hdc);

                if (ret.Width == 1280 && ret.Height == 720)
                {
                    return ret;
                }

                return ret.ResizeImage(1280, 720);
            }
            catch (ExternalException)
            {
                return new Bitmap(10, 10);
            }
        }
    }
}