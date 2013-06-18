using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal class LtrText : Text
    {
        public override void Draw(IGraphics graphics, Rectangle bounds)
        {
            int glyphMaxWidth = bounds.Width / text.Length;

            for (int i = 0; i < text.Length; i++)
            {
                Glyph characterGlyph = new Glyph();
                characterGlyph.Character = text.Substring(i, 1);
                characterGlyph.Outline = this.Prototype.Outline;
                characterGlyph.FillColor = this.Prototype.FillColor;
                characterGlyph.Transform = this.Transform;
                characterGlyph.Font = this.Fonts.Next;

                Point topLeft = new Point(bounds.TopLeft.X + i * glyphMaxWidth, bounds.TopLeft.Y);
                characterGlyph.Bounds = new Rectangle(topLeft, glyphMaxWidth, bounds.Height);

                characterGlyph.Draw(graphics);

                this[i] = characterGlyph;
            }
        }
    }
}
