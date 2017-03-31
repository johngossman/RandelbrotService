using System;

namespace Randelbrot
{
    public abstract class MandelbrotService
    {
        public abstract void RenderToBuffer(MandelbrotSet set, PixelBuffer buffer);
    }

    public class AdaptiveMandelbrotService : MandelbrotService
    {
        private IRenderTracer tracer = null;

        public AdaptiveMandelbrotService()
        {
        }

        public AdaptiveMandelbrotService(IRenderTracer tracer)
        {
            this.tracer = tracer;
        }

        public override void RenderToBuffer(MandelbrotSet set, PixelBuffer buffer)
        {
            var renderer = new ContourRenderer(this.tracer);
            Palette palette = new DefaultPalette();
            int maxCount = set.EstimateMaxCount();
            var bandMap = new LogarithmicBandMap(maxCount, 30.0);

            renderer.Render(buffer, set, bandMap, maxCount);
            buffer.ApplyPalette(palette);
        }
    }
}
