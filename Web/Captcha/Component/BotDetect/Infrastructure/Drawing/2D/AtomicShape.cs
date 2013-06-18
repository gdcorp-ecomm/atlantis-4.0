using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal abstract class AtomicShape : IDisposable
    {
        // outline definition
        public static readonly LineStyle DefaultOutline = LineStyle.Single(LineLayer.Solid(1, null));

        private LineStyle _outlineStyle;

        public LineStyle Outline
        {
            get
            {
                if (null == _outlineStyle)
                {
                    return DefaultOutline;
                }

                return _outlineStyle;
            }

            set
            {
                _outlineStyle = value;
            }
        }

        // fill definition
        public static readonly Color DefaultFillColor = null;

        private Color _fillColor;

        public Color FillColor
        {
            get
            {
                if (null == _fillColor)
                {
                    return DefaultFillColor;
                }

                return _fillColor;
            }

            set
            {
                _fillColor = value;
            }
        }

        // shape transform
        protected Transform transform = Transform.None;

        public Transform Transform
        {
            get
            {
                return transform;
            }

            set
            {
                transform = value;
            }
        }

        private System.Drawing.Graphics _gdiGraphics;
        public System.Drawing.Graphics Graphics
        {
            get
            {
                return _gdiGraphics;
            }
        }

        protected System.Drawing.Drawing2D.GraphicsPath gdiPath;

        public virtual System.Drawing.Drawing2D.GraphicsPath GdiPath
        {
            get
            {
                if (null == gdiPath)
                {
                    gdiPath = new System.Drawing.Drawing2D.GraphicsPath();
                }

                return gdiPath;
            }
        }

        public System.Drawing.Region GdiClip 
        {
            get
            {
                System.Drawing.Region clipRegion = new System.Drawing.Region(this.GdiPath);
                return clipRegion;
            }
        }

        protected static void ApplyTransform(System.Drawing.Drawing2D.GraphicsPath path, Transform transform)
        {
            System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();

            System.Drawing.RectangleF pathBoundsF = path.GetBounds();
            Rectangle bounds = new Rectangle(pathBoundsF);

            if (null != transform)
            {
                if (transform.Translation.HasValue())
                {
                    int xOffset = transform.Translation.xOffsetRelative(bounds.Width);
                    int yOffset = transform.Translation.yOffsetRelative(bounds.Height);
                    matrix.Translate(xOffset, yOffset);
                    path.Transform(matrix);
                    matrix.Reset();
                }

                if (transform.Scaling.HasValue())
                {
                    matrix.Scale((float)transform.Scaling.xScalingFactor, (float)transform.Scaling.yScalingFactor);
                    path.Transform(matrix);
                    matrix.Reset();
                }

                if (transform.Rotation.HasValue())
                {
                    matrix.RotateAt(transform.Rotation.Angle, bounds.Center.GdiPoint);
                    path.Transform(matrix);
                    matrix.Reset();
                }

                if (transform.Warp.HasValue())
                {
                    // distortion parameters are randomized
                    float v = (float)transform.Warp.WarpFactor;
                    System.Drawing.PointF[] points =
                    {
                        new System.Drawing.PointF(bounds.TopLeft.X + RandomGenerator.Next(bounds.Width * v), bounds.TopLeft.Y + RandomGenerator.Next(bounds.Height * v)),
                        new System.Drawing.PointF(bounds.TopLeft.X + bounds.Width - RandomGenerator.Next(bounds.Width * v), bounds.TopLeft.Y + RandomGenerator.Next(bounds.Height * v)),
                        new System.Drawing.PointF(bounds.TopLeft.X + RandomGenerator.Next(bounds.Width * v),  bounds.TopLeft.Y + bounds.Height - RandomGenerator.Next(bounds.Height * v)),
                        new System.Drawing.PointF(bounds.TopLeft.X + bounds.Width - RandomGenerator.Next(bounds.Width * v),  bounds.TopLeft.Y + bounds.Height - RandomGenerator.Next(bounds.Height * v))
                    };

                    // apply distortion to the path
                    path.Warp(points, bounds.GdiRectangleF, matrix, System.Drawing.Drawing2D.WarpMode.Perspective, 1F);
                    matrix.Reset();
                }
            }
        }

        protected static void ApplyTransform(System.Drawing.Drawing2D.GraphicsPath path, Transform transform, Rectangle targetBounds)
        {
            // apply transformations that affect the targetBounds
            if (null != transform)
            {
                if (transform.Translation.HasValue())
                {
                    // horizontal translation
                    int xOffset = transform.Translation.xOffsetRelative(targetBounds.Width);
                    targetBounds.TopLeft.X += xOffset;
                    targetBounds.BottomRight.X += xOffset;

                    // vertical translation
                    int yOffset = transform.Translation.yOffsetRelative(targetBounds.Height);
                    targetBounds.TopLeft.Y += yOffset;
                    targetBounds.BottomRight.Y += yOffset;
                }

                if (transform.Scaling.HasValue())
                {
                    // use a square root of the scaling factor, and apply it twice
                    // once to topLeft and once to bottomRight - for equal expansion 

                    // horizontal scaling
                    int width = targetBounds.Width;
                    int targetWidth = (int)Math.Round(width * transform.Scaling.xScalingFactor);
                    int xOffset = (targetWidth - width) / 2;
                    targetBounds.TopLeft.X -= xOffset;
                    targetBounds.BottomRight.X += xOffset;

                    // vertical scaling
                    int height = targetBounds.Height;
                    int targetHeight = (int)Math.Round(height * transform.Scaling.yScalingFactor);
                    int yOffset = (targetHeight - height) / 2;
                    targetBounds.TopLeft.Y -= yOffset;
                    targetBounds.BottomRight.Y += yOffset;

                    float yFactor1 = (float)Math.Sqrt(transform.Scaling.yScalingFactor);
                    targetBounds.TopLeft.Y = (int)Math.Round(targetBounds.TopLeft.Y / yFactor1);
                    targetBounds.BottomRight.Y = (int)Math.Round(yFactor1 * targetBounds.BottomRight.Y);
                }
            }

            // move the text to the top left corner of the image
            System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
            matrix.Translate(-path.GetBounds().Left, -path.GetBounds().Top);
            path.Transform(matrix);
            matrix.Reset();

            // calculate drawn bounds
            System.Drawing.RectangleF pathBoundsF = path.GetBounds();

            // scale to target size
            float xFactor = (float)targetBounds.Width / pathBoundsF.Width;
            float yFactor = (float)targetBounds.Height / pathBoundsF.Height;
            matrix.Scale(xFactor * 1.0F, yFactor * 1.0F);
            path.Transform(matrix);
            matrix.Reset();

            // move to target position
            matrix.Translate(targetBounds.TopLeft.X, (targetBounds.TopLeft.Y * 1.0F));
            path.Transform(matrix);
            matrix.Reset();

            // apply post-targetBounds transformations
            if (null != transform)
            {
                if (transform.Rotation.HasValue())
                {
                    // rotate around the center
                    matrix.RotateAt(transform.Rotation.Angle, targetBounds.Center.GdiPoint);
                    path.Transform(matrix);
                    matrix.Reset();
                }

                if (transform.Warp.HasValue())
                {
                    // distortion parameters are randomized
                    float v = (float)transform.Warp.WarpFactor;
                    System.Drawing.PointF[] points =
                    {
                        new System.Drawing.PointF(targetBounds.TopLeft.X + RandomGenerator.Next(targetBounds.Width * v), targetBounds.TopLeft.Y + RandomGenerator.Next(targetBounds.Height * v)),
                        new System.Drawing.PointF(targetBounds.TopLeft.X + targetBounds.Width - RandomGenerator.Next(targetBounds.Width * v), targetBounds.TopLeft.Y + RandomGenerator.Next(targetBounds.Height * v)),
                        new System.Drawing.PointF(targetBounds.TopLeft.X + RandomGenerator.Next(targetBounds.Width * v),  targetBounds.TopLeft.Y + targetBounds.Height - RandomGenerator.Next(targetBounds.Height * v)),
                        new System.Drawing.PointF(targetBounds.TopLeft.X + targetBounds.Width - RandomGenerator.Next(targetBounds.Width * v),  targetBounds.TopLeft.Y + targetBounds.Height - RandomGenerator.Next(targetBounds.Height * v))
                    };

                    // apply distortion to the path
                    path.Warp(points, targetBounds.GdiRectangleF, matrix, System.Drawing.Drawing2D.WarpMode.Perspective, 1F);
                    matrix.Reset();
                }
            }
        }

        #region IAtomicShape Members

        public abstract int Surface { get; }

        public virtual void Draw(IGraphics graphics)
        {
            _gdiGraphics = (graphics as GdiGraphics).Graphics;

            if ((!LineStyle.HasValue(this.Outline)) &&
                (null == this.FillColor))
            {
                return;
            }

            if (null != this.FillColor)
            {
                DrawBody();
            }

            if (LineStyle.HasValue(this.Outline))
            {
                DrawOutline();
            }
        }

        public virtual void DrawBody()
        {
            using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(this.FillColor.GdiColor))
            {
                _gdiGraphics.FillPath(brush, this.GdiPath);
            }
        }

        public virtual void DrawOutline()
        {
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (gdiPath != null)
                {
                    gdiPath.Dispose();
                    gdiPath = null;
                }
            }
        }

        #endregion
    }
}
