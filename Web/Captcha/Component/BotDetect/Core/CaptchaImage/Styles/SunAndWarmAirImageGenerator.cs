using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// SunAndWarmAir Captcha Image Generator
    /// </summary>
    internal class SunAndWarmAirImageGenerator : ImageGenerator, IImageGenerator
	{

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

            foreColor = Color.Rgb(210, 213, 220);
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
            }
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // cloud outlines
            LineStyle cloudOutline = LineStyle.Single(LineLayer.Solid(graphics.Height / 15, textColor));

            int dimension = Math.Min(graphics.Height, graphics.Width / 5);

            // center noise
            Circle centerCloud = new Circle(graphics.Bounds.Center, dimension * RandomGenerator.Next(6, 12) / 10);
            centerCloud.FillColor = foreColor;
            centerCloud.Outline = cloudOutline;
            centerCloud.Draw(graphics);

            // upper right noise
            Circle topRightCloud = new Circle(graphics.Bounds.TopRight, dimension * RandomGenerator.Next(6, 15) / 10);
            topRightCloud.FillColor = foreColor;
            topRightCloud.Outline = cloudOutline;
            topRightCloud.Draw(graphics);

            // lower left noise
            Circle bottomLeftCloud = new Circle(graphics.Bounds.BottomLeft, dimension * RandomGenerator.Next(6, 15) / 10);
            bottomLeftCloud.FillColor = foreColor;
            bottomLeftCloud.Outline = cloudOutline;
            bottomLeftCloud.Draw(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // heat "waves"
            ConcentricCircles circles = new ConcentricCircles();
            circles.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(1, backColor)
            );
            circles.Spacing = 2;
            circles.DrawFast(graphics);

            // noise
            RandomDots dots = new RandomDots();
            dots.Prototype.FillColor = textColor;
            dots.SurfacePercentage = 2;
            dots.DrawFast(graphics);
        }
    }
}
