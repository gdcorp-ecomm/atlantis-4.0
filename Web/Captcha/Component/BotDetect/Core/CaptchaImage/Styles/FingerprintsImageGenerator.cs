using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Fingerprints Captcha Image Generator
    /// </summary>
    internal class FingerprintsImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(255, 255, 255); // white background

            // randomized fingerprint color
            textColor = Color.BetweenRgb(50, 50, 50).AndRgb(150, 150, 150).Frozen;
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
                textColor = Color.Randomized(customDarkColor, 100).Frozen;
            }
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // random smudges
            RandomCircles smudges = new RandomCircles();
            smudges.Prototype.FillColor = textColor;
            smudges.Prototype.RadiusRange = new RandomRange(1, (int)Math.Round(0.3 * graphics.ScalingFactor));
            smudges.SurfacePercentage = 5;
            smudges.DrawFast(graphics);

            // fingerprint-like overlay
            ConcentricCircles grooves = new ConcentricCircles();
            grooves.Prototype.Outline = LineStyle.Single(LineLayer.Solid(2, backColor));
            grooves.Spacing = 3;
            grooves.DrawFast(graphics);
        }
    }
}
