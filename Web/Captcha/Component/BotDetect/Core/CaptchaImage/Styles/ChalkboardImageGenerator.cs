using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Chalkboard Captcha Image Generator
    /// </summary>
    internal class ChalkboardImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(0, 0, 0); // black background
            textColor = null; // transparent text body
            outlineColor = Color.Rgb(255, 255, 255); // white text outline
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            // CustomLightColor is used as text outline color
            if (null != customLightColor)
            {
                outlineColor = customLightColor;
            }

            // CustomDarkColor is used as image background color
            if (null != customDarkColor)
            {
                backColor = customDarkColor;
            }
        }


        protected override void DrawBackground()
        {
            base.DrawBackground();

            // white spacer lines painted on the blackboard 
            HorizontalLines lines = new HorizontalLines();
            lines.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(1, graphics.Height / 30), outlineColor)
            );
            lines.SpacingRange = new RandomRange(graphics.Height / 3, graphics.Height / 2);
            lines.Draw(graphics);

            // flashes
            VerticalLines vLines = new VerticalLines();
            vLines.SpacingRange = new RandomRange(graphics.Height, graphics.Height * 2);
            vLines.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(2, graphics.Height / 20), outlineColor)
            );
            vLines.Draw(graphics);

            // some breaks in the lines to segment them
            RandomCircles holes = new RandomCircles();
            holes.Prototype.FillColor = backColor;
            holes.Prototype.RadiusRange = new RandomRange(1, graphics.Height / 15 + 1);
            holes.SurfacePercentage = 10;
            holes.DrawFast(graphics);

            // chalk "smudges"
            RandomDots dots1 = new RandomDots();
            dots1.Prototype.FillColor = outlineColor;
            dots1.SurfacePercentage = 1;
            dots1.DrawFast(graphics);
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // a bit thicker white chalk text outline
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(2, graphics.Height / 20), outlineColor)
            );
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // horizontal narrowing and vertical expansion
            transform.Scaling.xScalingPercentageRange = new RandomRange(92, 97);
            transform.Scaling.yScalingPercentageRange = new RandomRange(100, 110);
        }


        protected override void DrawEffects()
        {
            base.DrawEffects();

            // water "drops" washing away random spots on the chalkboard
            RandomCircles holes = new RandomCircles();
            holes.Prototype.FillColor = backColor;
            holes.Prototype.RadiusRange = new RandomRange(1, graphics.Height / 15 + 1);
            holes.SurfacePercentage = 4;
            holes.DrawFast(graphics);

            // horizontal-only fuzz distortion to simulate chalk dissipation
            Fuzz fuzz = new Fuzz();
            fuzz.Horizontal = true;
            fuzz.Vertical = false;
            fuzz.Level = 1;
            fuzz.Apply(graphics);
        }
	}
}
