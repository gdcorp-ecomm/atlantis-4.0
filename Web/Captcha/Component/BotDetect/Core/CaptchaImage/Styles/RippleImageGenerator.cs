using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Ripple Captcha Image Generator
    /// </summary>
    internal class RippleImageGenerator : ImageGenerator, IImageGenerator
	{
        // color gradient outline
        protected LineStyle outline;

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // randomized color scheme
            textColor = Color.BetweenRgb(0, 0, 0).AndRgb(60, 60, 60).Frozen;
            backColor = textColor.Complement.Frozen;
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

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            InitGradientOutline();

            // transparent narrowed glyphs
            textRenderer.Prototype.FillColor = backColor;
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, backColor)
            );
            textRenderer.Prototype.Transform.Scaling.yScalingPercentageRange = new RandomRange(100, 110);
            textRenderer.Prototype.Transform.Scaling.xScalingPercentageRange = new RandomRange(85, 95);
        }

        protected virtual void InitGradientOutline()
        {
            outline = new LineStyle();

            // gradually change from the text color to the background color
            int gradientLayers = 4;
            for (int i = 0; i < gradientLayers; i++)
            {
                int curRed = backColor.R + ((textColor.R - backColor.R) / gradientLayers) * (gradientLayers - i);
                int curGreen = backColor.G + ((textColor.G - backColor.G) / gradientLayers) * (gradientLayers - i);
                int curBlue = backColor.B + ((textColor.B - backColor.B) / gradientLayers) * (gradientLayers - i);

                outline[i] = LineLayer.Solid(graphics.Height / 20, Color.Rgb(curRed, curGreen, curBlue));

                if (gradientLayers -1 == i)
                {
                    outlineColor = Color.Rgb(curRed, curGreen, curBlue);
                }
            }
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // horizontal "waves"
            HorizontalLines hLines = new HorizontalLines();
            hLines.SpacingRange = new RandomRange(graphics.Height / 8 + 1, graphics.Height / 6 + 1);
            hLines.Prototype.Outline = outline;
            hLines.Draw(graphics);

            // ripple distortion
            Wave wave = new Wave();
            wave.Level = RandomGenerator.Next(2, 5);
            wave.OverflowColor = backColor;
            wave.Apply(graphics);
        }
	}
}
