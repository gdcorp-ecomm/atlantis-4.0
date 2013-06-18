using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal abstract class RandomNoise : ShapeCollection
    {
        public const int DefaultSurfacePercentage = 50;

        protected RandomRange surfacePercentageRange;
        public RandomRange SurfacePercentageRange
        {
            get
            {
                return surfacePercentageRange;
            }

            set
            {
                surfacePercentageRange = value;
            }
        }

        public int SurfacePercentage
        {
            get
            {
                if (null == surfacePercentageRange)
                {
                    return DefaultSurfacePercentage;
                }

                return surfacePercentageRange.Next;
            }

            set
            {
                if (0 >= value || 1001 <= value)
                {
                    throw new DrawingException("SurfacePercentage for noises must be between 1 and 1000", value);
                }

                surfacePercentageRange = new RandomRange(value);
            }
        }

        public double SurfaceFactor
        {
            get
            {
                return (this.SurfacePercentage / 100.00);
            }
        }

        public abstract int DrawSingleRandomShape(IGraphics graphics, Rectangle bounds);

        public override void Draw(IGraphics graphics, Rectangle bounds)
        {
            int targetSurface = (int) Math.Round(this.SurfaceFactor * bounds.Surface);
            int drawnSurface = 0;
            while (drawnSurface < targetSurface)
            {
                drawnSurface += this.DrawSingleRandomShape(graphics, bounds);
            }
        }
    }
}
