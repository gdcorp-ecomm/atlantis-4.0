using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Chipped Captcha Image Generator
    /// </summary>
    internal class ChippedImageGenerator : ImageGenerator, IImageGenerator
	{
        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            textColor = Color.Rgb(50, 50, 60); // dark bluish gray text
            backColor = Color.Rgb(200, 205, 210); // lighter blusih gray background
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, textColor)
            );

            textRenderer.Prototype.Transform.Scaling.yScalingPercentageRange = new RandomRange(100, 110);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();
          
            // single larger curve
            Bezier curve = new Bezier(textRenderer.Bounds, 
                LineStyle.Single(
                    LineLayer.Solid(graphics.Height / 15, textColor)
                )
            );
            curve.Draw(graphics);

            // dark curves connecting glyphs
            RandomBeziers darkCurves = new RandomBeziers();
            darkCurves.Bounds = textRenderer.Bounds;
            darkCurves.ScalingPercentage = 16;
            darkCurves.SurfacePercentage = 12;
            darkCurves.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 15, textColor)
            );
            darkCurves.DrawFast(graphics);

            // light curves fragmenting glyphs
            RandomBeziers lightCurves = new RandomBeziers();
            lightCurves.Bounds = textRenderer.Bounds;
            lightCurves.ScalingPercentage = 3;
            lightCurves.SurfacePercentage = 20;
            lightCurves.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(graphics.Height / 25, 1), backColor)
            );
            lightCurves.DrawFast(graphics);
        }
    }
}
