using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Ghostly Captcha Image Generator
    /// </summary>
    internal class GhostlyImageGenerator : ImageGenerator, IImageGenerator
	{
        // ghostly gradient outline
        protected LineStyle outline;

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // randomized light text on dark background color scheme
            
            textColor = Color.BetweenRgb(200, 200, 200).AndRgb(250, 250, 250).Frozen; 
            backColor = textColor.Complement;
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                textColor = Color.Randomized(customLightColor, 50).Frozen;
            }

            if (null != customDarkColor)
            {
                backColor = customDarkColor;
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            InitGradientOutline();

            textRenderer.Prototype.Outline = outline;
            textRenderer.Prototype.FillColor = backColor;
        }

        protected void InitGradientOutline()
        {
            outline = new LineStyle();

            // we draw the layers from the most outer one to the most inner one
            // the outer layers color is more similar to the background color 
            // the inner layers color is more similar to the glyph color 
            int gradientLayers = graphics.Height / 5;

            for (int i = 0; i < gradientLayers; i++)
            {
                int curRed = backColor.R + ((textColor.R - backColor.R) / gradientLayers) * (gradientLayers - i);
                int curGreen = backColor.G + ((textColor.G - backColor.G) / gradientLayers) * (gradientLayers - i);
                int curBlue = backColor.B + ((textColor.B - backColor.B) / gradientLayers) * (gradientLayers - i);

                outline[i] = LineLayer.Solid(1, Color.Rgb(curRed, curGreen, curBlue));
            }
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // ghost lights
            RandomCircles lights = new RandomCircles();
            lights.Prototype.FillColor = null;
            lights.Prototype.Outline = outline;
            lights.Prototype.Radius = graphics.Height / 7;
            lights.SurfacePercentage = 20;
            lights.Draw(graphics);
        }
    }
}
