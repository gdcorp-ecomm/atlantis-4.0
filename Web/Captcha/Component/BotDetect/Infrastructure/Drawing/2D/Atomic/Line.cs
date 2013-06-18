using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal class Line : AtomicShape 
    {
        private Point _point1;

        public Point Point1
        {
            get
            {
                return _point1;
            }

            set
            {
                _point1 = value;
            }
        }

        private Point _point2;

        public Point Point2
        {
            get
            {
                return _point2;
            }

            set
            {
                _point2 = value;
            }
        }

        public Line(Point pt1, Point pt2, LineStyle style)
        {
            this.Point1 = pt1;
            this.Point2 = pt2;
            this.Outline = style;
        }

        public Line(Point pt1, Point pt2)
        {
            this.Point1 = pt1;
            this.Point2 = pt2;
        }

        public Line()
        {
        }

        public override int Surface
        {
            get 
            { 
                // line surface is a product of line length and thickness
                int length = this.Point1.DistanceTo(this.Point2);
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

                    System.Drawing.Point pt1 = this.Point1.GdiPoint;
                    System.Drawing.Point pt2 = this.Point2.GdiPoint;

                    gdiPath.AddLine(pt1, pt2);
                    ApplyTransform(gdiPath, base.Transform);
                }

                return gdiPath;
            }
        }

        public override void DrawBody()
        {
            // do nothing, lines have no body
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

                    // make sure the line layer expansion is orthogonal to the line
                    float Dx = path.GetBounds().Width;
                    float Dy = path.GetBounds().Height;

                    double dx = layer.Thickness;
                    double dy = layer.Thickness;

                    if (0 == Dx)
                    {
                        // vertical lines
                        dy = 0;
                    }
                    else if (0 == Dy)
                    {
                        // horizontal lines
                        dx = 0;
                    }
                    else
                    {
                        // angled lines
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
