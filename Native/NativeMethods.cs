using System;
using System.Drawing;
using System.Runtime.InteropServices;
using static LiveSplit.BlackScreenDetector.Native.NativeEnums;

namespace LiveSplit.BlackScreenDetector.Native
{
    internal class NativeMethods
    {
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr DC, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr SrcDC, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
    }
}