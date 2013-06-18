using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace BotDetect.Drawing
{
    internal class Circle : AtomicShape
    {
        private RandomRange _radiusRange;

        public RandomRange RadiusRange
        {
            get
            {
                return _radiusRange;
            }

            set
            {
                _radiusRange = value;
            }
        }

        public int Radius
        {
            get
            {
                if (null == _radiusRange)
                {
                    return 0;
                }

                return _radiusRange.Next;
            }
            set
            {
                if (value <= 0)
                {
                    throw new DrawingException("The radius of a circle has to be a positive integer", value);
                }

                _radiusRange = new RandomRange(value);
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

        public Circle(Point center, int radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public Circle(Point center, int minRadius, int maxRadius)
        {
            this.Center = center;
            this.RadiusRange = new RandomRange(minRadius, maxRadius);
        }

        public Circle()
        {
        }

        public override int Surface
        {
            get 
            {
                // circle interior + circle outline
                int r = this.Radius + this.Outline.Thickness;
                double surface = Math.Pow(r, 2) * Math.PI;

                return (int)Math.Round(surface);
            }
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Circle: {0}, {1}", _center, _radiusRange);
        }

        /// <summary>
        /// calculate the circle radius required to fit the whole rectangle inside
        /// </summary>
        /// <param name="center">the center of the circle</param>
        /// <param name="bounds">the rectangle to be fitted inside the circle</param>
        /// <returns></returns>
        public static int MaxRadius(Point center, Rectangle bounds)
        {
            double d1 = center.DistanceTo(bounds.TopLeft);
            double d2 = center.DistanceTo(bounds.BottomLeft);
            double d3 = center.DistanceTo(bounds.TopRight);
            double d4 = center.DistanceTo(bounds.BottomRight);

            return (int) Math.Ceiling(Math.Max(Math.Max(d1, d2), Math.Max(d3, d4)));
        }

        public override System.Drawing.Drawing2D.GraphicsPath GdiPath
        {
            get
            {
                if (null == gdiPath)
                {
                    gdiPath = new System.Drawing.Drawing2D.GraphicsPath();

                    int x = this.Center.X;
                    int y = this.Center.Y;
                    int r = this.Radius;

                    System.Drawing.Rectangle circleBounds = new System.Drawing.Rectangle(x - r, y - r, 2 * r, 2 * r);
                    gdiPath.AddEllipse(circleBounds);

                    ApplyTransform(base.GdiPath, base.Transform);
                }

                return gdiPath;
            }
        }

        public override void DrawOutline()
        {
            // leave the original path unmodified and make all temp changes to a copy
            System.Drawing.Drawing2D.GraphicsPath path = this.GdiPath.Clone() as System.Drawing.Drawing2D.GraphicsPath;
            float drawnRadius = path.GetBounds().Width / 2;

            for (int i = 0; i < this.Outline.LayerCount; i++)
            {
                LineLayer layer = this.Outline[i];
                if (null == layer)
                {
                    break;
                }

                using (System.Drawing.Pen pen = new System.Drawing.Pen(layer[0].Color.GdiColor, layer.Thickness))
                {
                    base.Graphics.DrawPath(pen, path);

                    // radius expansion
                    int Dr = layer.Thickness; // absolute
                    float newRadius = drawnRadius + Dr;
                    float dr = newRadius / drawnRadius; // relative
                    drawnRadius = newRadius; // increment

                    // move to top left corner of the image to scale properly
                    Point topLeft = new Point(path.GetBounds().Location);
                    System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
                    matrix.Translate((float)-topLeft.X, (float)-topLeft.Y);
                    path.Transform(matrix);

                    // scale
                    matrix.Reset();
                    matrix.Scale(dr, dr);
                    path.Transform(matrix);

                    // move to target location
                    matrix.Reset();
                    matrix.Translate((float)topLeft.X - Dr, (float)topLeft.Y - Dr);
                    path.Transform(matrix);
                }
            }
        }
    }
}
