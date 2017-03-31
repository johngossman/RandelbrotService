using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public class PixelBuffer
    {
        private int[] pixels;
        int offsetX, offsetY, stride;
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public PixelBuffer(int sizeX, int sizeY)
        {
            this.pixels = new int[sizeX * sizeY];
            this.offsetX = this.offsetY = 0;
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            this.stride = sizeX;
        }
        public PixelBuffer(int[] pixels, int offsetX, int offsetY, int sizeX, int sizeY, int stride)
        {
            if (offsetX + sizeX > stride)
                throw new ArgumentOutOfRangeException("OffsetX + SizeX greater than limits of pixels");
            int maxOffset = offsetY * stride + sizeY * stride + offsetX + sizeX;
            if (maxOffset > pixels.Length)
                throw new ArgumentOutOfRangeException("Offset + Size is greater than limits of pixels");
            this.pixels = pixels;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            this.stride = stride;
        }

        public PixelBuffer(PixelBuffer backing, int offsetX, int offsetY, int sizeX, int sizeY, int stride) :
            this(backing.pixels, offsetX, offsetY, sizeX, sizeY, stride)
        {

        }

        public PixelBuffer Clone()
        {
            var newBuffer = new PixelBuffer(this.SizeX, this.SizeY);
            for (int i = 0; i < pixels.Length; i++)
                newBuffer.pixels[i] = this.pixels[i];
            return newBuffer;
        }

        public void Clear()
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = 0;
            }
        }

        public int GetValue(int x, int y)
        {
            System.Diagnostics.Debug.Assert(x >= 0);
            System.Diagnostics.Debug.Assert(x < this.SizeX);
            System.Diagnostics.Debug.Assert(y >= 0);
            System.Diagnostics.Debug.Assert(y < this.SizeY);
            int offset = (x + this.offsetX) + ((y + this.offsetY) * stride);
            return this.pixels[offset];
        }

        public void SetValue(int x, int y, int value)
        {
            System.Diagnostics.Debug.Assert(x >= 0);
            System.Diagnostics.Debug.Assert(x < this.SizeX);
            System.Diagnostics.Debug.Assert(y >= 0);
            System.Diagnostics.Debug.Assert(y < this.SizeY);
            int offset = (x + this.offsetX) + ((y + this.offsetY) * stride);
            this.pixels[offset] = value;
        }

        public int[] GetPixels()
        {
            return this.pixels;
        }

        // Knowing about bands is a bit of policy leaking into this class...
        // Should be factored in some helper class
        public void ApplyPalette(Palette palette)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                int band = this.pixels[i];
                if (band == -1)
                    this.pixels[i] = palette.GetMaxCountColor();
                else
                    this.pixels[i] = palette.GetColor(band);
            }
        }
    }
}
