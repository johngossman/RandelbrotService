using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public abstract class Renderer
    {
        private IRenderTracer tracer = null;

        public Renderer(IRenderTracer tracer)
        {
            this.tracer = tracer;
        }

        abstract public void Render(PixelBuffer buffer, MandelbrotSet set, BandMap bandMap, int maxCount);

        protected virtual void SetBand(PixelBuffer buffer, BandMap bandMap, int x, int y, int count)
        {
            int band = bandMap.Map(count);
            buffer.SetValue(x, y, band);
        }

        protected double[] YCoordinates { get; private set; }
        protected double[] XCoordinates { get; private set; }
        protected virtual void InitializeCoordinateMap(int sizex, int sizey, MandelbrotSet set)
        {
            this.XCoordinates = new double[sizex];
            this.YCoordinates = new double[sizey];

            DoubleComplexNumber center = (DoubleComplexNumber)set.Center;
            double size = set.Side;
            double gapX = size / sizex;
            double gapY = size / sizey;
            double gap = Math.Min(gapX, gapY);

            double x = center.X - ((gap * sizex) / 2.0);
            double y = center.Y - ((gap * sizey) / 2.0);
            for (int i = 0; i < sizex; i++)
            {
                this.XCoordinates[i] = x;
                x += gap;
            }
            for (int i = 0; i < sizey; i++)
            {
                this.YCoordinates[i] = y;
                y += gap;
            }
        }

        [System.Diagnostics.ConditionalAttribute("DEBUG")]
        public void SaveBits(string message, PixelBuffer buffer)
        {
            if (this.tracer != null)
            {
                this.tracer.AddBits(message, buffer);
            }
        }

        [System.Diagnostics.ConditionalAttribute("DEBUG")]
        public void DumpBits()
        {
            if (this.tracer != null)
            {
                this.tracer.Dump();
            }
        }

        [System.Diagnostics.ConditionalAttribute("DEBUG")]
        public void ClearSavedBits()
        {
            if (this.tracer != null)
            {
                this.tracer.Clear();
            }
        }
    }

    public class DoubleRenderer : Renderer
    {
        public DoubleRenderer() 
            : base(null)
        {
        }

        public DoubleRenderer(IRenderTracer tracer)
            : base(tracer)
        {
        }

        override public void Render(PixelBuffer buffer, MandelbrotSet set, BandMap bandMap, int maxCount)
        {
            this.InitializeCoordinateMap(buffer.SizeX, buffer.SizeY, set);

            DoubleComplexNumber temp = new DoubleComplexNumber(0.0, 0.0);
            for (int i = 0; i < buffer.SizeX; i++)
            {
                temp.X = this.XCoordinates[i];
                for (int j = 0; j < buffer.SizeY; j++)
                {
                    temp.Y = this.YCoordinates[j];
                    int count = temp.CalculateCount(maxCount);
                    this.SetBand(buffer, bandMap, i, j, count);
                }
            }
        }
    }
}
