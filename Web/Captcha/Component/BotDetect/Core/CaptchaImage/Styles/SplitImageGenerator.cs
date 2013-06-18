using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Split Captcha Image Generator
    /// </summary>
    internal class SplitImageGenerator : ImageGenerator, IImageGenerator
	{
        // grid color
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

            backColor = Color.Rgb(210, 210, 210); // light gray
            textColor = Color.Rgb(0, 0, 0); // black
            lineColor = Color.Rgb(255, 255, 255); // white
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = customLightColor;
                lineColor = Color.Lightened(customLightColor, 1.33f);
            }

            if (null != customDarkColor)
            {
                textColor = customDarkColor;
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thicker glyph outline
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, textColor)
            );
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // individual glyphs are expanded vertically
            transform.Scaling.yScalingPercentageRange = new RandomRange(100, 110);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // foreground grid
            HorizontalLines hLines = new HorizontalLines();
            hLines.SpacingRange = new RandomRange(graphics.Height / 3, graphics.Height / 2);
            hLines.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 20, lineColor)
            );
            hLines.Draw(graphics);

            VerticalLines vLines = new VerticalLines();
            vLines.SpacingRange = new RandomRange(graphics.Height / 2, 2 * graphics.Height / 3);
            vLines.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 20 + 1, lineColor)
            );
            vLines.Draw(graphics);
        }
    }
}
