using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Mass Captcha Image Generator
    /// </summary>
    internal class MassImageGenerator : ImageGenerator, IImageGenerator
	{
        // glyph color
        protected Color letterColor
        {
            get
            {
                return colors["letterColor"];
            }
            set
            {
                colors["letterColor"] = value;
            }
        }

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(255, 255, 220); // yellowish background
            textColor = Color.BetweenRgb(0, 0, 0).AndRgb(150, 150, 150); // darker text
            letterColor = Color.BetweenRgb(170, 170, 170).AndRgb(250, 250, 250); // lighter text
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
                textColor = Color.Randomized(customDarkColor, 75);
                letterColor = Color.Randomized(Color.SaturationAdjusted(Color.Lightened(customDarkColor, 1.5f), 0.5f), 75);
            }
        }

        // noise renderer, re-used for glyph noise layers
        protected RandomGlyphs noiseGlyphs = new RandomGlyphs();

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // heavily transformed noise glyphs
            noiseGlyphs.Fonts = fonts;
            noiseGlyphs.Prototype.FillColor = letterColor;
            noiseGlyphs.Text = text;
            noiseGlyphs.Bounds = textRenderer.Bounds;
            noiseGlyphs.Prototype.Transform = new Transform();
            noiseGlyphs.Prototype.Transform.Rotation.AngleRange = new RandomRange(-30, 30);
            noiseGlyphs.Prototype.Transform.Warp.WarpPercentageRange = new RandomRange(5, 20);
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // dark background glyphs
            noiseGlyphs.Prototype.FillColor = textColor;
            noiseGlyphs.SurfacePercentage = 12;
            noiseGlyphs.Draw(graphics);

            // light background glyphs
            noiseGlyphs.Prototype.FillColor = letterColor;
            noiseGlyphs.SurfacePercentage = 40;
            noiseGlyphs.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // light foreground glyphs
            noiseGlyphs.SurfacePercentage = 12;
            noiseGlyphs.Draw(graphics);
        }
    }
}
