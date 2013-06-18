using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Graffiti Captcha Image Generator
    /// </summary>
    internal class GraffitiImageGenerator : ImageGenerator, IImageGenerator
	{
        protected int coin = RandomGenerator.Next(0, 3);

        // wall color
        protected Color fillColor
        {
            get
            {
                return colors["fillColor"];
            }
            set
            {
                colors["fillColor"] = value;
            }
        }

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            switch (coin)
            {
                case 0:
                    textColor = Color.BetweenRgb(80, 0, 0).AndRgb(155, 0, 0).Frozen;
                    break;
                case 1:
                    textColor = Color.BetweenRgb(0, 80, 0).AndRgb(0, 155, 0).Frozen;
                    break;
                case 2:
                    textColor = Color.BetweenRgb(0, 0, 80).AndRgb(0, 0, 155).Frozen;
                    break;
            }

            fillColor = textColor;
            backColor = Color.Rgb(255, 255, 255); // white background
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
                switch (coin)
                {
                    case 0:
                        textColor = Color.RandomizedR(customDarkColor, 70).Frozen;
                        break;
                    case 1:
                        textColor = Color.RandomizedG(customDarkColor, 70).Frozen;
                        break;
                    case 2:
                        textColor = Color.RandomizedB(customDarkColor, 70).Frozen;
                        break;
                }

                fillColor = textColor;
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // custom outline
            textRenderer.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(graphics.Height / 15, backColor),
                LineLayer.Solid(graphics.Height / 15, textColor)
            );
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // a bit wider overlapping letters
            transform.Scaling.xScalingPercentageRange = new RandomRange(105, 110);
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // background "wall" surface
            RandomDots dots = new RandomDots();
            dots.Prototype.FillColor = fillColor;
            dots.SurfacePercentage = 2;
            dots.DrawFast(graphics);
        }
    }
}
