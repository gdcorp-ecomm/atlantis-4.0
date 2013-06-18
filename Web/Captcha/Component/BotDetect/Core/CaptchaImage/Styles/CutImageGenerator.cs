using System;
using System.Collections;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Cut Captcha Image Generator
    /// </summary>
    internal class CutImageGenerator : ImageGenerator, IImageGenerator
    {
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            textColor = Color.Rgb(0, 0, 0);  // black text
            backColor = Color.Rgb(220, 220, 220); // light gray background
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // random cuts in the image
            RandomLines cuts = new RandomLines();
            cuts.Prototype.Outline = LineStyle.Single(LineLayer.Solid(graphics.Height / 20, textColor));
            cuts.SurfacePercentage = 5;
            cuts.Bounds = textRenderer.Bounds;
            cuts.Draw(graphics);

            // re-draw the cuts within text bounds - thicker, using background color
            graphics.Clip = textRenderer;
            foreach (AtomicShape s in cuts)
            {
                Line l = s as Line;
                if (null != l)
                {
                    l.Outline = LineStyle.Single(LineLayer.Solid(graphics.Height / 20, backColor));
                    l.Draw(graphics);
                }
            }
            graphics.Clip = null;
        }
	}		
}
