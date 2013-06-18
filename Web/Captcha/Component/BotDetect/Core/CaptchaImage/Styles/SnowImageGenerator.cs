using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Snow Captcha Image Generator
    /// </summary>
    internal class SnowImageGenerator : ImageGenerator, IImageGenerator
	{

        protected Color noiseColor
        {
            get
            {
                return colors["noiseColor"];
            }
            set
            {
                colors["noiseColor"] = value;
            }
        }

        protected Color foreColor
        {
            get
            {
                return colors["foreColor"];
            }
            set
            {
                colors["foreColor"] = value;
            }
        }

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            textColor = Color.Rgb(35, 38, 45);
            backColor = Color.Rgb(245, 248, 255);
            foreColor = Color.Rgb(200, 203, 210);
            noiseColor = Color.Rgb(100, 103, 110);
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = customLightColor;
                foreColor = Color.Darkened(customLightColor, 85);
            }

            if (null != customDarkColor)
            {
                textColor = customDarkColor;
                noiseColor = Color.Lightened(customDarkColor, 2.0f);
            }
        }

        protected override void InitTextRenderer()
        {
            base.InitTextRenderer();

            textRenderer.Prototype.FillColor = backColor;

            // thick text outline
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(1, graphics.Height / 30), textColor)
            );
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // individual glyphs are expanded horizontally and vertically
            transform.Scaling.xScalingPercentageRange = new RandomRange(100, 110);
            transform.Scaling.yScalingPercentageRange = new RandomRange(100, 110);
        }


        protected override void DrawBackground()
        {
            base.DrawBackground();

            // rain drops
            HorizontalLines hLines = new HorizontalLines();
            hLines.SpacingRange = new RandomRange(graphics.Height / 4, graphics.Height / 2 + 1);
            hLines.Prototype.Outline = LineStyle.Double(
                LineLayer.Solid(Math.Max(1, graphics.Height / 30), textColor),
                LineLayer.Solid(Math.Max(1, graphics.Height / 30), noiseColor)
            );
            hLines.Prototype.Transform = Transform.None;
            hLines.Prototype.Transform.Rotation.AngleRange = new RandomRange(15, 35);
            hLines.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // dark and light blur
            RandomDots dots = new RandomDots();
            dots.Prototype.FillColor = noiseColor;
            dots.SurfacePercentage = 2;
            dots.DrawFast(graphics);

            // snowflakes and noise
            RandomCircles specks = new RandomCircles();
            specks.Bounds = textRenderer.Bounds;
            specks.Prototype.FillColor = noiseColor;
            specks.Prototype.RadiusRange = new RandomRange(1, graphics.Height / 30 + 2);
            specks.SurfacePercentage = 2;
            specks.DrawFast(graphics);
            
            RandomCircles snowflakes = new RandomCircles();
            snowflakes.Bounds = textRenderer.Bounds;
            snowflakes.Prototype.FillColor = backColor;
            snowflakes.Prototype.RadiusRange = new RandomRange(1, graphics.Height / 30 + 2);
            snowflakes.SurfacePercentage = 10;
            snowflakes.DrawFast(graphics);
        }
    }
}
