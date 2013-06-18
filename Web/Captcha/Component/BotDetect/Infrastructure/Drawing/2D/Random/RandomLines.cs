using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing 
{
    class RandomLines : RandomNoise
    {
        // shape prototype
        public Line Prototype
        {
            get
            {
                if (null == prototype)
                {
                    prototype = new Line();
                }

                return prototype as Line;
            }
        }

        public override void DrawFast(IGraphics graphics, Rectangle bounds)
        {
            // performance optimization - skip the regular shape drawing code
            throw new NotImplementedException("Not used in any algorithms yet");
        }

        public override int DrawSingleRandomShape(IGraphics graphics, Rectangle bounds)
        {
            Point pt1 = new Point(bounds).Frozen;
            Point pt2 = new Point(bounds).Frozen;
            Line line = new Line(pt1, pt2, this.Prototype.Outline);
            line.Transform = this.Prototype.Transform;

            line.Draw(graphics);
            this.Add(line);

            return line.Surface;
        }
    }
}
