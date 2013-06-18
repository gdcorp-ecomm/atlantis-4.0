using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing 
{
    class RandomCircles : RandomNoise
    {
        public const int DefaultRadius = 5;

        // shape prototype
        public Circle Prototype
        {
            get
            {
                if (null == prototype)
                {
                    prototype = new Circle();
                }

                return prototype as Circle;
            }
        }

        public override void DrawFast(IGraphics graphics, Rectangle bounds)
        {
            //  performance optimization - skip the regular shape drawing code
            System.Drawing.Graphics gdiGraphics = (graphics as GdiGraphics).Graphics;

            int targetSurface = (int)Math.Round(this.SurfaceFactor * bounds.Surface);
            int drawnSurface = 0;

            int minWidth = bounds.TopLeft.X;
            int maxWidth = minWidth + bounds.Width;
            int minHeight = bounds.TopLeft.Y;
            int maxHeight = minHeight + bounds.Height;

            if (!this.Prototype.FillColor.IsRandomized)
            {
                // fixed color, we can use a single brush
                using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(this.Prototype.FillColor.GdiColor))
                {
                    while (drawnSurface < targetSurface)
                    {
                        int x = RandomGenerator.Next(minWidth, maxWidth);
                        int y = RandomGenerator.Next(minHeight, maxHeight);
                        int r = this.Prototype.Radius;

                        gdiGraphics.FillEllipse(brush, new System.Drawing.Rectangle(x - r, y - r, 2 * r, 2 * r));
                        drawnSurface += (int)Math.Round(Math.Pow(r, 2) * Math.PI);
                    }
                }
            }
            else
            {
                // randomized color, we need multiple brushes
                while (drawnSurface < targetSurface)
                {
                    int x = RandomGenerator.Next(0, maxWidth);
                    int y = RandomGenerator.Next(0, maxHeight);
                    int r = this.Prototype.Radius;

                    using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(this.Prototype.FillColor.GdiColor))
                    {
                        gdiGraphics.FillEllipse(brush, new System.Drawing.Rectangle(x - r, y - r, 2 * r, 2 * r));
                    }

                    drawnSurface += (int)Math.Round(Math.Pow(r, 2) * Math.PI);
                }
            }
        }

        public override int DrawSingleRandomShape(IGraphics graphics, Rectangle bounds)
        {
            Point pt = new Point(bounds);
            Circle c = new Circle(pt, this.Prototype.Radius);
            c.FillColor = this.Prototype.FillColor;
            c.Outline = this.Prototype.Outline;
            c.Transform = this.Prototype.Transform;

            c.Draw(graphics);
            this.Add(c);

            return c.Surface;
        }
    }
}
