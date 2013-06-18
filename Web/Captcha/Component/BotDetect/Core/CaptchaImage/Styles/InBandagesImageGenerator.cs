using System;
using System.Collections;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// InBandages Captcha Image Generator
    /// </summary>
    internal class InBandagesImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            textColor = Color.BetweenRgb(0, 0, 0).AndRgb(100, 100, 100); // random dark color
            backColor = textColor.Complement.Frozen;
        }


        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            textRenderer.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(Math.Max(2, graphics.Height / 20), backColor),
                LineLayer.Solid(graphics.Height / 15, textColor)   
            );
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // horizontal bandages
            HorizontalLines bandages = new HorizontalLines();
            bandages.SpacingRange = new RandomRange(graphics.Height / 3, graphics.Height / 2);
            bandages.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(graphics.Height / 20, backColor),
                LineLayer.Solid(graphics.Height / 20, textColor)
            );
            bandages.Draw(graphics);

            // foreground shapes
            RandomRectangles bricks = new RandomRectangles();
            bricks.Prototype.FillColor = backColor;
            bricks.ScalingPercentage = 5;
            bricks.SurfacePercentage = 10;
            bricks.DrawFast(graphics);
        }
    }		
}
