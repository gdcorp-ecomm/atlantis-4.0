using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal class VerticalLines : ShapeCollection
    {
        // shape prototype
        public Line Prototype
        {
            get
            {
                if (null == prototype)
                {
                    prototype = new Line();
                }

                return prototype as Line;
            }
        }

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

        public override void Draw(IGraphics graphics, Rectangle bounds)
        {
            int x = RandomGenerator.Next(0, this.SpacingRange.Min);
            while (x < bounds.Width)
            {
                Point pt1 = new Point(x, bounds.TopLeft.Y);
                Point pt2 = new Point(x, bounds.BottomRight.Y);
                Line line = new Line(pt1, pt2, this.Prototype.Outline);
                line.Transform = this.Prototype.Transform;

                line.Draw(graphics);
                this.Add(line);

                x += this.Spacing;
            }
        }
    }
}
