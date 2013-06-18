using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Distortion Captcha Image Generator
    /// </summary>
    internal class DistortionImageGenerator : ImageGenerator, IImageGenerator
	{
        protected override void DrawEffects()
        {
            base.DrawEffects();

            // random smudges
            RandomCircles spots = new RandomCircles();
            spots.Prototype.FillColor = textColor;
            spots.Prototype.RadiusRange = new RandomRange(1, (int)Math.Round(0.2 * graphics.ScalingFactor));
            spots.SurfacePercentage = 5;
            spots.DrawFast(graphics);

            // wave distortion
            Wave wave = new Wave();
            wave.Level = 3;
            wave.Apply(graphics);
        }
    }
}
