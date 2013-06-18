using System;
using System.Collections;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// ThickThinLines2 Captcha Image Generator
    /// </summary>
    internal class ThickThinLines2ImageGenerator : ThickThinLinesImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(255, 255, 255);
            textColor = Color.BetweenRgb(50, 50, 50).AndRgb(180, 180, 180);
            lineColor = Color.BetweenRgb(55, 55, 55).AndRgb(255, 255, 255);
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
                textColor = Color.Randomized(customDarkColor, 70);
                outlineColor = textColor;
                lineColor = Color.Randomized(customDarkColor, 150);
            }
        }
    }
}
