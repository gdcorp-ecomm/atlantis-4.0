using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// WantedCircular Captcha Image Generator
    /// </summary>
    internal class WantedCircularImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(220, 220, 220);
            textColor = Color.Rgb(0, 0, 0);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // dollar signs as segmentation challenge
            RandomGlyphs prize = new RandomGlyphs();
            prize.Text = "$";
            prize.Fonts = fonts;
            prize.Bounds = textRenderer.Bounds;
            prize.Prototype.Transform = new Transform();
            prize.Prototype.Transform.Rotation.AngleRange = new RandomRange(-5, 5);
            prize.Prototype.FillColor = textColor;
            prize.SurfacePercentage = 10;
            prize.Draw(graphics);

            // scratches
            RandomBeziers darkScratches = new RandomBeziers();
            darkScratches.Bounds = textRenderer.Bounds;
            darkScratches.ScalingPercentage = 5;
            darkScratches.SurfacePercentage = 5;
            darkScratches.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, textColor));
            darkScratches.DrawFast(graphics);

            RandomBeziers lightScratches = new RandomBeziers();
            lightScratches.Bounds = textRenderer.Bounds;
            lightScratches.ScalingPercentage = 5;
            lightScratches.SurfacePercentage = 5;
            lightScratches.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, backColor));
            lightScratches.DrawFast(graphics);    

            // noise
            RandomDots dots = new RandomDots();
            dots.Prototype.FillColor = textColor;
            dots.SurfacePercentage = 3;
            dots.DrawFast(graphics);
        }
    }
}
