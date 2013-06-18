using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Corrosion Captcha Image Generator
    /// </summary>
    internal class CorrosionImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(192, 192, 192); // dull gray
            textColor = Color.Rgb(165, 45, 0); // rusty red
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // "spinal" wire
            Rectangle bounds = textRenderer.Bounds;
            int thickness = RandomGenerator.Next(bounds.Height / 15, bounds.Height / 10);
            Line wire = new Line(
                Point.Between(bounds.TopLeft.X, bounds.TopLeft.Y).And(bounds.TopLeft.X + bounds.Height, bounds.TopLeft.Y + bounds.Height),
                Point.Between(bounds.BottomRight.X - bounds.Height, bounds.BottomRight.Y - bounds.Height).And(bounds.BottomRight.X, bounds.BottomRight.Y),
                LineStyle.Single(LineLayer.Solid(thickness, textColor))
            );
            wire.Draw(graphics);

            // vertical water "trails"
            VerticalLines trails = new VerticalLines();
            trails.Bounds = textRenderer.Bounds;
            trails.SpacingRange = new RandomRange(graphics.Height / 2, graphics.Height);
            trails.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, backColor));
            trails.Draw(graphics);

            // rust fragments on the surface
            RandomBeziers rust = new RandomBeziers();
            rust.Bounds = textRenderer.Bounds;
            rust.ScalingPercentage = 5;
            rust.SurfacePercentage = 10;
            rust.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, textColor));
            rust.DrawFast(graphics);

            // rust holes in the text
            RandomBeziers holes = new RandomBeziers();
            holes.Bounds = textRenderer.Bounds;
            holes.ScalingPercentage = 5;
            holes.SurfacePercentage = 10;
            holes.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, backColor));
            holes.DrawFast(graphics);    
        }
    }
}
