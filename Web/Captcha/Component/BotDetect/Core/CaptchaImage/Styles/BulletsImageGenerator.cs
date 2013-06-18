using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Bullets Captcha Image Generator
    /// </summary>
    internal class BulletsImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(220, 220, 220); // light gray background
            textColor = Color.Rgb(0, 0, 0); // black text
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // shooting range "wire"
            HorizontalLines wires = new HorizontalLines();
            wires.SpacingRange = new RandomRange(graphics.Height / 3, graphics.Height / 2);
            wires.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(2, graphics.Height / 20), textColor)
            );
            wires.Draw(graphics);

            // bullet "holes" in text
            RandomCircles holes = new RandomCircles();
            holes.Bounds = textRenderer.Bounds;
            holes.Prototype.FillColor = backColor;
            holes.Prototype.Radius = graphics.Height / 15;
            holes.SurfacePercentage = graphics.Height / 6;
            holes.DrawFast(graphics);

            // bullets embedded in background "wall"
            RandomCircles bullets = new RandomCircles();
            bullets.Bounds = textRenderer.Bounds;
            bullets.Prototype.FillColor = textColor;
            bullets.Prototype.Radius = graphics.Height / 15;
            bullets.SurfacePercentage = graphics.Height / 12;
            bullets.DrawFast(graphics);
        }
	}
}
