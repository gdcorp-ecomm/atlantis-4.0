using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// ThinWavyLetters Captcha Image Generator
    /// </summary>
    internal class ThinWavyLettersImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(0, 0, 0);
            textColor = null;
            outlineColor = Color.Rgb(170, 230, 40);
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                outlineColor = customLightColor;
            }

            if (null != customDarkColor)
            {
                backColor = customDarkColor;
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thicker glyph outline in text color for "bolder" text
            textRenderer.Prototype.Outline = LineStyle.Single(LineLayer.Solid(2, outlineColor));
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // random background arcs
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, outlineColor)
            );
            circles.Prototype.Center = new Point(graphics.Bounds);
            circles.Prototype.Center.Y += 2 * graphics.Height;
            circles.Spacing = graphics.Height / 2;
            circles.Draw(graphics);

            // ripple distortion
            Wave wave = new Wave();
            wave.Level = 2;
            wave.OverflowColor = backColor;
            wave.Apply(graphics);
        }
    }
}
