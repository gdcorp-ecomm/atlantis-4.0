using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Halo Captcha Image Generator
    /// </summary>
    internal class HaloImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            textColor = Color.Rgb(200, 210, 225); // light bluish gray
            backColor = Color.Rgb(0, 0, 0); // black bacground
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                textColor = customLightColor;
            }

            if (null != customDarkColor)
            {
                backColor = customDarkColor;
            }
        }

        protected override void DrawText()
        {
            DrawBackgroundText();
            DrawForegroundText();
        }

        protected void DrawBackgroundText()
        {
            // thick outline to generate a big enough halo 
            textRenderer.Prototype.FillColor = textColor;
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 7, textColor)
            );
            textRenderer.Draw(graphics);

            // create the halo outline
            Halo halo = new Halo();
            halo.Level = 1;
            halo.Apply(graphics);

            base.AddTrademark();
        }

        protected void DrawForegroundText()
        {
            // transparent glyph body
            textRenderer.Prototype.FillColor = backColor;
            textRenderer.Prototype.Outline = null;
            textRenderer.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // noise
            RandomDots freckles = new RandomDots();
            freckles.Bounds = textRenderer.Bounds;
            freckles.Prototype.FillColor = backColor;
            freckles.SurfacePercentage = 1;
            freckles.DrawFast(graphics);

            RandomCircles flare = new RandomCircles();
            flare.Bounds = textRenderer.Bounds;
            flare.Prototype.RadiusRange = new RandomRange(1, graphics.Height / 10);
            flare.Prototype.FillColor = textColor;
            flare.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(2, Color.Darkened(textColor, 50))
            );
            flare.SurfacePercentage = 2;
            flare.DrawFast(graphics);
        }
    }
}