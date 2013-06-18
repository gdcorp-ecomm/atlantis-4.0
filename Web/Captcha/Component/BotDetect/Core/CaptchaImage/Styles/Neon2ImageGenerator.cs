using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Neon2 Captcha Image Generator
    /// </summary>
    internal class Neon2ImageGenerator : NeonImageGenerator, IImageGenerator
	{
        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thick mixed color outline to produce "comic"-like glyphs
            textRenderer.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(graphics.Height / 15, backColor),
                LineLayer.Solid(graphics.Height / 15 + 1, textColor)
            );
        }

        protected override void DrawEffects()
        {
            // ripple-like distortion
            Wave wave = new Wave();
            wave.OverflowColor = backColor;
            wave.Level = 3;
            wave.Apply(graphics);
        }
        
	}
}
