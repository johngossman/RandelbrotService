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
            int stride = buffer.SizeX * sizeof(int);
            byte[] bytes = new byte[stride];
            Buffer.BlockCopy(buffer.GetPixels(), 0, bytes, 0, bytes.Length);

            SampleRow[] rows = new SampleRow[buffer.SizeY];
            for(int i = 0; i < buffer.SizeY; i++)
            {
                Buffer.BlockCopy(buffer.GetPixels(), i * stride, bytes, 0, stride);
                rows[i] = new SampleRow(bytes, buffer.SizeX, 8, 4);
            }

            JpegImage image = new JpegImage(rows, Colorspace.CMYK);

            MemoryStream stream = new MemoryStream();
            image.WriteJpeg(stream);

            return stream.GetBuffer();
        }
    }
}