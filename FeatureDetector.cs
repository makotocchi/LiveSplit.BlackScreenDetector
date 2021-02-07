using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace LoadDetector
{
    //This class contains settings, features and methods for computing features from a given Bitmap
    internal class FeatureDetector
    {
        //this list of vectors is for 300x100, patchSize = 50, numberOfBins = 16
        //to adapt - if wrongly detected pause, increase match threshold and add wrongly detected runs to list
        public static int[,] listOfFeatureVectorsEng = { { 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

        public static int[,] listOfFeatureVectorsTransition = {
            {2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            { 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

        public static int numberOfBinsCorrect = 330;

        private static readonly float additiveVariance = 2.0f;

        public static int numberOfBins = 16;

        public static int patchSizeX = 50;

        public static int patchSizeY = 50;

        private static readonly float varianceOfBinsAllowedMult = 1.45f;

        public static bool CompareFeatureVector(int[] newVector, int[,] comparison_vectors, out int matchingBins, float percentageOfBinsCorrectOverride = -1.0f)
        {
            int size = newVector.Length;

            if (comparison_vectors.GetLength(1) < size)
            {
                size = comparison_vectors.GetLength(1);
            }

            int numVectors = comparison_vectors.GetLength(0);

            matchingBins = 0;
            int matching = 0;
            int matching_bins_result = 0;
            Parallel.For(0, numVectors, (vectorIndex, loopState) =>
            {
                int tempMatchingBins = 0;
                //check if the current feature vector matches one of the stored ones closely enough

                for (int bin = 0; bin < size; bin++)
                {
                    //Determine upper/lower histogram ranges for matching bins
                    int lower_bound = (int)((comparison_vectors[vectorIndex, bin] / varianceOfBinsAllowedMult) - additiveVariance);
                    int upper_bound = (int)((comparison_vectors[vectorIndex, bin] * varianceOfBinsAllowedMult) + additiveVariance);

                    if (newVector[bin] <= upper_bound && newVector[bin] >= lower_bound)
                    {
                        tempMatchingBins++;
                    }

                    //If we can not get a possible match anymore, break for speed
                    if (((bin - tempMatchingBins) > (size - numberOfBinsCorrect)) && percentageOfBinsCorrectOverride < 0.0f)
                    {
                        break;
                    }
                }

                if (tempMatchingBins >= numberOfBinsCorrect)
                {
                    // if we found enough similarities, we found a match. this will early-out for speed, but possibly report lower matchingBins values
                    // we also swap this comparison index with the first one, to give it priority.
                    Interlocked.Exchange(ref matching, 1);
                    Interlocked.Exchange(ref matching_bins_result, tempMatchingBins);
                    loopState.Stop();
                }
            });

            matchingBins = matching_bins_result;
            if (matching != 0)
                return true;

            if (matchingBins >= numberOfBinsCorrect && percentageOfBinsCorrectOverride < 0.0f)
            {
                //if we found enough similarities, we found a match.
                return true;
            }

            if (percentageOfBinsCorrectOverride >= 0.0f && (matchingBins / (float)size) >= percentageOfBinsCorrectOverride)
            {
                return true;
            }

            return false;
        }

        public static bool CompareFeatureVector(int[] newVector, List<int[]> comparison_vectors, out int matchingBins, float percentageOfBinsCorrectOverride = -1.0f)
        {
            int size = newVector.Length;

            matchingBins = 0;

            if (comparison_vectors.Count == 0)
                return false;

            if (comparison_vectors[0].Length < size)
            {
                size = comparison_vectors[0].Length;
            }

            int numVectors = comparison_vectors.Count;

            int matching = 0;
            int matching_bins_result = 0;
            Parallel.For(0, numVectors, (vectorIndex, loopState) =>
            {
                int tempMatchingBins = 0;
                //check if the current feature vector matches one of the stored ones closely enough

                for (int bin = 0; bin < size; bin++)
                {
                    //Determine upper/lower histogram ranges for matching bins
                    int lower_bound = (int)((comparison_vectors[vectorIndex][bin] / varianceOfBinsAllowedMult) - additiveVariance);
                    int upper_bound = (int)((comparison_vectors[vectorIndex][bin] * varianceOfBinsAllowedMult) + additiveVariance);

                    if (newVector[bin] <= upper_bound && newVector[bin] >= lower_bound)
                    {
                        tempMatchingBins++;
                    }

                    //If we can not get a possible match anymore, break for speed
                    if (((bin - tempMatchingBins) > (size - numberOfBinsCorrect)) && percentageOfBinsCorrectOverride < 0.0f)
                    {
                        break;
                    }
                }

                if (tempMatchingBins >= numberOfBinsCorrect)
                {
                    // if we found enough similarities, we found a match. this will early-out for speed, but possibly report lower matchingBins values
                    // we also swap this comparison index with the first one, to give it priority.
                    Interlocked.Exchange(ref matching, 1);
                    Interlocked.Exchange(ref matching_bins_result, tempMatchingBins);
                    loopState.Stop();
                }
            });

            matchingBins = matching_bins_result;
            if (matching != 0)
                return true;

            if (matchingBins >= numberOfBinsCorrect && percentageOfBinsCorrectOverride < 0.0f)
            {
                //if we found enough similarities, we found a match.
                return true;
            }

            if (percentageOfBinsCorrectOverride >= 0.0f && (matchingBins / (float)size) >= percentageOfBinsCorrectOverride)
            {
                return true;
            }

            return false;
        }

        public static bool IsGameTransition(Bitmap capture, int black_level)
        {
            BitmapData bData = capture.LockBits(new Rectangle(0, 0, capture.Width, capture.Height), ImageLockMode.ReadWrite, capture.PixelFormat);
            int bmpStride = bData.Stride;
            int size = bData.Stride * bData.Height;

            byte[] data = new byte[size];

            /*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
            Marshal.Copy(bData.Scan0, data, 0, size);
            int r = 0;
            int g = 0;
            int b = 0;
            //we look at 50x50 patches and compute histogram bins for the a/r/g/b values.

            int stride = 1; //spacing between feature pixels

            for (int patchX = 0; patchX < (capture.Width / patchSizeX); patchX++)
            {
                for (int patchY = 0; patchY < (capture.Height / patchSizeY); patchY++)
                {
                    int xStart = patchX * patchSizeX * stride;
                    int yStart = patchY * patchSizeX * stride;
                    int xEnd = (patchX + 1) * patchSizeX * stride;
                    int yEnd = (patchY + 1) * patchSizeY * stride;

                    for (int x_index = xStart; x_index < xEnd; x_index += stride)
                    {
                        for (int y_index = yStart; y_index < yEnd; y_index += stride)
                        {
                            int yAdd = y_index * bmpStride;

                            //NOTE: while the pixel format is 32ARGB, reading byte-wise results in BGRA.
                            b += data[(x_index * 4) + yAdd + 0];
                            g += data[(x_index * 4) + yAdd + 1];
                            r += data[(x_index * 4) + yAdd + 2];
                        }
                    }
                }
            }

            capture.UnlockBits(bData);

            b /= capture.Width * capture.Height;
            r /= capture.Width * capture.Height;
            g /= capture.Width * capture.Height;

            return b < black_level && r < black_level && g < black_level;
        }

        private static bool CheckBlackLevelTransition(List<int> max_per_patch, out float average_max_transition, out int matchingBins, int black_level_config = 10)
        {
            int black_level = black_level_config;

            int max_max = 0;

            matchingBins = 0;

            foreach (int max_val in max_per_patch)
            {
                max_max = Math.Max(max_val, max_max);
            }

            average_max_transition = 0.0f;
            // Baseline: If the *maximum* of all pixels is less than the tolerance, we can immediately decide that it is a transition.
            return max_max < black_level;
        }

        private static bool CheckWhiteLevelTransition(List<int> min_per_patch, out float average_max_transition, out int matchingBins, int black_level_config = 10)
        {
            int black_level = black_level_config;

            int min_min = 255;

            matchingBins = 0;

            foreach (int min_val in min_per_patch)
            {
                min_min = Math.Min(min_val, min_min);
            }

            average_max_transition = 0.0f;
            // Baseline: If the *maximum* of all pixels is less than the tolerance, we can immediately decide that it is a transition.
            return min_min > 255 - black_level;
        }

        public static bool CompareFeatureVectorTransitionBlack(List<int> max_per_patch, out float average_max_transition, out int matchingBins, int black_level_config = 10)
        {
            if (CheckBlackLevelTransition(max_per_patch, out average_max_transition, out matchingBins, black_level_config))
                return true;

            return false;
        }

        public static bool CompareFeatureVectorTransitionWhite(List<int> min_per_patch, out float average_max_transition, out int matchingBins, int black_level_config = 10)
        {
            if (CheckWhiteLevelTransition(min_per_patch, out average_max_transition, out matchingBins, black_level_config))
                return true;

            return false;
        }

        public static List<int> FeaturesFromBitmap(Bitmap capture, out List<int> max_per_patch, out int black_level, out List<int> min_per_patch)
        {
            List<int> features = new List<int>();
            max_per_patch = new List<int>();
            min_per_patch = new List<int>();
            black_level = 255;
            BitmapData bData = capture.LockBits(new Rectangle(0, 0, capture.Width, capture.Height), ImageLockMode.ReadWrite, capture.PixelFormat);
            int bmpStride = bData.Stride;
            int size = bData.Stride * bData.Height;

            byte[] data = new byte[size];

            /*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
            Marshal.Copy(bData.Scan0, data, 0, size);
            //we look at 50x50 patches and compute histogram bins for the a/r/g/b values.

            int stride = 1; //spacing between feature pixels

            for (int patchX = 0; patchX < (capture.Width / patchSizeX); patchX++)
            {
                for (int patchY = 0; patchY < (capture.Height / patchSizeY); patchY++)
                {
                    int xStart = patchX * (patchSizeX * stride);
                    int yStart = patchY * (patchSizeX * stride);
                    int xEnd = (patchX + 1) * (patchSizeX * stride);
                    int yEnd = (patchY + 1) * (patchSizeY * stride);

                    int b_max = 0;
                    int g_max = 0;
                    int r_max = 0;
                    int b_min = 9999;
                    int g_min = 9999;
                    int r_min = 9999;

                    for (int x_index = xStart; x_index < xEnd; x_index += stride)
                    {
                        for (int y_index = yStart; y_index < yEnd; y_index += stride)
                        {
                            int yAdd = y_index * bmpStride;

                            //NOTE: while the pixel format is 32ARGB, reading byte-wise results in BGRA.
                            int b = (int)(data[(x_index * 4) + (yAdd) + 0]);
                            int g = (int)(data[(x_index * 4) + (yAdd) + 1]);
                            int r = (int)(data[(x_index * 4) + (yAdd) + 2]);

                            b_max = Math.Max(b, b_max);
                            g_max = Math.Max(g, g_max);
                            r_max = Math.Max(r, r_max);

                            b_min = Math.Min(b, b_min);
                            g_min = Math.Min(g, g_min);
                            r_min = Math.Min(r, r_min);
                        }
                    }

                    max_per_patch.Add(b_max);
                    max_per_patch.Add(g_max);
                    max_per_patch.Add(r_max);

                    min_per_patch.Add(b_min);
                    min_per_patch.Add(g_min);
                    min_per_patch.Add(r_min);
                }
            }

            capture.UnlockBits(bData);

            return features;
        }
    }
}