using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal abstract class Text : ShapeCollection, ICloneable
    {
        // shape prototype
        public Glyph Prototype
        {
            get
            {
                if (null == prototype)
                {
                    prototype = new Glyph();
                }

                return prototype as Glyph;
            }
        }

        // text to render
        protected string text;

        public string TextToRender
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        // text font selection
        protected FontCollection fonts;

        public FontCollection Fonts
        {
            get
            {
                return fonts;
            }

            set
            {
                fonts = value;
            }
        }

        // character transform definition
        protected Transform transform;

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

        public static Rectangle DefaultTextBounds(IGraphics graphics)
        {
            int xOffset = graphics.Width / 7;
            int yOffset = graphics.Height / 6;

            Point topLeft = Point.Between(1, 1).And(xOffset, yOffset).Frozen;
            Point bottomRight = Point.Between(graphics.Width - xOffset, graphics.Height - yOffset).And(graphics.Width - 1, graphics.Height - 1).Frozen;

            return new Rectangle(topLeft, bottomRight);
        }

        public abstract override void Draw(IGraphics graphics, Rectangle bounds);

        public override void Draw(IGraphics graphics)
        {
            Rectangle bounds = this.Bounds;
            if (null == bounds)
            {
                bounds = DefaultTextBounds(graphics);
                this.Bounds = bounds;
            }

            this.Draw(graphics, bounds);
        }

        public override void Draw(IGraphics graphics, ShapeCollection clip)
        {
            Rectangle bounds = this.Bounds;
            if (null == bounds)
            {
                bounds = DefaultTextBounds(graphics);
                this.Bounds = bounds;
            }

            graphics.Clip = clip;
            this.Draw(graphics, bounds);
            graphics.Clip = null;
        }


        #region ICloneable Members

        public object Clone()
        {


            return this.MemberwiseClone();
        }

        #endregion
    }
}
