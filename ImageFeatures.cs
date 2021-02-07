using System;
using System.Collections.Generic;

namespace LiveSplit.BlackScreenDetector
{
    public class ImageFeatures
    {
        public ImageFeatures(IEnumerable<int> maxColorIntensityPerPatch, IEnumerable<int> minColorIntensityPerPatch)
        {
            MaxColorIntensityPerPatch = maxColorIntensityPerPatch ?? throw new ArgumentNullException(nameof(maxColorIntensityPerPatch));
            MinColorIntensityPerPatch = minColorIntensityPerPatch ?? throw new ArgumentNullException(nameof(minColorIntensityPerPatch));
        }

        public IEnumerable<int> MaxColorIntensityPerPatch { get; }
        public IEnumerable<int> MinColorIntensityPerPatch { get; }
    }
}