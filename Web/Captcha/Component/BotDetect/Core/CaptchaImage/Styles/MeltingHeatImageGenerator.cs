using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// MeltingHeat Captcha Image Generator
    /// </summary>
    internal class MeltingHeatImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // randomized flame color
            textColor = Color.BetweenRgb(255, 100, 0).AndRgb(255, 150, 0);

            backColor = Color.Rgb(0, 0, 0); // black bacground
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                textColor = Color.RandomizedR(customLightColor, 50);
            }

            if (null != customDarkColor)
            {
                backColor = customDarkColor;
            }
        }

        protected override void DrawText()
        {
            DrawBackgroundText();

            base.AddTrademark();

            DrawForegroundText();
        }

        protected virtual void DrawBackgroundText()
        {
            // 2-layer thick outline to make the flame halo big enough
            textRenderer.Prototype.FillColor = textColor;
            textRenderer.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(graphics.Height / 12, textColor), 
                LineLayer.Solid(graphics.Height / 12, textColor)
            );
            textRenderer.Draw(graphics);

            // create the text halo outline
            Halo halo = new Halo();
            halo.Level = 1;
            halo.Apply(graphics);
        }

        protected virtual void DrawForegroundText()
        {
            // transparent glyph body
            textRenderer.Prototype.FillColor = backColor;
            textRenderer.Prototype.Outline = null;
            textRenderer.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            // extra flame-like distortion
            Wave wave = new Wave();
            wave.Level = 2;
            wave.OverflowColor = backColor;
            wave.Apply(graphics);
        }
    }
}
