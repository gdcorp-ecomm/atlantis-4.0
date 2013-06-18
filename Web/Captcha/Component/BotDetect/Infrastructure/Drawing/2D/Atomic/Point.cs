using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace BotDetect.Drawing
{
    internal class Point : AtomicShape 
    {
        // x
        private RandomRange _xRange;

        public RandomRange XRange
        {
            get
            {
                return _xRange;
            }

            set
            {
                _xRange = value;
            }
        }

        public int X
        {
            get
            {
                if (null == _xRange)
                {
                    return 0;
                }

                return _xRange.Next;
            }
            set
            {
                _xRange = new RandomRange(value);
            }
        }


        // y
        private RandomRange _yRange;

        public RandomRange YRange
        {
            get
            {
                return _yRange;
            }

            set
            {
                _yRange = value;
            }
        }

        public int Y
        {
            get
            {
                if (null == _yRange)
                {
                    return 0;
                }

                return _yRange.Next;
            }
            set
            {
                _yRange = new RandomRange(value);
            }
        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(Point value)
        {
            _xRange = value._xRange;
            _yRange = value._yRange;
        }

        public Point(Point min, Point max)
        {
            _xRange = new RandomRange(min.XRange.Min, max.XRange.Max);
            _yRange = new RandomRange(min.YRange.Min, max.YRange.Max);
        }

        public Point(Rectangle rectangle)
            : this(rectangle.TopLeft, rectangle.BottomRight)
        {
        }

        public Point()
        {
        }

        public override string ToString()
        {
            string output = String.Format(CultureInfo.InvariantCulture, @"BotDetect.Drawing.Point { x={0}, y={1} }", _xRange, _yRange);
            return output;
        }

        public Point(System.Drawing.Point point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        public Point(System.Drawing.PointF point)
        {
            this.X = (int)(Math.Round(point.X));
            this.Y = (int)(Math.Round(point.Y));
        }

        public static Point At(int x, int y)
        {
            return new Point(x, y);
        }

        public static Point Between(int x, int y)
        {
            return new Point(x, y);
        }

        public Point And(int x, int y)
        {
            return new Point(this, new Point(x, y));
        }

        public override int Surface
        {
            get
            {
                return 1;
            }
        }

        public int DistanceTo(Point point2)
        {
            return (int)Math.Round(Math.Sqrt(Math.Pow((this.X - point2.X), 2) + Math.Pow((this.Y - point2.Y), 2)));
        }

        public void Freeze()
        {
            _xRange.Freeze();
            _yRange.Freeze();
        }

        public Point Frozen
        {
            get
            {
                this.Freeze();
                return this;
            }
        }

        public System.Drawing.Point GdiPoint
        {
            get
            {
                return new System.Drawing.Point(this.X, this.Y);
            }
        }

        public override System.Drawing.Drawing2D.GraphicsPath GdiPath
        {
            get
            {
                if (null == gdiPath)
                {
                    gdiPath = new System.Drawing.Drawing2D.GraphicsPath();

                    int x = this.X;
                    int y = this.Y;
                    int r = 1;

                    gdiPath.AddEllipse(x, y, r, r);
                    ApplyTransform(gdiPath, base.Transform);
                }

                return gdiPath;
            }
        }

        public override void DrawOutline()
        {
            // do nothing, points have no outline
        }
    }
}
