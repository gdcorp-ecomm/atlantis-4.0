using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Darts Captcha Image Generator
    /// </summary>
    internal class DartsImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // random text color
            textColor = Color.BetweenRgb(0, 0, 0).AndRgb(80, 80, 80);

            // lighter background color
            backColor = textColor.Complement.Frozen;
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // random dark arcs in the background
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(graphics.Height / 20, textColor),
                LineLayer.Solid(graphics.Height / 20, backColor)
            );
            circles.Prototype.Center = new Point(graphics.Bounds);
            circles.Prototype.Center.Y += 2 * graphics.Height;
            circles.Spacing = graphics.Height / 3;
            circles.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // concentric circles as noise
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, backColor));
            circles.Spacing = 3;
            circles.Prototype.Center = GetCenter();

            circles.DrawFast(graphics);
        }

        protected Point GetCenter()
        {
            // place the center somewhere in the central region of the image
            int minX = graphics.Bounds.TopLeft.X + graphics.Width / 3;
            int maxX = graphics.Bounds.BottomRight.X - graphics.Width / 3;

            int minY = graphics.Bounds.TopLeft.Y + graphics.Height / 3;
            int maxY = graphics.Bounds.BottomRight.Y - graphics.Height / 3;

            return Point.Between(minX, minY).And(maxX, maxY).Frozen;
        }
    }
}
