using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Overlap Captcha Image Generator
    /// </summary>
    internal class OverlapImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(255, 255, 255); // white background
            textColor = Color.Rgb(255, 255, 255); // filled white text body
            outlineColor = Color.Rgb(0, 0, 0); // black outline
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = customLightColor;
                textColor = customLightColor;
            }

            if (null != customDarkColor)
            {
                outlineColor = customDarkColor;
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thin glyph outline
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(2, outlineColor)
            );
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // individual glyphs are extra expanded horizontally so they overlap
            transform.Scaling.xScalingPercentage = 135;
        }
    }
}
