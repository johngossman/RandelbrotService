using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public class MandelbrotSet
    {
        public double CX { get; private set; }
        public double CY { get; private set; }
        public double Side { get; private set; }
        public MandelbrotSet(double cx, double cy, double side)
        {
            this.CX = cx;
            this.CY = cy;
            this.Side = side;
        }

        public int EstimateMaxCount()
        {
            double temp = Math.Log(1.0 / Side);
            temp *= temp * 80;
            return (int)temp + 600;
        }

        public static int CalculateCount(double cx, double cy, int maxCount)
        {
            double tx = cx;
            double ty = cy;
            double x2, y2;
            int count = 0;

            x2 = tx * tx; y2 = ty * ty;

            while ((x2 + y2 < 4.0) && (count < maxCount))
            {
                ty = 2 * tx * ty + cy;
                tx = x2 - y2 + cx;
                x2 = tx * tx;
                y2 = ty * ty;
                count++;
            }
            return (count);
        }
    }
}
