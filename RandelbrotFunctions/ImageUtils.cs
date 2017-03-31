using System;
using System.IO;

using BitMiracle.LibJpeg;

using Randelbrot;

namespace RandelbrotFunctions
{
    public class ImageUtils
    {
        public byte[] PixelBufferToJPEG(PixelBuffer buffer)
        {
            // RGB -- 3 components per pixel
            const int components = 3;

            byte[] rgbBits = this.bufferToRGB(buffer);

            int stride = buffer.SizeX * components; 
            byte[] rowBits = new byte[stride];

            SampleRow[] rows = new SampleRow[buffer.SizeY];
            for(int i = 0; i < buffer.SizeY; i++)
            {
                Buffer.BlockCopy(rgbBits, i * stride, rowBits, 0, stride);
                rows[i] = new SampleRow(rowBits, buffer.SizeX, 8, components);
            }

            JpegImage image = new JpegImage(rows, Colorspace.RGB);

            MemoryStream stream = new MemoryStream();
            image.WriteJpeg(stream);

            return stream.GetBuffer();
        }

        private byte[] bufferToRGB(PixelBuffer buffer)
        {
            // RGB == 3 components per pixel
            byte[] rgbBits = new byte[buffer.SizeX * buffer.SizeY * 3];

            int[] pixels = buffer.GetPixels(); // In ARGB format
            for(int i = 0; i < pixels.Length; i++)
            {
                int offset = i * 3;
                rgbBits[offset] = (byte)(byte)((pixels[i] >> 16) & 0xff);   // red
                rgbBits[offset + 1] = (byte)((pixels[i] >> 8) & 0xff);  // green
                rgbBits[offset + 2] = (byte)(pixels[i] & 0xff); // blue
            }

            return rgbBits;
        }
    }
}