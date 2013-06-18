using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Electric Captcha Image Generator
    /// </summary>
    internal class ElectricImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // randomized electric arc color
            textColor = Color.BetweenRgb(175, 175, 175).AndRgb(255, 255, 255).Frozen;

            backColor = Color.Rgb(0, 0, 0); // black bacground
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

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // custom outline
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, textColor)
            );
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // electric grid background
            HorizontalLines hLines = new HorizontalLines();
            hLines.SpacingRange = new RandomRange(graphics.Height / 4, graphics.Height / 2);
            hLines.Prototype.Outline = LineStyle.Single(LineLayer.Solid(graphics.Height / 20, textColor));
            hLines.Prototype.Transform.Rotation.Angle = new RandomRange(-5, 5).Frozen;
            hLines.Draw(graphics);

            VerticalLines vLines = new VerticalLines();
            vLines.SpacingRange = new RandomRange(graphics.Height / 4, graphics.Height);
            vLines.Prototype.Outline = LineStyle.Single(LineLayer.Solid(graphics.Height / 20, textColor));
            vLines.Prototype.Transform.Rotation.Angle = new RandomRange(-10, 10).Frozen;
            vLines.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // horizontal-only fuzz distortion to simulate current dissipation
            Fuzz fuzz = new Fuzz();
            fuzz.Horizontal = false;
            fuzz.Vertical = true;
            fuzz.Level = 1;
            fuzz.Apply(graphics);
        }

	}
}
