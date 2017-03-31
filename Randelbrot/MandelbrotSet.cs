using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public class MandelbrotSet
    {
        public DoubleComplexNumber Center { get; private set; }
        public double Side { get; private set; }
        public MandelbrotSet(DoubleComplexNumber center, double side)
        {
            this.Center = center;
            this.Side = side;
        }

        public int EstimateMaxCount()
        {
            double temp = Math.Log(1.0 / Side);
            temp *= temp * 80;
            return (int)temp + 600;
        }
    }
}
