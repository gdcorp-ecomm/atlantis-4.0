using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Circles Captcha Image Generator
    /// </summary>
    internal class CirclesImageGenerator : ImageGenerator, IImageGenerator
	{
        // random circles inverting the color scheme
        ShapeCollection circleBounds = new ShapeCollection();

        protected override void CustomInit()
        {
            // random circle generation
            int drawnSurface = 0;
            Rectangle bounds = textRenderer.Bounds;
            while (drawnSurface < graphics.Surface / 2)
            {
                RandomRange radiusRange = new RandomRange(graphics.Height / 4, graphics.Height / 2);
                Circle circle = new Circle(new Point(bounds).Frozen, radiusRange.Next);
                circleBounds.Add(circle);

                drawnSurface += circle.Surface;
            }
        }

        protected override void DrawBackground()
        {
            // white image background
            graphics.Fill(backColor);

            // black circle background
            graphics.Fill(textColor, circleBounds);
        }

        protected override void InitFonts()
        {
            base.InitFonts();

            // use a fixed font family, so the black and white fragments are not
            // too different and hard to read
            FontCollection fontSelection = new FontCollection();
            fontSelection[0] = Font.From("Courier New", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[1] = Font.From("Times New Roman", FontCase.Uppercase, FontWeight.Normal);
            fontSelection[2] = Font.From("Microsoft Sans Serif", FontCase.Uppercase, FontWeight.Normal);

            fonts = fontSelection;
            fonts.RandomizationStyle = RandomizationStyle.RandomizeOnce;
        }

        protected override void InitGlyphTransform()
        {
            // reduced glyph transformation set to make the character fragments easier to read
            transform = Transform.None;
            transform.Scaling.xScalingPercentage = 98;
            transform.Rotation.Angle = new RandomRange(-5, 5).Frozen;
        }

        protected override void DrawText()
        {
            // black letters on white background
            textRenderer.Prototype.FillColor = textColor;
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 20, textColor)
            );
            textRenderer.Draw(graphics);

            // white letters on black background
            textRenderer.Prototype.FillColor = backColor;
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 20, backColor)
            );
            textRenderer.Draw(graphics, circleBounds);
        }
    }
}