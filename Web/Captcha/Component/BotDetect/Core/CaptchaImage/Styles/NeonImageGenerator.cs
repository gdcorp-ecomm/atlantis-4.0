using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Neon Captcha Image Generator
    /// </summary>
    internal class NeonImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(0, 0, 0); // black background
            textColor = Color.BetweenRgb(150, 155, 100).AndRgb(255, 255, 255).Frozen; // light colorful neon light
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

        protected override void InitFonts()
        {
            base.InitFonts();

            // bold fonts for easier reading
            FontCollection fontSelection = new FontCollection();
            fontSelection[0] = Font.From("Arial", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[2] = Font.From("Microsoft Sans Serif", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[4] = Font.From("Tahoma", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[5] = Font.From("Verdana", FontCase.Uppercase, FontWeight.Bold);

            fonts = fontSelection;
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thicker outline
            textRenderer.Prototype.FillColor = backColor;
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 15, textColor)
            );
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // random flashes connecting the letter outlines randomly
            RandomLines flashes = new RandomLines();
            flashes.Prototype.Outline = LineStyle.Single(LineLayer.Solid(graphics.Height / 15, textColor));
            flashes.Prototype.Transform.Rotation.Angle = new RandomRange(-5, 5).Frozen;
            flashes.SurfacePercentage = 10;
            flashes.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // electrical-like horizontal displacement
            Fuzz fuzz = new Fuzz();
            fuzz.Horizontal = true;
            fuzz.Vertical = false;
            fuzz.Level = 1;
            fuzz.Apply(graphics);
        }

	}
}
