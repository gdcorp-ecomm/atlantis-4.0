using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Vertigo Captcha Image Generator
    /// </summary>
    internal class VertigoImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.BetweenRgb(175, 175, 175).AndRgb(255, 255, 255).Frozen; // random lighter background
            textColor = Color.BetweenRgb(35, 35, 35).AndRgb(135, 135, 135); // random darker text
            outlineColor = textColor;
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = Color.Randomized(customLightColor, 50);
            }

            if (null != customDarkColor)
            {
                textColor = Color.Randomized(customDarkColor, 100);
                outlineColor = textColor;
            }
        }


        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thicker glyph outline in text color for "bolder" text
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(1, graphics.Height / 30), textColor)
            );
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // thick vertical "flashes"
            VerticalLines thickLines = new VerticalLines();
            thickLines.SpacingRange = new RandomRange(graphics.Height / 2, graphics.Height * 3 / 2);
            thickLines.Prototype.Transform.Rotation.AngleRange = new RandomRange(-10, 10);
            thickLines.Prototype.Transform.Scaling.xScalingPercentageRange = new RandomRange(100, 120);
            thickLines.Prototype.Transform.Scaling.yScalingPercentageRange = new RandomRange(80, 100);
            thickLines.Prototype.Outline = LineStyle.Single(LineLayer.Solid(graphics.Height / 15, textColor));
            thickLines.Draw(graphics);

            // thin vertical "flashes"
            VerticalLines thinLines = new VerticalLines();
            thinLines.SpacingRange = new RandomRange(graphics.Height / 2, graphics.Height * 3 / 2);
            thinLines.Prototype.Transform.Rotation.AngleRange = new RandomRange(-30, 30);
            thinLines.Prototype.Outline = LineStyle.Single(LineLayer.Solid(graphics.Height / 20, textColor));
            thinLines.Draw(graphics);

            // concentric vertigo "waves"
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(1, graphics.Height / 30), backColor)
            );
            circles.Prototype.Center = new Point(graphics.Bounds).Frozen;
            circles.Prototype.Center.Y += graphics.Height;
            circles.Spacing = Math.Max(1, graphics.Height / 30) + 2;
            circles.DrawFast(graphics);
        }
    }
}
