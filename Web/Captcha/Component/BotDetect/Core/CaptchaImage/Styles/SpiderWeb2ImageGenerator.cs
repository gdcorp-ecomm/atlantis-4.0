using System;
using System.Collections;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// SpiderWeb2 Captcha Image Generator
    /// </summary>
    internal class SpiderWeb2ImageGenerator : ImageGenerator, IImageGenerator
	{
        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            // custom outline
            textRenderer.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(2, backColor), 
                LineLayer.Solid(2, textColor)
            );
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            graphics.Fill(textColor);

            // slightly below the image
            Point center = new Point(graphics.Bounds).Frozen;
            center.Y += 2 * graphics.Height;

            // web = circles + spokes
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Triple(
                LineLayer.Solid(2, backColor), 
                LineLayer.Solid(2, textColor), 
                LineLayer.Solid(2, backColor)
            );
            circles.Spacing = 11;
            circles.Prototype.Center = center;
            circles.Draw(graphics);

            SpokeLines spokes = new SpokeLines();
            spokes.Prototype.Outline = LineStyle.Triple(
                LineLayer.Solid(2, textColor), 
                LineLayer.Solid(3, backColor), 
                LineLayer.Solid(2, textColor)
            );
            spokes.AngleDeltaRange = new RandomRange(8, 10);
            spokes.Center = center;
            spokes.Draw(graphics);
        }
    }		
}
