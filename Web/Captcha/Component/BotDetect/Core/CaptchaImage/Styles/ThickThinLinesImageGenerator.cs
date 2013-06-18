using System;
using System.Collections;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// ThickThinLines Captcha Image Generator
    /// </summary>
    internal class ThickThinLinesImageGenerator : ImageGenerator, IImageGenerator
	{
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

        protected int coin = RandomGenerator.Next(0, 3);

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            switch (coin) // reddish, greenish or bluish color scheme
            {
                case 0:
                    textColor = Color.BetweenRgb(50, 0, 0).AndRgb(130, 0, 0);
                    lineColor = Color.BetweenRgb(80, 0, 0).AndRgb(255, 0, 0);
                    break;
                case 1:
                    textColor = Color.BetweenRgb(0, 50, 0).AndRgb(0, 130, 0);
                    lineColor = Color.BetweenRgb(0, 80, 0).AndRgb(0, 255, 0);
                    break;
                case 2:
                    textColor = Color.BetweenRgb(0, 0, 50).AndRgb(0, 0, 130);
                    lineColor = Color.BetweenRgb(0, 0, 80).AndRgb(0, 0, 255);
                    break;
            }

            backColor = Color.Rgb(255, 255, 255);
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
                switch (coin) // reddish, greenish or bluish color scheme
                {
                    case 0:
                        textColor = Color.RandomizedR(customDarkColor, 70);
                        lineColor = Color.RandomizedR(customDarkColor, 150);
                        break;
                    case 1:
                        textColor = Color.RandomizedG(customDarkColor, 70);
                        lineColor = Color.RandomizedG(customDarkColor, 150);
                        break;
                    case 2:
                        textColor = Color.RandomizedB(customDarkColor, 70);
                        lineColor = Color.RandomizedB(customDarkColor, 150);
                        break;
                }

                outlineColor = textColor;
            }
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // random thick background lines
            RandomLines lines = new RandomLines();
            lines.Bounds = textRenderer.Bounds;
            lines.Prototype.Outline = LineStyle.Triple(
                LineLayer.Solid(graphics.Height / 20, lineColor),
                LineLayer.Solid(1, backColor),
                LineLayer.Solid(graphics.Height / 20, lineColor)
                );
            lines.SurfacePercentage = 15;
            lines.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // horizontal foreground lines
            HorizontalLines hLines = new HorizontalLines();
            hLines.SpacingRange = new RandomRange(graphics.Height / 4 + 1, graphics.Height / 3 + 1);
            hLines.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(1, lineColor),
                LineLayer.Solid(1, lineColor)
            );
            hLines.Prototype.Transform = Transform.None;
            hLines.Prototype.Transform.Rotation.AngleRange = new RandomRange(-15, 15);
            hLines.Draw(graphics);

            // vertical foreground lines
            VerticalLines vLines = new VerticalLines();
            vLines.SpacingRange = new RandomRange(graphics.Height / 3 + 1, graphics.Height / 2 + 1);
            vLines.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(1, lineColor),
                LineLayer.Solid(1, lineColor)
            );
            vLines.Prototype.Transform = Transform.None;
            vLines.Prototype.Transform.Rotation.AngleRange = new RandomRange(-15, 15);
            vLines.Draw(graphics);
            
        }
    }
}
