using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Strippy Captcha Image Generator
    /// </summary>
    internal class StrippyImageGenerator : ImageGenerator, IImageGenerator
	{
        // colorful lines
        protected Color lineColor
        {
            get
            {
                return colors["lineColor"];
            }
            set
            {
                colors["lineColor"] = value;
            }
        }

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors(); 

            // randomized light background
            backColor = Color.BetweenRgb(200, 200, 200).AndRgb(255, 255, 255).Frozen;

            // randomized colorful lines
            lineColor = Color.BetweenRgb(55, 55, 55).AndRgb(255, 255, 255);

            // text is not actually shown in the image
            textColor = lineColor;
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = Color.Randomized(customLightColor, 50).Frozen;
                textColor = backColor;
            }

            if (null != customDarkColor)
            {
                lineColor = Color.Randomized(customDarkColor, 150);
            }
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // individual glyphs are slightly expanded horizontally so they overlap
            transform.Scaling.xScalingPercentage = 105;
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thick light-colored outline 
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 20, lineColor)
            );
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // distort the text
            Wave wave = new Wave();
            wave.Level = 2;
            wave.OverflowColor = backColor;
            wave.Apply(graphics);

            // colorful stripes within text bounds
            HorizontalLines lines = new HorizontalLines();
            lines.Spacing = graphics.Height / 8;
            lines.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 8, lineColor)
            );
            lines.Draw(graphics, textRenderer);
        }
    }
}
