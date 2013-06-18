using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Wave Captcha Image Generator
    /// </summary>
    internal class WaveImageGenerator : ImageGenerator, IImageGenerator
	{
        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // individual glyphs are expanded vertically so they overlap with wave borders
            transform.Scaling.yScalingPercentageRange = new RandomRange(100, 110);
            transform.Translation.yOffset = this.graphics.Height / -20;
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // sand frame
            int thickness = RandomGenerator.Next(1, graphics.Height / 5);
            Rectangle frame = graphics.Bounds;
            frame.Outline = LineStyle.Single(LineLayer.Solid(thickness, textColor));
            frame.Draw(graphics);

            // wave "crests"
            HorizontalLines crests = new HorizontalLines();
            crests.SpacingRange = new RandomRange(graphics.Height / 5, graphics.Height / 4);
            crests.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, textColor));
            crests.Draw(graphics);

            // "sand"
            RandomDots sand = new RandomDots();
            sand.Prototype.FillColor = textColor;
            sand.SurfacePercentage = 3;
            sand.DrawFast(graphics);

            // "stones"
            RandomCircles stones = new RandomCircles();
            stones.Prototype.FillColor = textColor;
            stones.Prototype.RadiusRange = new RandomRange(1, graphics.Height / 12);
            stones.SurfacePercentage = 5;
            stones.DrawFast(graphics);

            // "water"
            Wave wave = new Wave();
            wave.Level = RandomGenerator.Next(2, 4);
            wave.OverflowColor = textColor;
            wave.Apply(graphics);
        }
    }
}
