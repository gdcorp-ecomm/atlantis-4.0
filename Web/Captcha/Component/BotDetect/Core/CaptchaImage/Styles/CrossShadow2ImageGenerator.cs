using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// CrossShadow2 Captcha Image Generator
    /// </summary>
    internal class CrossShadow2ImageGenerator : CrossShadowImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // white background
            backColor = Color.Rgb(255, 255, 255);

            // dark random letter color
            textColor = Color.BetweenRgb(30, 30, 30).AndRgb(100, 100, 100);

            // lighter random shadow color
            shadowColor = textColor.Complement;
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = Color.Randomized(customLightColor, 50).Frozen;
            }

            if (null != customDarkColor)
            {
                textColor = Color.Randomized(customDarkColor, 70);
            }

            if (null != customLightColor || null != customDarkColor)
            {
                shadowColor = Color.Median(backColor, textColor);
            }
        }
    }
}