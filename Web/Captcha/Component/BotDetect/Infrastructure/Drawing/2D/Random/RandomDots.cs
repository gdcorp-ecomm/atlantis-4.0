using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing 
{
    class RandomDots : RandomNoise
    {
        // shape prototype
        public Point Prototype
        {
            get
            {
                if (null == prototype)
                {
                    prototype = new Point();
                }

                return prototype as Point;
            }
        }

        public override void DrawFast(IGraphics graphics, Rectangle bounds)
        {
            // performance optimization - skip the regular shape drawing code
            System.Drawing.Graphics gdiGraphics = (graphics as GdiGraphics).Graphics;

            int targetSurface = (int)Math.Round(this.SurfaceFactor * bounds.Surface);
            int drawnSurface = 0;

            int minWidth = bounds.TopLeft.X;
            int maxWidth = minWidth + bounds.Width;
            int minHeight = bounds.TopLeft.Y;
            int maxHeight = minHeight + bounds.Height;
            Color fillColor = this.Prototype.FillColor;

            using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(fillColor.GdiColor))
            {
                while (drawnSurface < targetSurface)
                {
                    int x = RandomGenerator.Next(minWidth, maxWidth);
                    int y = RandomGenerator.Next(minHeight, maxHeight);
                    gdiGraphics.FillRectangle(brush, x, y, 1, 1);
                    drawnSurface += 1;
                }
            }
        }

        public override int DrawSingleRandomShape(IGraphics graphics, Rectangle bounds)
        {
            Point pt = new Point(bounds);
            pt.FillColor = this.Prototype.FillColor;

            pt.Draw(graphics);
            this.Add(pt);

            return pt.Surface;
        }
    }
}
