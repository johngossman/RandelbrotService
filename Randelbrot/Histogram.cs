using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public class Histogram
    {
        private Dictionary<int, int> values = new Dictionary<int,int>();
        public Histogram()
        {
            this.values[0] = 0;
        }

        public void Evaluate(PixelBuffer buffer)
        {
            for (int i = 0; i < buffer.SizeX; i++)
            {
                for (int j = 0; j < buffer.SizeY; j++)
                {
                    int val = buffer.GetValue(i, j);
                    int count = 0;
                    this.values.TryGetValue(val, out count);
                    count++;
                    this.values[val] = count;
                }
            }
        }

        public int GetValue(int val)
        {
            int retval = 0;
            this.values.TryGetValue(val, out retval);
            return retval;
        }

        public int NumberOfValues
        {
            get
            {
                return this.values.Count;
            }
        }
    }
}
