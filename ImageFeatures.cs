namespace LiveSplit.Mgs3LoadRemover
{
    public class ImageFeatures
    {
        public int MaxColorIntensity { get; }
        public int MinColorIntensity { get; }

        public ImageFeatures(int maxColorIntensity, int minColorIntensity)
        {
            MaxColorIntensity = maxColorIntensity;
            MinColorIntensity = minColorIntensity;
        }
    }
}