using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Jail Captcha Image Generator
    /// </summary>
    internal class JailImageGenerator : ImageGenerator, IImageGenerator
	{
        protected LineStyle bars;

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            outlineColor = Color.Lightened(textColor, 1.8F); // text outline "bolding" the glyphs
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = customLightColor;
            }

            if (null != customDarkColor)
            {
                textColor = customDarkColor;
                outlineColor = Color.Lightened(textColor, 1.8F);
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thicker outline
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 20, outlineColor)
            );
        }

        protected override void CustomInit()
        {
            // jail "bars" have both a light and a dark side
            bars = LineStyle.Double(
                LineLayer.Solid(Math.Max(graphics.Height / 15 - 1, 1), textColor),
                LineLayer.Solid(Math.Max(graphics.Height / 15 - 1, 1), backColor)
            );
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // distorted 
            Wave wave = new Wave();
            wave.Level = 1;
            wave.Apply(graphics);

            // foreground grid
            HorizontalLines hLines = new HorizontalLines();
            hLines.SpacingRange = new RandomRange(3 * graphics.Height / 8 + 1, 4 * graphics.Height / 9 + 1);
            hLines.Prototype.Outline = bars;
            hLines.Draw(graphics);

            VerticalLines vLines = new VerticalLines();
            vLines.SpacingRange = new RandomRange(3 * graphics.Height / 6 + 1, 4 * graphics.Height / 7 + 1);
            vLines.Prototype.Outline = bars;
            vLines.Draw(graphics);
        }
    }
}
