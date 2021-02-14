using System.Drawing;
using System.Windows.Forms;

namespace LiveSplit.Mgs3LoadRemover
{
    public class ScreenPrinter : IBitmapPrinter
    {
        private readonly Screen _screen;

        public ScreenPrinter(Screen screen)
        {
            _screen = screen;
        }

        public Bitmap CaptureImage()
        {
            try
            {
                var bitmap = new Bitmap(_screen.Bounds.Width, _screen.Bounds.Height);

                //Full screen capture
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(_screen.Bounds.X, _screen.Bounds.Y, 0, 0, _screen.Bounds.Size, CopyPixelOperation.SourceCopy);
                }

                return bitmap;
            }
            catch
            {
                return new Bitmap(1, 1);
            }
        }
    }
}
