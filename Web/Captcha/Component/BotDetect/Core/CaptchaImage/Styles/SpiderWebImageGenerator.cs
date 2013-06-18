using System;
using System.Collections;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// SpiderWeb Captcha Image Generator
    /// </summary>
    internal class SpiderWebImageGenerator : ImageGenerator, IImageGenerator
	{
        protected override void DrawEffects()
        {
            base.DrawEffects();

            // slightly below the image
            Point center = new Point(graphics.Bounds).Frozen;
            center.Y += 2 * graphics.Height;

            // web = circles + spokes
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, textColor));
            circles.Spacing = 4;
            circles.Prototype.Center = center;
            circles.DrawFast(graphics);

            ConcentricCircles circles2 = new ConcentricCircles();
            circles2.Prototype.Outline = LineStyle.Single(LineLayer.Solid(2, textColor));
            circles2.Spacing = 16;
            circles2.Prototype.Center = center;
            circles2.DrawFast(graphics);

            ConcentricCircles circles3 = new ConcentricCircles();
            circles3.Prototype.Outline = LineStyle.Single(LineLayer.Solid(3, textColor));
            circles3.Spacing = 48;
            circles3.Prototype.Center = center;
            circles3.DrawFast(graphics);

            SpokeLines spokes = new SpokeLines();
            spokes.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, textColor));
            spokes.AngleDeltaRange = new RandomRange(3, 6);
            spokes.Center = center;
            spokes.Draw(graphics);
        }
    }		
}
