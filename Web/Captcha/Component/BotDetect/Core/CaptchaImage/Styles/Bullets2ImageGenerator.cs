using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Bullets2 Captcha Image Generator
    /// </summary>
    internal class Bullets2ImageGenerator : BulletsImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // individual characters and wires have a random color
            textColor = Color.BetweenRgb(25, 25, 25).AndRgb(125, 125, 125);

            // background has a fixed value in the complementary range
            backColor = Color.BetweenRgb(155, 155, 155).AndRgb(205, 205, 205).Frozen;
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            // background color is randomized around the CustomLightColor
            if (null != customLightColor)
            {
                backColor = Color.Randomized(customLightColor, 50).Frozen;
            }

            // text color is randomized around the CustomDarkColor
            if (null != customDarkColor)
            {
                textColor = Color.Randomized(customDarkColor, 100);
            }
        }
	}
}
