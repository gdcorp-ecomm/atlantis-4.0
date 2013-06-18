using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing 
{
    class SpokeLines : ShapeCollection
    {
        public const int DefaultAngleDelta = 10;

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

        private Point _center;

        public Point Center
        {
            get
            {
                return _center;
            }

            set
            {
                _center = value;
            }
        }

        private RandomRange _angleDeltaRange;

        public RandomRange AngleDeltaRange
        {
            get
            {
                return _angleDeltaRange;
            }

            set
            {
                _angleDeltaRange = value;
            }
        }

        public int AngleDelta
        {
            get
            {
                if (null == _angleDeltaRange)
                {
                    return DefaultAngleDelta;
                }

                return _angleDeltaRange.Next;
            }

            set
            {
                if (0 >= value)
                {
                    throw new DrawingException("Angle delta must be a positive number", value);
                }

                _angleDeltaRange = new RandomRange(value);
            }
        }

        public override void Draw(IGraphics graphics, Rectangle bounds)
        {
            if (null == this.Center)
            {
                this.Center = new Point(bounds).Frozen;
            }

            double ro = Circle.MaxRadius(this.Center, bounds);
            for (int phi = 0; phi <= 360; phi += this.AngleDelta)
            {
                double radians = phi * Math.PI / 180;
                int tX = (int)Math.Round(this.Center.X + Math.Cos(radians) * ro);
                int tY = (int)Math.Round(this.Center.Y + Math.Sin(radians) * ro);

                Point endpoint = new Point(tX, tY);
                Line line = new Line(this.Center, endpoint, this.Prototype.Outline);
                line.Transform = this.Prototype.Transform;

                line.Draw(graphics);
                this.Add(line);
            }
        }
    }
}
