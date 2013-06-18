using System;
using System.Collections;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// CaughtInTheNet Captcha Image Generator
    /// </summary>
    internal class CaughtInTheNetImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(0, 50, 150); // blue background
            textColor = Color.Rgb(220, 220, 220); // light gray text
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            // use CustomLightColor for text fill
            if (null != customLightColor)
            {
                textColor = customLightColor;
            }

            // use CustomDarkColor for image background
            if (null != customDarkColor)
            {
                backColor = customDarkColor;
            }
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // background lines in text color, mixing with glyph fragments
            RandomLines thickLines = new RandomLines();
            thickLines.Bounds = textRenderer.Bounds;
            thickLines.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(1, graphics.Height / 20), textColor)
             );
            thickLines.SurfacePercentage = 1;
            thickLines.Draw(graphics);

            RandomLines thinLines = new RandomLines();
            thinLines.Bounds = textRenderer.Bounds;
            thinLines.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, textColor)
            );
            thinLines.SurfacePercentage = 3;
            thinLines.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // foreground lines in background color, splitting text apart
            RandomLines lines = new RandomLines();
            lines.Bounds = textRenderer.Bounds;
            lines.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, backColor)
            );
            lines.SurfacePercentage = 4;
            lines.Draw(graphics);

            // foreground concentric circles, splitting text apart
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, backColor)
            );
            circles.SpacingRange = new RandomRange(5, 8);
            circles.DrawFast(graphics);
        }
	}		
}