using System;


namespace Randelbrot
{
    // Used to combine bands of count, making for more pleasing pictures
    // NOTE:  Points inside the set (maxcount) are set to band -1
    // All other bands are mapped to the range 1 -> N
    // 0 is the uninitialized value
    public class BandMap
    {
        protected int[] Values { get; private set; }
        protected int MaxCount { get; private set; }

        private BandMap() {}
        protected BandMap(int maxCount)
        {
            this.MaxCount = maxCount;
            this.Values = new int[maxCount];
        }

        public int Map(int count)
        {
            if (count >= this.MaxCount)
                return -1;
            return this.Values[count];
        }
    }

    // Combines bands logarithmically.  Seems to match the behavior of the set.
    public class LogarithmicBandMap : BandMap
    {
        // To combine more bands, decrease this factor
        private double combinationFactor = 35.0;


        public LogarithmicBandMap(int maxCount, double combinationFactor = 32.0)
            : base(maxCount)
        {
            this.combinationFactor = combinationFactor;
            // Combine bands logarithmically
            for (int i = 0; i < maxCount; i++)
            {
                double temp = Math.Log((double)i + 1) * combinationFactor;
                this.Values[i] = (int)(temp + 1);
            }
            // Now make them consecutive so they map to a Palette nicely
            int bandNumber = 1;
            int lastBand = this.Values[0];
            for (int i = 0; i < maxCount; i++)
            {
                if (lastBand != this.Values[i])
                {
                    bandNumber++;
                    lastBand = this.Values[i];
                }
                this.Values[i] = bandNumber;
            }
        }
    }
}
