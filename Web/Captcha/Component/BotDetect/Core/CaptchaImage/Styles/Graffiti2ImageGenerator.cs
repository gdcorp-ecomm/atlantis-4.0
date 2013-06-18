using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Graffiti2 Captcha Image Generator
    /// </summary>
    internal class Graffiti2ImageGenerator : GraffitiImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            textColor = null; // transparent text

            switch (coin)
            {
                case 0:
                    outlineColor = Color.BetweenRgb(80, 0, 0).AndRgb(155, 0, 0);
                    break;
                case 1:
                    outlineColor = Color.BetweenRgb(0, 80, 0).AndRgb(0, 155, 0);
                    break;
                case 2:
                    outlineColor = Color.BetweenRgb(0, 0, 80).AndRgb(0, 0, 155);
                    break;
            }

            fillColor = outlineColor;
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
                switch (coin)
                {
                    case 0:
                        outlineColor = Color.RandomizedR(customDarkColor, 70);
                        break;
                    case 1:
                        outlineColor = Color.RandomizedG(customDarkColor, 70);
                        break;
                    case 2:
                        outlineColor = Color.RandomizedB(customDarkColor, 70);
                        break;
                }

                fillColor = outlineColor;
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // custom outline
            textRenderer.Prototype.Outline = LineStyle.Single(LineLayer.Solid(2, outlineColor));
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // a bit wider overlapping letters
            transform.Scaling.xScalingPercentageRange = new RandomRange(115, 120);
        }
    }
}
