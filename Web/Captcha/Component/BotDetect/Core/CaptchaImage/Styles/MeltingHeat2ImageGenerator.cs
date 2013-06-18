using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// MeltingHeat2 Captcha Image Generator
    /// </summary>
    internal class MeltingHeat2ImageGenerator : MeltingHeatImageGenerator, IImageGenerator
	{
        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // individual glyphs are expanded horizontally and vertically
            transform.Scaling.xScalingPercentageRange = new RandomRange(100, 110);
            transform.Scaling.yScalingPercentageRange = new RandomRange(100, 110);
        }


        protected override void DrawForegroundText()
        {
            // transparent glyph body
            textRenderer.Prototype.FillColor = backColor;
            textRenderer.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(2, textColor),
                LineLayer.Solid(1, backColor)
                
            );
            textRenderer.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            // random cuts in the image
            RandomLines cuts = new RandomLines();
            cuts.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, textColor)
            );
            cuts.SurfacePercentage = 10;
            cuts.Bounds = textRenderer.Bounds;
            cuts.Draw(graphics);

            // extra flame-like distortion
            Wave wave = new Wave();
            wave.Level = 2;
            wave.OverflowColor = backColor;
            wave.Apply(graphics);
        }
    }
}
