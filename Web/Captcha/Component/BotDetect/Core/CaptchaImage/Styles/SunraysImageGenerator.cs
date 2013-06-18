using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Sunrays Captcha Image Generator
    /// </summary>
    internal class SunraysImageGenerator : ImageGenerator, IImageGenerator
	{
        protected Circle sun;

        protected ShapeCollection corona;

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            textColor = Color.Rgb(50, 50, 50); // dark gray text
            backColor = textColor.Complement; // lighter background
        }

        protected override void InitFonts()
        {
            base.InitFonts();

            // use a fixed font family, so the black and white fragments are not
            // too different and hard to read
            fonts.RandomizationStyle = RandomizationStyle.RandomizeOnce;
        }

        protected override void InitGlyphTransform()
        {
            // reduced glyph transformation set to make the character fragments easier to read
            transform = Transform.None;
        }

        protected override void CustomInit()
        {
            // place the "sun" in the central region of the image
            int minX = graphics.Bounds.TopLeft.X + graphics.Width / 3;
            int maxX = graphics.Bounds.BottomRight.X - graphics.Width / 3;

            int minY = graphics.Bounds.TopLeft.Y + graphics.Height / 3;
            int maxY = graphics.Bounds.BottomRight.Y - graphics.Height / 3;

            Point center = Point.Between(minX, minY).And(maxX, maxY).Frozen;
            sun = new Circle(center, graphics.Height);

            corona = new ShapeCollection();
            corona[0] = sun;
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // sunrays emanating from the "sun"
            SpokeLines lines = new SpokeLines();
            lines.Center = sun.Center;
            lines.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(RandomGenerator.Next(graphics.Height / 20, graphics.Height / 10), textColor)
            );
            lines.Draw(graphics);

            // except the "sun" itself, which we paint over
            graphics.Fill(textColor, corona);
        }

        protected override void DrawText()
        {
            // original text outside the "sun"
            textRenderer.Draw(graphics);

            // inverted text inside the "sun"
            textRenderer.Prototype.FillColor = backColor;
            textRenderer.Draw(graphics, corona);
        }
    }
}
