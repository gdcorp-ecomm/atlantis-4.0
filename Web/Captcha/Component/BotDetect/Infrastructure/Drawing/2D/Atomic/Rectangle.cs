using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal class Rectangle : AtomicShape
    {
        private Point _topLeft;
        public Point TopLeft
        {
            get
            {
                return _topLeft;
            }
        }

        private Point _topRight;
        public Point TopRight
        {
            get
            {
                return _topRight;
            }
        }

        private Point _bottomLeft;
        public Point BottomLeft
        {
            get
            {
                return _bottomLeft;
            }
        }

        private Point _bottomRight;
        public Point BottomRight
        {
            get
            {
                return _bottomRight;
            }
        }

        public int Width
        {
            get
            {
                return (this.BottomRight.X - this.TopLeft.X);
            }
        }

        public int Height
        {
            get
            {
                return (this.BottomRight.Y - this.TopLeft.Y);
            }
        }

        public Rectangle(Point topLeft, int width, int height)
        {
            _topLeft = topLeft.Frozen;

            _bottomLeft = new Point(topLeft.X, topLeft.Y + height);
            _topRight = new Point(topLeft.X + width, topLeft.Y);
            _bottomRight = new Point(topLeft.X + width, topLeft.Y + height);
        }

        public Rectangle(Point topLeft, Point bottomRight)
        {
            _topLeft = topLeft.Frozen;
            _bottomRight = bottomRight.Frozen;

            _bottomLeft = new Point(topLeft.X, bottomRight.Y);
            _topRight = new Point(bottomRight.X, topLeft.Y);
        }

        public Rectangle(Rectangle rectangle)
        {
            _topLeft = rectangle.TopLeft;
            _bottomLeft = rectangle.BottomLeft;
            _bottomRight = rectangle.BottomRight;
            _topRight = rectangle.TopRight;
        }

        public Rectangle(System.Drawing.Rectangle rectangle)
        {
            _topLeft = new Point(rectangle.Left, rectangle.Top);
            _bottomLeft = new Point(rectangle.Left, rectangle.Bottom);
            _bottomRight = new Point(rectangle.Right, rectangle.Bottom);
            _topRight = new Point(rectangle.Right, rectangle.Top);
        }

        public Rectangle(System.Drawing.RectangleF rectangle)
        {
            _topLeft = new Point(((int)Math.Round(rectangle.Left)), ((int)Math.Round(rectangle.Top)));
            _bottomLeft = new Point(((int)Math.Round(rectangle.Left)), ((int)Math.Round(rectangle.Bottom)));
            _bottomRight = new Point(((int)Math.Round(rectangle.Right)), ((int)Math.Round(rectangle.Bottom)));
            _topRight = new Point(((int)Math.Round(rectangle.Right)), ((int)Math.Round(rectangle.Top)));
        }

        public Rectangle(Rectangle outer, Rectangle inner)
        {
            _topLeft = new Point(outer.TopLeft, inner.TopLeft).Frozen;
            _bottomRight = new Point(inner.BottomRight, outer.BottomRight).Frozen;

            _bottomLeft = new Point(_topLeft.X, _bottomRight.Y).Frozen;
            _topRight = new Point(_bottomRight.X, _topLeft.Y).Frozen;
        }

        public Rectangle(Rectangle rectangle, double ratio)
        {
            if (0.0 > ratio || ratio >= 1.0)
            {
                throw new DrawingException("Can only create smaller rectangles with this method, the ratio must be between 0 and 1", ratio);
            }

            int safeBoundsWidth = (int) (Math.Round(rectangle.Width * (1 - ratio)));
            int safeBoundsHeight = (int) (Math.Round(rectangle.Height * (1 - ratio)));
            Rectangle safeBounds = new Rectangle(rectangle.TopLeft, safeBoundsWidth, safeBoundsHeight);

            _topLeft = new Point(safeBounds).Frozen;

            int width = (int) (Math.Round(rectangle.Width * ratio));
            int height = (int)(Math.Round(rectangle.Height * ratio));

            _bottomLeft = new Point(_topLeft.X, _topLeft.Y + height);
            _topRight = new Point(_topLeft.X + width, _topLeft.Y);
            _bottomRight = new Point(_topLeft.X + width, _topLeft.Y + height);
        }

        public Rectangle()
        {
        }

        public Point Center
        {
            get
            {
                return new Point((_topLeft.X + _bottomRight.X) / 2, (_topLeft.Y + _bottomRight.Y) / 2);
            }
        }

        public System.Drawing.Rectangle GdiRectangle
        {
            get
            {
                return new System.Drawing.Rectangle(_topLeft.X, _topLeft.Y, this.Width, this.Height);
            }
        }

        public System.Drawing.RectangleF GdiRectangleF
        {
            get
            {
                return new System.Drawing.RectangleF(_topLeft.X, _topLeft.Y, this.Width, this.Height);
            }
        }

        public override int Surface
        {
            get
            {
                int w = this.Width;
                int h = this.Height;

                // rectangle interior surface
                int innerSurface = w * h;

                // rectangle outline surface
                int circumference = 2 * (w + h);
                int outersurface = circumference * this.Outline.Thickness;

                return (innerSurface + outersurface);
            }
        }

        public override System.Drawing.Drawing2D.GraphicsPath GdiPath
        {
            get
            {
                if (null == gdiPath)
                {
                    gdiPath = new System.Drawing.Drawing2D.GraphicsPath();

                    System.Drawing.Rectangle gdiRectangle = this.GdiRectangle;
                    gdiPath.AddRectangle(gdiRectangle);
                    ApplyTransform(gdiPath, this.Transform);
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

                    Rectangle drawnBounds = new Rectangle(path.GetBounds());

                    System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
                    matrix.Translate((float)-drawnBounds.TopLeft.X, (float)-drawnBounds.TopLeft.Y);
                    path.Transform(matrix);

                    System.Drawing.RectangleF targetBounds = path.GetBounds();
                    targetBounds.Inflate(layer.Thickness, layer.Thickness);

                    matrix = new System.Drawing.Drawing2D.Matrix();
                    float xFactor = (float)targetBounds.Width / drawnBounds.Width;
                    float yFactor = (float)targetBounds.Height / drawnBounds.Height;
                    matrix.Scale(xFactor * 1.0F, yFactor * 1.0F);
                    path.Transform(matrix);

                    matrix.Reset();
                    matrix.Translate((float)drawnBounds.TopLeft.X - layer.Thickness, (float)drawnBounds.TopLeft.Y - layer.Thickness);
                    path.Transform(matrix);
                }
            }
        }
    }
}
