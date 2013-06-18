using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Radar Captcha Image Generator
    /// </summary>
    internal class RadarImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(0, 0, 0); // black background
            textColor = null;
            outlineColor = Color.Rgb(170, 230, 40); // yellowish green radar screen
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                outlineColor = customLightColor;
            }

            if (null != customDarkColor)
            {
                backColor = customDarkColor;
            }
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // individual glyphs are spread out a bit more
            transform.Scaling.xScalingPercentageRange = new RandomRange(92, 97);
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thick text outline
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(2, outlineColor)
            );
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // slightly below the image
            Point center = new Point(graphics.Bounds).Frozen;
            center.Y += 2 * graphics.Height;

            // radar coordinate lines = circles + spokes

            // thin coordinate concentric circles
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, outlineColor)
            );
            circles.Spacing = 10;
            circles.Prototype.Center = center;
            circles.DrawFast(graphics);

            /*// thicker coordinate concentric circles, every 10th line
            ConcentricCircles circles2 = new ConcentricCircles();
            circles2.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(2, outlineColor)
            );
            circles2.Spacing = 63;
            circles2.Prototype.Center = center;
            circles2.DrawFast(graphics);*/

            // thicker spoke lines, randomly spaced
            SpokeLines spokes = new SpokeLines();
            spokes.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(2, outlineColor)
            );
            spokes.AngleDelta = RandomGenerator.Next(15, 18);
            spokes.Center = center;
            spokes.Draw(graphics);

            // ripple distortion
            Wave wave = new Wave();
            wave.Level = 2;
            wave.Apply(graphics);
        }
	}
}
