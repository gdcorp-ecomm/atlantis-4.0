using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// WavyColorLetters Captcha Image Generator
    /// </summary>
    internal class WavyColorLettersImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            outlineColor = Color.BetweenRgb(0, 0, 0).AndRgb(100, 100, 100);
            backColor = outlineColor.Complement.Frozen;
            textColor = null; // transparent text body
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = Color.Randomized(customLightColor, 50).Frozen;
            }

            if (null != customDarkColor)
            {
                outlineColor = Color.Randomized(customDarkColor, 100);
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // a bit thicker glyph outline
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 30 + 1, outlineColor)
            );
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // random background arcs
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(graphics.Height / 55 + 1, outlineColor),
                LineLayer.Solid(graphics.Height / 55 + 1, outlineColor)
            );
            circles.Prototype.Center = new Point(graphics.Bounds);
            circles.Prototype.Center.Y += 2 * graphics.Height;
            circles.Spacing = graphics.Height / 2;
            circles.Draw(graphics);

            // wave distortion
            Wave wave = new Wave();
            wave.Level = 3;
            wave.Apply(graphics);
        }
    }
}
