using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal class HorizontalLines : ShapeCollection
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
            int y = RandomGenerator.Next(0, this.SpacingRange.Min);
            while (y < bounds.Height)
            {
                Point pt1 = new Point(bounds.TopLeft.X, y);
                Point pt2 = new Point(bounds.BottomRight.X, y);
                Line line = new Line(pt1, pt2, this.Prototype.Outline);
                line.Transform = this.Prototype.Transform;

                line.Draw(graphics);
                this.Add(line);

                y += this.Spacing;
            }
        }
    }
}
