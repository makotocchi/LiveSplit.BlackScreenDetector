using System.Drawing;

namespace LiveSplit.Mgs3LoadRemover
{
    public class DummyPrinter : IBitmapPrinter
    {
        public Bitmap CaptureImage()
        {
            return new Bitmap(1, 1);
        }
    }
}
