using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing 
{
    class RandomBeziers : RandomNoise
    {
        // shape prototype
        public Bezier Prototype
        {
            get
            {
                if (null == prototype)
                {
                    prototype = new Bezier();
                }

                return prototype as Bezier;
            }
        }

        // scaling definition
        public const int DefaultScalingPercentage = 50;

        private RandomRange _scalingPercentageRange;
        public RandomRange ScalingPercentageRange
        {
            get
            {
                return _scalingPercentageRange;
            }

            set
            {
                _scalingPercentageRange = value;
            }
        }

        public int ScalingPercentage
        {
            get
            {
                if (null == _scalingPercentageRange)
                {
                    return DefaultScalingPercentage;
                }

                return _scalingPercentageRange.Next;
            }

            set
            {
                if (0 >= value)
                {
                    throw new DrawingException("Shape scaling percentage must be a positive number", value);
                }

                _scalingPercentageRange = new RandomRange(value);
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
            // performance optimization - skip the regular shape drawing code
            System.Drawing.Graphics gdiGraphics = (graphics as GdiGraphics).Graphics;

            int targetSurface = (int)Math.Round(this.SurfaceFactor * bounds.Surface);
            int drawnSurface = 0;

            int minWidth = bounds.TopLeft.X;
            int maxWidth = minWidth + bounds.Width;
            int minHeight = bounds.TopLeft.Y;
            int maxHeight = minHeight + bounds.Height;
            LineStyle outline = this.Prototype.Outline;

            int boundsWidth = (int) Math.Round(this.ScalingFactor * maxWidth);
            int boundsHeight = (int) Math.Round(this.ScalingFactor * maxHeight);

            // aproximation of the curve surface with a diagonal line surface
            int surfaceApprox = (int) Math.Round(Math.Sqrt(boundsWidth * boundsWidth + boundsHeight * boundsHeight) * outline.Thickness);

            using (System.Drawing.Pen pen = new System.Drawing.Pen(outline[0][0].Color.GdiColor, outline[0].Thickness))
            {
                while (drawnSurface < targetSurface)
                {
                    int minX = RandomGenerator.Next(minWidth, maxWidth);
                    int minY = RandomGenerator.Next(minHeight, maxHeight);

                    int maxX = minX + boundsWidth;
                    int maxY = minY + boundsHeight;

                    float x1 = RandomGenerator.Next(minX, maxX);
                    float y1 = RandomGenerator.Next(minY, maxY);
                    float x2 = RandomGenerator.Next(minX, maxX);
                    float y2 = RandomGenerator.Next(minY, maxY);
                    float x3 = RandomGenerator.Next(minX, maxX);
                    float y3 = RandomGenerator.Next(minY, maxY);
                    float x4 = RandomGenerator.Next(minX, maxX);
                    float y4 = RandomGenerator.Next(minY, maxY);

                    gdiGraphics.DrawBezier(pen, x1, y1, x2, y2, x3, y3, x4, y4);
                    drawnSurface += surfaceApprox;
                }
            }
        }

        public override int DrawSingleRandomShape(IGraphics graphics, Rectangle bounds)
        {
            Rectangle curveBounds = new Rectangle(bounds, this.ScalingFactor);
            Bezier bezier = new Bezier(curveBounds, this.Prototype.Outline);
            bezier.Transform = this.Prototype.Transform;

            bezier.Draw(graphics);
            this.Add(bezier);

            return bezier.Surface;
        }
    }
}
