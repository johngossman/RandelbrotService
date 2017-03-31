using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Randelbrot;

namespace RandelbrotFunctions
{
    public class ImageUtils
    {
        public byte[] PixelBufferToJPEG(PixelBuffer buffer)
        {
            Bitmap bitmap = new Bitmap(buffer.SizeX, buffer.SizeY, PixelFormat.Format32bppArgb);
            Rectangle bounds = new Rectangle(0, 0, buffer.SizeX, buffer.SizeY);

            // Using Lockbits here would be more efficient
            BitmapData bits = bitmap.LockBits(bounds, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            int totalBytes = buffer.SizeX * buffer.SizeY * 4;
            int bitmapBytes = bits.Stride * bits.Height;
            System.Diagnostics.Debug.Assert((totalBytes == bitmapBytes), "Size mismatch in bitmap calculation");
            System.Diagnostics.Debug.Assert(totalBytes == (buffer.GetPixels().Length * 4), "Size mismatch in bitmap calculation");

            System.Runtime.InteropServices.Marshal.Copy(buffer.GetPixels(), 0, bits.Scan0, buffer.GetPixels().Length);
            bitmap.UnlockBits(bits);

            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);

            return stream.GetBuffer();
        }
    }
}