using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public interface IRenderTracer
    {
        // Add a picture that we may want to save
        void AddBits(string message, PixelBuffer buffer);

        // Dump all the saved pictures
        void Dump();

        // Clear the list of saved pictures
        void Clear();
    }
}
