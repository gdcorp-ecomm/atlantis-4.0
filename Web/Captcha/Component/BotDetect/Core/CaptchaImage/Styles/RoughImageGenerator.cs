using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Rough Captcha Image Generator
    /// </summary>
    internal class RoughImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(255, 255, 255); // white
            textColor = Color.Rgb(180, 180, 180); // light gray
            outlineColor = Color.Rgb(60, 60, 60); // dark gray
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
                textColor = customDarkColor;
                outlineColor = Color.Darkened(customDarkColor, 50);
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // a bit thicker dark outline
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(2, outlineColor)
            );
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // wall texture noise
            RandomCircles dots1 = new RandomCircles();
            dots1.Bounds = textRenderer.Bounds;
            dots1.Prototype.Radius = 2;
            dots1.Prototype.FillColor = textColor;
            dots1.SurfacePercentage = 8;
            dots1.DrawFast(graphics);

            RandomCircles dots2 = new RandomCircles();
            dots2.Bounds = textRenderer.Bounds;
            dots2.Prototype.Radius = 2;
            dots2.Prototype.FillColor = backColor;
            dots2.SurfacePercentage = 8;
            dots2.DrawFast(graphics);

            // age-chipped noise
            RandomBeziers curves2 = new RandomBeziers();
            curves2.Bounds = textRenderer.Bounds;
            curves2.ScalingPercentage = 6;
            curves2.SurfacePercentage = 6;
            curves2.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(2, textColor)
            );
            curves2.DrawFast(graphics);

            RandomBeziers curves1 = new RandomBeziers();
            curves1.Bounds = textRenderer.Bounds;
            curves1.ScalingPercentage = 6;
            curves1.SurfacePercentage = 6;
            curves1.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(2, backColor)
            );
            curves1.DrawFast(graphics);    
        }
    }
}
