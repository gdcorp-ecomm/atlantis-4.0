using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Negative Captcha Image Generator
    /// </summary>
    internal class NegativeImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(0, 0, 0); // black background
            textColor = Color.Rgb(255, 255, 255); // white text
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                textColor = customLightColor;
            }

            if (null != customDarkColor)
            {
                backColor = customDarkColor;
            }
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // random background arcs
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(graphics.Height / 20, textColor),
                LineLayer.Solid(graphics.Height / 20, backColor)
            );
            circles.Prototype.Center = new Point(graphics.Bounds);
            circles.Prototype.Center.Y += 2 * graphics.Height;
            circles.Spacing = graphics.Height / 2;
            circles.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // film-like grainy noise
            RandomDots lightDots = new RandomDots();
            lightDots.Prototype.FillColor = textColor;
            lightDots.SurfacePercentage = 3;
            lightDots.DrawFast(graphics);

            RandomDots darkDots = new RandomDots();
            darkDots.Prototype.FillColor = backColor;
            darkDots.SurfacePercentage = 3;
            darkDots.DrawFast(graphics);
        }
    }
}
