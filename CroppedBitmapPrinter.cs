using System.Drawing;

namespace LiveSplit.Mgs3LoadRemover
{
    public class CroppedBitmapPrinter : IBitmapPrinter
    {
        private readonly IBitmapPrinter _bitmapPrinter;
        private readonly Rectangle _cropSettings;

        public CroppedBitmapPrinter(IBitmapPrinter bitmapPrinter, Rectangle cropSettings)
        {
            _bitmapPrinter = bitmapPrinter;
            _cropSettings = cropSettings;
        }

        public Bitmap CaptureImage()
        {

            Bitmap bitmap = _bitmapPrinter.CaptureImage();

            if (bitmap.Width != _cropSettings.Width || bitmap.Width != _cropSettings.Width)
            {
                return bitmap.Crop(_cropSettings);
            }

            return bitmap;
        }
    }
}
