using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BotDetect.Drawing
{
    internal interface IGraphics : IDisposable
    {
        // image output
        MemoryStream GetStream(ImageFormat format);

        // basic dimensions and related
        int Width { get; }
        int Height { get; }
        int Surface { get; }
        double ScalingFactor { get; }

        // various bounds
        Rectangle Bounds { get; }
        Rectangle VisibleBounds { get; }
        ShapeCollection Clip { get; set; }

        // pixel manipulation
        void LockPixels();
        void SetPixel(int x1, int y1, Color value);
        void SwapPixels(int x1, int y1, int x2, int y2);
        void UnlockPixels();

        // individual pixels
        //PixelData this[int x, int y] { get; set; }

        // flood-fill
        void Fill(Color backColor);
        void Fill(Color backColor, ShapeCollection clip);

        // easy copying utility method
        IGraphics Clone();
    }
}
