using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// PaintMess Captcha Image Generator
    /// </summary>
    internal class PaintMessImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            textColor = Color.Rgb(200, 200, 50); // yellow
            backColor = Color.Rgb(120, 50, 30); // dark reddish
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

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // "spinal" brush stroke
            Rectangle bounds = textRenderer.Bounds;
            int thickness = RandomGenerator.Next(2, graphics.Height / 15 + 2);
            Line wire = new Line(
                Point.Between(bounds.TopLeft.X, bounds.TopLeft.Y).And(bounds.TopLeft.X + bounds.Height, bounds.TopLeft.Y + bounds.Height),
                Point.Between(bounds.BottomRight.X - bounds.Height, bounds.BottomRight.Y - bounds.Height).And(bounds.BottomRight.X, bounds.BottomRight.Y),
                LineStyle.Single(LineLayer.Solid(thickness, textColor))
            );
            wire.Draw(graphics);

            // random paint drops
            RandomCircles paintDrops = new RandomCircles();
            paintDrops.Prototype.FillColor = textColor;
            paintDrops.Prototype.RadiusRange = new RandomRange(2, graphics.Height / 12 + 1);
            paintDrops.SurfacePercentage = 5;
            paintDrops.DrawFast(graphics);

            // distorted so they are not perfectly circular and the "spinal" brush stroke meanders 
            Wave wave = new Wave();
            wave.Level = 4;
            wave.Apply(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // random foreground paint spots
            RandomDots spots2 = new RandomDots();
            spots2.Prototype.FillColor = backColor;
            spots2.SurfacePercentage = 2;
            spots2.DrawFast(graphics);  
        }
    }
}
