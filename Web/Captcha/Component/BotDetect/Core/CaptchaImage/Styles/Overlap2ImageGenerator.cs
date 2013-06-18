using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Overlap2 Captcha Image Generator
    /// </summary>
    internal class Overlap2ImageGenerator : OverlapImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(255, 255, 255);
            textColor = null; // transparent text body
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
            }

            if (null != customDarkColor)
            {
                outlineColor = customDarkColor;
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // custom outline resulting in thicker outlined glyphs
            textRenderer.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(2, outlineColor), 
                LineLayer.Solid(2, backColor)
            );
        }
    }
}
