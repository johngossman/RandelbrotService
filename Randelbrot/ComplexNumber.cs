using System;
using System.Collections.Generic;

namespace Randelbrot
{
    // Specific implementation for double based math
    public class DoubleComplexNumber 
    {
        public double X { get; set; }
        public double Y { get; set; }
        public DoubleComplexNumber(double x, double y)
        {
            this.X = x; this.Y = y;
        }
        public DoubleComplexNumber(DoubleComplexNumber other)
        {
            this.X = other.X; this.Y = other.Y;
        }

        public void Add(DoubleComplexNumber other)
        {
            this.X += other.X;
            this.Y += other.Y;
        }

        public int CalculateCount(int maxCount)
        {
            double tx = this.X;
            double ty = this.Y;
            double x2, y2;
            int count = 0;

            x2 = tx * tx; y2 = ty * ty;

            while ((x2 + y2 < 4.0) && (count < maxCount))
            {
                ty = 2 * tx * ty + this.Y;
                tx = x2 - y2 + this.X;
                x2 = tx * tx;
                y2 = ty * ty;
                count++;
            }
            return (count);
        }
    }   
}
