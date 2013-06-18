using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Lego Captcha Image Generator
    /// </summary>
    internal class LegoImageGenerator : ImageGenerator, IImageGenerator
	{
        // text brick color
        protected Color brickColor
        {
            get
            {
                return colors["brickColor"];
            }
            set
            {
                colors["brickColor"] = value;
            }
        }

        // background brick color
        protected Color lightBrickColor
        {
            get
            {
                return colors["lightBrickColor"];
            }
            set
            {
                colors["lightBrickColor"] = value;
            }
        }

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // white background
            backColor = Color.Rgb(255, 255, 255);

            // text lego bricks
            brickColor = Color.BetweenRgb(0, 0, 0).AndRgb(230, 230, 230);

            // background lego bricks
            lightBrickColor = Color.BetweenRgb(200, 200, 200).AndRgb(255, 255, 255);

            // text is not actually shown in the image
            textColor = Color.BetweenRgb(100, 100, 100).AndRgb(200, 200, 200);
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = customLightColor;
                textColor = backColor;
            }

            if (null != customDarkColor)
            {
                brickColor = Color.Randomized(customDarkColor, 100);
            }

            if (null != customDarkColor || null != customDarkColor)
            {
                lightBrickColor = Color.Randomized(Color.Lightened(Color.SaturationAdjusted(Color.Median(backColor, customDarkColor), 0.8f), 1.3f), 50);
            }
        }

        protected override void InitFonts()
        {
            base.InitFonts();

            // bold fonts for easier reading
            FontCollection fontSelection = new FontCollection();
            fontSelection[0] = Font.From("Arial", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[2] = Font.From("Microsoft Sans Serif", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[3] = Font.From("Times New Roman", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[4] = Font.From("Tahoma", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[5] = Font.From("Verdana", FontCase.Uppercase, FontWeight.Bold);

            fonts = fontSelection;
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // lighter background bricks
            RandomRectangles backgroundBricks = new RandomRectangles();
            backgroundBricks.Prototype.FillColor = lightBrickColor;
            backgroundBricks.Prototype.Transform.Rotation.AngleRange = new RandomRange(-20, 20);
            backgroundBricks.ScalingPercentage = 15;
            backgroundBricks.SurfacePercentage = 100;
            backgroundBricks.DrawFast(graphics);
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // darker text bricks within glyph bounds
            RandomRectangles foregroundBricks = new RandomRectangles();
            foregroundBricks.Bounds = textRenderer.Bounds;
            foregroundBricks.Prototype.FillColor = brickColor;
            foregroundBricks.Prototype.Transform.Rotation.AngleRange = new RandomRange(-20, 20);
            foregroundBricks.ScalingPercentage = 13;
            foregroundBricks.SurfacePercentage = 280;
            foregroundBricks.DrawFast(graphics, textRenderer);
        }
    }
}
