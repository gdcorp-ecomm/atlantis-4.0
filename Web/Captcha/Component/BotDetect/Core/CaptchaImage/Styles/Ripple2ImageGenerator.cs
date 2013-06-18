using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Ripple2 Captcha Image Generator
    /// </summary>
    internal class Ripple2ImageGenerator : RippleImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // randomized light text on dark background color scheme
            textColor = Color.BetweenRgb(100, 100, 100).AndRgb(200, 200, 200);
            backColor = Color.Rgb(0, 0, 0);
        }

        protected override void InitFonts()
        {
            base.InitFonts();

            // bold fonts for easier reading
            FontCollection fontSelection = new FontCollection();
            fontSelection[0] = Font.From("Arial", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[2] = Font.From("Microsoft Sans Serif", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[3] = Font.From("Times New Roman", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[4] = Font.From("Tahoma", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[5] = Font.From("Verdana", FontCase.Uppercase, FontWeight.Bold);

            fonts = fontSelection;
        }

        protected override void InitGradientOutline()
        {
            // randomized color lines
            outline = new LineStyle();
            int gradientLayers = 3;
            for (int i = 0; i < gradientLayers; i++)
            {
                int thickness = graphics.Height / 20;
                outline[i] = LineLayer.Solid(thickness, textColor);
            }
        }
    }
}
