using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Stitch Captcha Image Generator
    /// </summary>
    internal class StitchImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            textColor = null; // transparent text body
            outlineColor = Color.Rgb(0, 0, 0); // black outline
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thicker glyph outline in text color for "bolder" text
            textRenderer.Prototype.Outline = LineStyle.Single(LineLayer.Solid(2, outlineColor));
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // individual glyphs are extra expanded horizontally and vertically
            transform.Scaling.xScalingPercentageRange = new RandomRange(100, 110);
            transform.Scaling.yScalingPercentageRange = new RandomRange(100, 110);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // canvas-like thick grid
            HorizontalLines hLines = new HorizontalLines();
            hLines.Spacing = 3;
            hLines.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, backColor));
            hLines.Draw(graphics);

            VerticalLines vLines = new VerticalLines();
            vLines.Spacing = 3;
            vLines.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, backColor));
            vLines.Draw(graphics);
        }
    }
}
