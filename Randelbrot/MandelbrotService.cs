using System;

namespace Randelbrot
{

    public class MandelbrotRenderer 
    {
        private IRenderTracer tracer = null;
        Palette palette = new DefaultPalette();

        public MandelbrotRenderer()
        {
        }

        public MandelbrotRenderer(IRenderTracer tracer)
        {
            this.tracer = tracer;
        }

        public void RenderToBuffer(MandelbrotSet set, PixelBuffer buffer)
        {
            var renderer = new ContourRenderer(this.tracer);

            int maxCount = set.EstimateMaxCount();
            var bandMap = new LogarithmicBandMap(maxCount, 30.0);

            renderer.Render(buffer, set, bandMap, maxCount);
            buffer.ApplyPalette(this.palette);
        }
    }
}
