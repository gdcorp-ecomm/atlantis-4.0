using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal class Halo : Effect, IEffect
    {
        public override void Apply(IGraphics graphics, Rectangle bounds)
        {
            // create a bitmap in a appropriate ratio to the original drawing area.
            float ratio = 0.20F;//(10 - this.Level) / 1000.0F;

            GdiGraphics g = (graphics as GdiGraphics);
            System.Drawing.Graphics gImage = g.Graphics;
            System.Drawing.Bitmap bImage = g.Image;

            System.Drawing.Bitmap subImage = new System.Drawing.Bitmap((int)(bounds.Width * ratio), (int)(bounds.Height * ratio));
            System.Drawing.Graphics gSubImage = System.Drawing.Graphics.FromImage(subImage);

            gImage.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            gSubImage.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

            System.Drawing.Rectangle fullSize = new System.Drawing.Rectangle(0, 0, (int)bounds.Width, (int)bounds.Height);
            System.Drawing.Rectangle scaledDown = new System.Drawing.Rectangle(0, 0, (int)subImage.Width, (int)subImage.Height);

            gSubImage.DrawImage(bImage, scaledDown, fullSize, System.Drawing.GraphicsUnit.Pixel);
            gImage.DrawImage(subImage, fullSize, scaledDown, System.Drawing.GraphicsUnit.Pixel);
        }
    }
}
