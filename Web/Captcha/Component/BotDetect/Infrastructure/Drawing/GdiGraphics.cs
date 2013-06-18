using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BotDetect.Drawing
{
    internal class GdiGraphics : IGraphics, IDisposable
    {
        private System.Drawing.Bitmap _image;
        internal System.Drawing.Bitmap Image
        {
            get
            {
                return _image;
            }
        }

        private System.Drawing.Graphics _gdiGraphics;
        internal System.Drawing.Graphics Graphics
        {
            get
            {
                return _gdiGraphics;
            }
        }

        private Rectangle _bounds;
        public Rectangle Bounds
        {
            get
            {
                return _bounds;
            }
        }

        private Rectangle _visibleBounds;
        public Rectangle VisibleBounds
        {
            get
            {
                return _visibleBounds;
            }
        }

        public int Width
        {
            get
            {
                return _image.Width;
            }
        }

        public int Height
        {
            get
            {
                return _image.Height;
            }
        }

        public int Surface
        {
            get
            {
                return _image.Width * _image.Height;
            }
        }

        public double ScalingFactor
        {
            get
            {
                return Math.Log(this.Surface, 2.0);
            }
        }

        //private System.Drawing.Imaging.BitmapData bitmapData;
        //private IntPtr pBase;
        private int stride;
        private byte[] rgbValues;
        private byte[] rgbValuesCopy;
        private byte[] bitmapHeader;
        private bool locked;

        public GdiGraphics(ImageSize size)
        {
            _image = new System.Drawing.Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _gdiGraphics = System.Drawing.Graphics.FromImage(_image);

            // high-speed is more important than high quality
            //_gdiGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            //_gdiGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            //_gdiGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            _gdiGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            // except for...
            _gdiGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 1 pixel "padding" outside the image to avoid black borders
            _bounds = new Rectangle(new Point(-1, -1), _image.Width + 2, _image.Height + 2);
            _visibleBounds = new Rectangle(new Point(0, 0), _image.Width, _image.Height);
        }

        public GdiGraphics(GdiGraphics other)
        {
            _image = new System.Drawing.Bitmap(other.Width, other.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _gdiGraphics = System.Drawing.Graphics.FromImage(_image);
            _gdiGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // copy data
            _gdiGraphics.DrawImage(other.Image, other.Bounds.GdiRectangleF, other.Bounds.GdiRectangleF, System.Drawing.GraphicsUnit.Pixel);

            // 1 pixel "padding" outside the image to avoid black borders
            _bounds = new Rectangle(new Point(-1, -1), _image.Width + 2, _image.Height + 2);
            _visibleBounds = new Rectangle(new Point(0, 0), _image.Width, _image.Height);
        }

        public IGraphics Clone()
        {
            return new GdiGraphics(this);
        }

        public MemoryStream GetStream(ImageFormat format)
        {
            MemoryStream image = new MemoryStream();
            System.Drawing.Imaging.ImageFormat gdiFormat = ImageFormatHelper.GetGdiImageFormat(format);
            _image.Save(image, gdiFormat);
            return image;
        }

        internal const int BytesPerPixel = 3;
        internal const int BitmapHeaderBytes = 54;

        public void LockPixels()
        {
            // lock the visible image data
            //System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(System.Drawing.Point.Empty, _image.Size);
            /*bitmapData = _image.LockBits(bounds, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            stride = bitmapData.Stride;

            // get the address of the first line, and copy the RGB values into the array
            pBase = bitmapData.Scan0;*/
            stride = _image.Width * BytesPerPixel;
            if (stride % 4 != 0) stride = 4 * (stride / 4 + 1);
            int bytes = stride * _image.Height;
            rgbValues = new byte[bytes];
            rgbValuesCopy = new byte[bytes];

            MemoryStream ms = new MemoryStream();
            _image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] streamBytes = ms.ToArray();
            Buffer.BlockCopy(streamBytes, BitmapHeaderBytes, rgbValues, 0, bytes);

            bitmapHeader = new byte[BitmapHeaderBytes];
            Buffer.BlockCopy(streamBytes, 0, bitmapHeader, 0, BitmapHeaderBytes);

            //System.Runtime.InteropServices.Marshal.Copy(pBase, rgbValues, 0, bytes);

            rgbValuesCopy = rgbValues.Clone() as byte[];
            locked = true;
        }

        public void SetPixel(int x1, int y1, Color color)
        {
            int index1 = y1 * stride + x1 * BytesPerPixel;
            rgbValues[index1] = color.B;
            rgbValues[index1 + 1] = color.G;
            rgbValues[index1 + 2] = color.R;
        }

        public void SwapPixels(int x1, int y1, int x2, int y2)
        {
            int index1 = y1 * stride + x1 * BytesPerPixel;
            int index2 = y2 * stride + x2 * BytesPerPixel;

            rgbValues[index1] = rgbValuesCopy[index2];
            rgbValues[index1 + 1] = rgbValuesCopy[index2 + 1];
            rgbValues[index1 + 2] = rgbValuesCopy[index2 + 2];

            /* int bytes = rgbValues.Length;
            int index1 = y1 * stride + x1 * 3;
            int index2 = (bytes -3) - ((y2+1) * stride - x2 * 3);

            rgbValues[index1] = rgbValuesCopy[index2];
            rgbValues[index1 + 1] = rgbValuesCopy[index2 + 1];
            rgbValues[index1 + 2] = rgbValuesCopy[index2 + 2];*/
        }

        public void UnlockPixels()
        {
            if (locked)
            {
                // copy the RGB values back to the bitmap
                //System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, pBase, rgbValues.Length);
                //_image.UnlockBits(bitmapData);
                MemoryStream ms = new MemoryStream();
                ms.Write(bitmapHeader, 0, BitmapHeaderBytes);
                ms.Write(rgbValues, 0, rgbValues.Length);
                _image = new System.Drawing.Bitmap(ms);
                _gdiGraphics = System.Drawing.Graphics.FromImage(_image);
                _gdiGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                _gdiGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                //pBase = IntPtr.Zero;
                //bitmapData = null;
                rgbValues = null;
                rgbValuesCopy = null;

                locked = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_gdiGraphics != null)
                {
                    _gdiGraphics.Dispose();
                    _gdiGraphics = null;
                }
                if(_image != null)
                {
                    _image.Dispose();
                    _image = null;
                }
                if (_bounds != null)
                {
                    _bounds.Dispose();
                    _bounds = null;
                }
                if (_visibleBounds != null)
                {
                    _visibleBounds.Dispose();
                    _visibleBounds = null;
                }
            }
        }

        #region IGraphics Members

        public void Fill(Color backColor, ShapeCollection clip)
        {
            this.Clip = clip;
            Fill(backColor);
            this.Clip = null;
        }

        public void Fill(Color backColor)
        {
            if (null == backColor)
            {
                return;
            }

            using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(backColor.GdiColor))
            {
                _gdiGraphics.FillRectangle(brush, _bounds.GdiRectangleF);
            }
        }

        private ShapeCollection _clipBounds;

        public ShapeCollection Clip
        {
            get
            {
                return _clipBounds;
            }

            set
            {
                _clipBounds = value;

                if (null == _clipBounds)
                {
                    _gdiGraphics.ResetClip();
                }
                else
                {
                    _gdiGraphics.ResetClip();
                    _gdiGraphics.SetClip(_clipBounds.GdiClip, System.Drawing.Drawing2D.CombineMode.Intersect);
                } 
            }
        }

        #endregion
    }
}
