using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing 
{
    class RandomGlyphs : RandomNoise
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

        public string Text
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

        public override void DrawFast(IGraphics graphics, Rectangle bounds)
        {
            //  performance optimization - skip the regular shape drawing code
            throw new NotImplementedException("Not used in any algorithms yet");
        }

        public override int DrawSingleRandomShape(IGraphics graphics, Rectangle bounds)
        {
            Glyph glyph = new Glyph();

            Point topLeft = new Point(bounds);
            topLeft.X -= bounds.Height / 10;
            topLeft.Y -= bounds.Height / 4;

            glyph.Bounds = new Rectangle(topLeft, bounds.Height / 2, bounds.Height / 3);

            glyph.FillColor = this.Prototype.FillColor;
            glyph.Outline = this.Prototype.Outline;
            glyph.Transform = this.Prototype.Transform;
            glyph.Font = this.Fonts.Next;
            glyph.Character = RandomGenerator.Next(text);

            glyph.Draw(graphics);
            this.Add(glyph);

            return glyph.Surface;
        }
    }
}
