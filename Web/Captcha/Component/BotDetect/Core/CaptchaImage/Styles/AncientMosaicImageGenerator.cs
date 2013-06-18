using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
	/// <summary>
	/// AncientMosaic Captcha Image Generator
	/// </summary>
    internal class AncientMosaicImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(245, 245, 245); // near-white background
            textColor = Color.Rgb(200, 200, 200); // gray text body
            outlineColor = Color.Rgb(30, 30, 30); // near-black text outline
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            // use the CustomLightColor as image background
            if (null != customLightColor)
            {
                backColor = customLightColor;
            }

            // use the CustomDarkColor for text body, and a darker shade
            // for text outline
            if (null != customDarkColor)
            {
                textColor = customDarkColor;
                outlineColor = Color.Darkened(customDarkColor, 50);
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // custom glyph outline style 
            int shadowThickness = graphics.Height / 15; // shadow scaling
            textRenderer.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(1, outlineColor), // a thin dark outline for readability
                LineLayer.Solid(shadowThickness, textColor) // a thick gray outline as shadow / blur
            );
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // random dots and flecks to simulate aged wall surface

            RandomDots darkDots = new RandomDots();
            darkDots.Bounds = textRenderer.Bounds;
            darkDots.Prototype.FillColor = outlineColor;
            darkDots.SurfacePercentage = 3;
            darkDots.DrawFast(graphics);

            RandomCircles darkFlecks = new RandomCircles();
            darkFlecks.Prototype.Radius = 2;
            darkFlecks.Prototype.FillColor = textColor;
            darkFlecks.SurfacePercentage = 16;
            darkFlecks.DrawFast(graphics);
        }

	}
}
