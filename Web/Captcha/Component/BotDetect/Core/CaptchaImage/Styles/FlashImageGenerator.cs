using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Flash Captcha Image Generator
    /// </summary>
    internal class FlashImageGenerator : ImageGenerator, IImageGenerator
	{
        // grid color
        protected Color lineColor
        {
            get
            {
                return colors["lineColor"];
            }
            set
            {
                colors["lineColor"] = value;
            }
        }

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(255, 255, 220); // yellowish background
            textColor = Color.BetweenRgb(0, 0, 0).AndRgb(150, 150, 150); // random letter color
            lineColor = Color.BetweenRgb(50, 50, 50).AndRgb(200, 200, 200); // random line color
            outlineColor = textColor;
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
                textColor = Color.Randomized(customDarkColor, 100);
                lineColor = Color.Randomized(Color.SaturationAdjusted(Color.Lightened(customDarkColor, 1.5f), 0.5f), 100);
                outlineColor = textColor;
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // thicker outline
            textRenderer.Prototype.Outline = LineStyle.Single(LineLayer.Solid(graphics.Height / 15, textColor));
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // foreground grid
            HorizontalLines hLines = new HorizontalLines();
            hLines.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(graphics.Height / 20, lineColor),
                LineLayer.Solid(graphics.Height / 20, lineColor)
            );
            hLines.SpacingRange = new RandomRange(graphics.Height / 4, graphics.Height / 2);
            hLines.Prototype.Transform = Transform.None;
            hLines.Prototype.Transform.Rotation.AngleRange = new RandomRange(-5, 5);
            hLines.Draw(graphics);

            VerticalLines vLines = new VerticalLines();
            vLines.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(graphics.Height / 20, lineColor),
                LineLayer.Solid(graphics.Height / 20, lineColor)
            );
            vLines.SpacingRange = new RandomRange(graphics.Height / 3, graphics.Height * 2 / 3);
            vLines.Prototype.Transform = Transform.None;
            vLines.Prototype.Transform.Rotation.AngleRange = new RandomRange(-5, 5);
            vLines.Draw(graphics);
        }
    }
}
