using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal class ShapeCollection: IEnumerable<AtomicShape>, IDisposable
    {
        protected AtomicShape prototype;

        // noise bounds
        protected Rectangle bounds;

        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }

            set
            {
                bounds = value;
            }
        }

        private List<AtomicShape> _shapes;

        public ShapeCollection()
        {
            _shapes = new List<AtomicShape>();
        }

        public AtomicShape this[int index]
        {
            get
            {
                AtomicShape element = null;

                if (index < _shapes.Count)
                {
                    element = _shapes[index];
                }

                return element;
            }

            set
            {
                if (index < _shapes.Count)
                {
                    _shapes[index] = value;
                }
                else
                {
                    _shapes.Add(value);
                }
            }
        }

        public void Add(AtomicShape shape)
        {
            _shapes.Add(shape);
        }

        public System.Drawing.Region GdiClip
        {
            get
            {
                System.Drawing.Region region = new System.Drawing.Region();
                bool empty = true;

                foreach (AtomicShape shape in this)
                {
                    if (empty)
                    {
                        region.Intersect(shape.GdiClip);
                        empty = false;
                    }
                    else
                    {
                        region.Union(shape.GdiClip);
                    }
                }

                return region;
            }
        }

        #region IEnumerable<AtomicShape> Members

        public IEnumerator<AtomicShape> GetEnumerator()
        {
            return _shapes.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _shapes.GetEnumerator();
        }

        #endregion

        #region IAtomicShape Members

        public int Surface
        {
            get 
            {
                int surface = 0;

                foreach (AtomicShape shape in this)
                {
                    surface += shape.Surface;
                }

                return surface;
            }
        }

        #endregion

        public virtual void Draw(IGraphics graphics)
        {
            Rectangle bounds = this.Bounds;
            if (null == bounds)
            {
                bounds = graphics.VisibleBounds;
            }

            this.Draw(graphics, bounds);
        }

        public virtual void Draw(IGraphics graphics, Rectangle bounds)
        {
            throw new NotImplementedException("Each shape class must implement the Draw method.");
        }

        public virtual void Draw(IGraphics graphics, ShapeCollection clip)
        {
            Rectangle bounds = this.Bounds;
            if (null == bounds)
            {
                bounds = graphics.VisibleBounds;
            }

            graphics.Clip = clip;
            this.Draw(graphics, bounds);
            graphics.Clip = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void DrawFast(IGraphics graphics, Rectangle bounds)
        {
            throw new NotImplementedException("Each shape class must implement the DrawFast method, if it's to be used.");
        }

        /// <summary>
        /// optimized drawing variant, used when 1) we don't need to keep each shape path,
        /// 2) we can ignore transforms, and 3) the shape is closed and doesn't have an outline
        /// 4) the shape is open and has only a single outline layer
        /// </summary>
        /// <param name="graphics"></param>
        public virtual void DrawFast(IGraphics graphics)
        {
            Rectangle bounds = this.Bounds;
            if (null == bounds)
            {
                bounds = graphics.VisibleBounds;
            }

            this.DrawFast(graphics, bounds);
        }

        public virtual void DrawFast(IGraphics graphics, ShapeCollection clip)
        {
            Rectangle bounds = this.Bounds;
            if (null == bounds)
            {
                bounds = graphics.VisibleBounds;
            }

            graphics.Clip = clip;
            this.DrawFast(graphics, bounds);
            graphics.Clip = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (prototype != null)
                {
                    prototype.Dispose();
                    prototype = null;
                }
            }
        }

    }
}
