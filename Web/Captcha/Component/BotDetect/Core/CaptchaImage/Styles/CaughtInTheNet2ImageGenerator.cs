using System;
using System.Collections;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// CaughtInTheNet2 Captcha Image Generator
    /// </summary>
    internal class CaughtInTheNet2ImageGenerator : CaughtInTheNetImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // individual characters have a random color
            textColor = Color.BetweenRgb(155, 155, 155).AndRgb(255, 255, 255);

            // background has a fixed value in the complementary range
            backColor = Color.BetweenRgb(25, 25, 25).AndRgb(75, 75, 75).Frozen;
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            // text fill color is randomized around the CustomLightColor
            if (null != customLightColor)
            {
                textColor = Color.Randomized(customLightColor, 100);
            }

            // background color is randomized around the CustomDarkColor
            if (null != customDarkColor)
            {
                backColor = Color.Randomized(customDarkColor, 50).Frozen;
            }
        }
    }		
}