namespace LiveSplit.Mgs3LoadRemover
{
    public class ColorDetector
    {
        private readonly int _colorTolerance;

        public ColorDetector(int colorTolerance) => _colorTolerance = colorTolerance;
        public bool IsBlackImage(ImageFeatures features) => features.MaxColorIntensity < _colorTolerance;
        public bool IsWhiteImage(ImageFeatures features) => features.MinColorIntensity > 255 - _colorTolerance;
    }
}
