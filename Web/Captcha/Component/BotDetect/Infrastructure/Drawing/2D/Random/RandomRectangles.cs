using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing 
{
    class RandomRectangles : RandomNoise
    {
        // shape prototype
        public Rectangle Prototype
        {
            get
            {
                if (null == prototype)
                {
                    prototype = new Rectangle();
                }

                return prototype as Rectangle;
            }
        }

        // scaling definition
        public const int DefaultScalingPercentage = 50;

        protected RandomRange scalingPercentageRange;
        public RandomRange ScalingPercentageRange
        {
            get
            {
                return scalingPercentageRange;
            }

            set
            {
                scalingPercentageRange = value;
            }
        }

        public int ScalingPercentage
        {
            get
            {
                if (null == scalingPercentageRange)
                {
                    return DefaultScalingPercentage;
                }

                return scalingPercentageRange.Next;
            }

            set
            {
                if (0 >= value)
                {
                    throw new DrawingException("Shape scaling percentage must be a positive number", value);
                }

                scalingPercentageRange = new RandomRange(value);
            }
        }

        public double ScalingFactor
        {
            get
            {
                return (this.ScalingPercentage / 100.00);
            }
        }

        public override void DrawFast(IGraphics graphics, Rectangle bounds)
        {
            //  performance optimization - skip the regular shape drawing code
            System.Drawing.Graphics gdiGraphics = (graphics as GdiGraphics).Graphics;

            int targetSurface = (int)Math.Round(this.SurfaceFactor * bounds.Surface);
            int drawnSurface = 0;

            int minX = bounds.TopLeft.X;
            int maxX = bounds.BottomRight.X;
            int minY = bounds.TopLeft.Y;
            int maxY = bounds.BottomRight.Y;

            int drawnWidth = (int)Math.Round(this.ScalingFactor * bounds.Width);
            if (0 == drawnWidth) { drawnWidth = 1; }
            int drawnHeight = (int)Math.Round(this.ScalingFactor * bounds.Height);
            if (0 == drawnHeight) { drawnHeight = 1; }
            int individualSurface = drawnWidth * drawnHeight;

            if (!this.Prototype.FillColor.IsRandomized)
            {
                // fixed color, we can use a single brush
                using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(this.Prototype.FillColor.GdiColor))
                {
                    while (drawnSurface < targetSurface)
                    {
                        int x = RandomGenerator.Next(minX, maxX - drawnWidth);
                        int y = RandomGenerator.Next(minY, maxY - drawnHeight);

                        // randomly vertical or horizontal rectangles
                        if (0 == RandomGenerator.Next(2))
                        {
                            gdiGraphics.FillRectangle(brush, x, y, drawnWidth, drawnHeight);
                        }
                        else
                        {
                            gdiGraphics.FillRectangle(brush, x, y, drawnHeight, drawnWidth);
                        }

                        drawnSurface += individualSurface;
                    }
                }
            }
            else
            {
                // randomized color, we need multiple brushes
                while (drawnSurface < targetSurface)
                {
                    int x = RandomGenerator.Next(minX, maxX /*- drawnWidth / 2*/);
                    int y = RandomGenerator.Next(minY, maxY - drawnHeight);

                    using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(this.Prototype.FillColor.GdiColor))
                    {
                        // randomly vertical or horizontal rectangles
                        if (0 == RandomGenerator.Next(2))
                        {
                            gdiGraphics.FillRectangle(brush, x, y, drawnWidth, drawnHeight);
                        }
                        else
                        {
                            gdiGraphics.FillRectangle(brush, x, y, drawnHeight, drawnWidth);
                        }
                    }

                    drawnSurface += individualSurface;
                }
            }
        }

        public override int DrawSingleRandomShape(IGraphics graphics, Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds, this.ScalingFactor);
            r.FillColor = this.Prototype.FillColor;
            r.Outline = this.Prototype.Outline;
            r.Transform = this.Prototype.Transform;

            r.Draw(graphics);
            this.Add(r);

            return r.Surface;
        }
    }
}
