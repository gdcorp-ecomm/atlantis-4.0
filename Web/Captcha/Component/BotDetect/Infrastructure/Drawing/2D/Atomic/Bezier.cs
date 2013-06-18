using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace BotDetect.Drawing
{
    internal class Bezier : AtomicShape 
    {
        private Rectangle _curveBounds;

        public Rectangle CurveBounds
        {
            get
            {
                return _curveBounds;
            }

            set
            {
                _curveBounds = value;
            }
        }

        public Bezier(Rectangle curveBounds, LineStyle style)
        {
            this.CurveBounds = curveBounds;
            this.Outline = style;
        }

        public Bezier(Rectangle curveBounds)
        {
            this.CurveBounds = curveBounds;
        }

        public Bezier()
        {
        }

        public override string ToString()
        {
            string output = String.Format(CultureInfo.InvariantCulture, @"BotDetect.Drawing.Bezier { curve bounds: {0}, line style: {1} }", StringHelper.ToString(_curveBounds), StringHelper.ToString(this.Outline));
            return output;
        }

        public override int Surface
        {
            get 
            {
                // aproximation of the curve surface with a diagonal line surface
                int length = this.CurveBounds.TopLeft.DistanceTo(this.CurveBounds.BottomRight);
                return length * this.Outline.Thickness;
            }
        }

        public override System.Drawing.Drawing2D.GraphicsPath GdiPath
        {
            get
            {
                if (null == gdiPath)
                {
                    gdiPath = new System.Drawing.Drawing2D.GraphicsPath();

                    System.Drawing.Point pt1 = new Point(this.CurveBounds).GdiPoint;
                    System.Drawing.Point pt2 = new Point(this.CurveBounds).GdiPoint;
                    System.Drawing.Point pt3 = new Point(this.CurveBounds).GdiPoint;
                    System.Drawing.Point pt4 = new Point(this.CurveBounds).GdiPoint;

                    gdiPath.AddBezier(pt1, pt2, pt3, pt4);
                    ApplyTransform(gdiPath, base.Transform);
                }

                return gdiPath;
            }
        }

        public override void DrawOutline()
        {
            // leave the original path unmodified and make all temp changes to a copy
            System.Drawing.Drawing2D.GraphicsPath path = this.GdiPath.Clone() as System.Drawing.Drawing2D.GraphicsPath;

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

                    System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();

                    // make sure the curve layer expansion is orthogonal to the curve axis
                    float Dx = path.GetBounds().Width;
                    float Dy = path.GetBounds().Height;

                    double dx = layer.Thickness;
                    double dy = layer.Thickness;

                    if (0 == Dx)
                    {
                        // vertical curve axis
                        dy = 0;
                    }
                    else if (0 == Dy)
                    {
                        // horizontal curve axis
                        dx = 0;
                    }
                    else
                    {
                        // angled curve axis
                        double alpha = Math.Atan2((double)Dx, (double)Dy);
                        dx *= Math.Cos(alpha);
                        dy *= Math.Sin(alpha);
                    }

                    matrix.Translate((float)dx, (float)dy);
                    path.Transform(matrix);
                }
            }
        }
    }
}
