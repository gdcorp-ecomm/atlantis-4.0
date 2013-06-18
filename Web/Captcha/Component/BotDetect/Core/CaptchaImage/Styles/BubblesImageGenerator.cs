using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Bubbles Captcha Image Generator
    /// </summary>
    internal class BubblesImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.BetweenRgb(200, 200, 200).AndRgb(250, 250, 250).Frozen; // light background
            textColor = Color.Rgb(0, 0, 0); // black text
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // thick outline background bubbles
            RandomCircles backBubbles = new RandomCircles();
            backBubbles.Prototype.FillColor = backColor;
            backBubbles.Bounds = textRenderer.Bounds; // rendered over the drawn text bounds
            backBubbles.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 15, textColor)
            );
            backBubbles.Prototype.RadiusRange = new RandomRange(1, graphics.Height / 6);
            backBubbles.SurfacePercentage = 12;
            backBubbles.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // thin outline foreground bubbles
            RandomCircles foreBubbles = new RandomCircles();
            foreBubbles.Bounds = textRenderer.Bounds; // rendered over the drawn text bounds
            foreBubbles.Prototype.FillColor = backColor;
            foreBubbles.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, textColor)
            );
            foreBubbles.Prototype.RadiusRange = new RandomRange(1, graphics.Height / 8);
            foreBubbles.SurfacePercentage = 6;
            foreBubbles.Draw(graphics);
        }
	}
}
