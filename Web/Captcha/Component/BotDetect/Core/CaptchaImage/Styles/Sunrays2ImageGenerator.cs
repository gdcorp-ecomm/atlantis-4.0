using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Sunrays2 Captcha Image Generator
    /// </summary>
    internal class Sunrays2ImageGenerator : SunraysImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            textColor = Color.BetweenRgb(25, 25, 25).AndRgb(125, 125, 125); // dark sun color
            backColor = Color.BetweenRgb(180, 180, 180).AndRgb(230, 230, 230).Frozen; // light color
        }

        protected override void OverrideColors()
        {
            // apply user-defined color scheme
            if (null != customLightColor)
            {
                backColor = Color.Randomized(customLightColor, 50);
            }

            if (null != customDarkColor)
            {
                textColor = Color.Randomized(customDarkColor, 100);
            }
        }
    }
}
