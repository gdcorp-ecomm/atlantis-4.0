using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    class ConcentricCircles : ShapeCollection
    {
        // spacing definition
        public const int DefaultSpacing = 5;

        private RandomRange _spacingRange;
        public RandomRange SpacingRange
        {
            get
            {
                return _spacingRange;
            }

            set
            {
                _spacingRange = value;
            }
        }

        public int Spacing
        {
            get
            {
                if (null == _spacingRange)
                {
                    return DefaultSpacing;
                }

                return _spacingRange.Next;
            }

            set
            {
                if (0 >= value)
                {
                    throw new DrawingException("Shape spacing must be a positive number", value);
                }

                _spacingRange = new RandomRange(value);
            }
        }

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
            // performance optimization - skip the regular shape drawing code
            System.Drawing.Graphics gdiGraphics = (graphics as GdiGraphics).Graphics;

            int maxWidth = bounds.Width;
            int maxHeight = bounds.Height;

            // frozen center only!
            int x, y;
            if (null != this.Prototype.Center)
            {
                x = this.Prototype.Center.X;
                y = this.Prototype.Center.Y;
            }
            else
            {
                x = RandomGenerator.Next(0, maxWidth);
                y = RandomGenerator.Next(0, maxHeight);
            }

            Color outlineColor = this.Prototype.Outline[0][0].Color;
            int t = this.Prototype.Outline[0].Thickness;

            int maxRadius = Circle.MaxRadius(new Point(x, y), bounds);
            int spacing = this.Spacing;

            // fixed color, we can use a single brush
            using (System.Drawing.Pen pen = new System.Drawing.Pen(outlineColor.GdiColor, t))
            {
                for (int r = 1; r <= maxRadius; r += spacing)
                {
                    gdiGraphics.DrawEllipse(pen, new System.Drawing.Rectangle(x - r, y - r, 2 * r, 2 * r));
                }
            }
        }

        public override void Draw(IGraphics graphics, Rectangle bounds)
        {
            if (null == this.Prototype.Center)
            {
                this.Prototype.Center = new Point(bounds).Frozen;
            }

            int maxRadius = Circle.MaxRadius(this.Prototype.Center, bounds);
            for(int radius = 1; radius<= maxRadius; radius += this.Spacing)
            {
                Circle c = new Circle(this.Prototype.Center, radius);
                c.Outline = this.Prototype.Outline;
                c.Transform = this.Prototype.Transform;

                c.Draw(graphics);
                this.Add(c);
            }
        }
    }
}
