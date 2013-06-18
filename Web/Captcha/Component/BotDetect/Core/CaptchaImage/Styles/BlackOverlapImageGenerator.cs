using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// BlackOverlap Captcha Image Generator
    /// </summary>
    internal class BlackOverlapImageGenerator : ImageGenerator, IImageGenerator
	{
        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thicker glyph outline in text color for "bolder" text
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(2, graphics.Height / 20), textColor)
            );
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // individual glyphs are expanded horizontally so they overlap
            transform.Scaling.xScalingPercentageRange = new RandomRange(115, 125);
        }
	}
}
