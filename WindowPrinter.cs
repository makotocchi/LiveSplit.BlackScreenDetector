using LiveSplit.Mgs3LoadRemover.Native;
using System;
using System.Drawing;

namespace LiveSplit.Mgs3LoadRemover
{
    public class WindowPrinter : IBitmapPrinter
    {
        private readonly IntPtr _hwnd;

        public WindowPrinter(IntPtr hwnd)
        {
            _hwnd = hwnd;
        }

        public Bitmap CaptureImage()
        {
            try
            {
                NativeMethods.GetClientRect(_hwnd, out Rectangle windowSize);

                if (windowSize.Width < 0)
                {
                    return new Bitmap(1, 1);
                }

                IntPtr hdcwnd = NativeMethods.GetDC(_hwnd);
                IntPtr hdc = NativeMethods.CreateCompatibleDC(hdcwnd);
                IntPtr hbmp = NativeMethods.CreateCompatibleBitmap(hdcwnd, windowSize.Width, windowSize.Height);
                NativeMethods.SelectObject(hdc, hbmp);

                NativeMethods.BitBlt(hdc, 0, 0, windowSize.Width, windowSize.Height, hdcwnd, 0, 0, NativeEnums.TernaryRasterOperations.SRCCOPY);

                var bitmap = (Bitmap)Image.FromHbitmap(hbmp).Clone();

                NativeMethods.DeleteObject(hbmp);
                NativeMethods.ReleaseDC(_hwnd, hdcwnd);
                NativeMethods.DeleteDC(hdc);

                return bitmap;
            }
            catch
            {
                return new Bitmap(1, 1);
            }
        }
    }
}
