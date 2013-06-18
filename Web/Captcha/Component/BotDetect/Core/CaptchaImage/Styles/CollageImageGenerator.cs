using System;
using System.Collections;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Collage Captcha Image Generator
    /// </summary>
    internal class CollageImageGenerator : ImageGenerator, IImageGenerator
	{
        // collage lines color
        protected Color lineColor
        {
            get
            {
                return colors["lineColor"];
            }
            set
            {
                colors["lineColor"] = value;
            }
        }

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            // randomized light background
            backColor = Color.BetweenRgb(225, 225, 225).AndRgb(255, 255, 255);

            // randomized colorful lines
            lineColor = Color.BetweenRgb(75, 75, 75).AndRgb(245, 245, 245);

            textColor = lineColor; 
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            // background color is randomized around the CustomLightColor
            if (null != customLightColor)
            {
                backColor = Color.Randomized(customLightColor, 50);
            }

            // line color is randomized around the CustomDarkColor
            if (null != customDarkColor)
            {
                lineColor = Color.Randomized(customDarkColor, 120);
                textColor = lineColor; 
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

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // adjusted glyph size for better readability
            transform.Scaling.yScalingPercentageRange = new RandomRange(100, 110);
            transform.Scaling.xScalingPercentage = 100;
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            // random color background segments
            int totalWidth = 0;
            while (totalWidth < graphics.Width)
            {
                int width = RandomGenerator.Next(graphics.Width / 20, graphics.Width / 5);
                Rectangle back = new Rectangle(new Point(totalWidth, 0), width, graphics.Height);
                back.FillColor = backColor;
                back.Draw(graphics);
                totalWidth += width;
            }
            
        }

        protected override void DrawEffects()
        {
            base.DrawEffects();

            // transparent "viewport" made out of random rectangles and text
            RandomRectangles connectors = new RandomRectangles();
            connectors.Bounds = textRenderer.Bounds;
            connectors.ScalingPercentage = 15;
            connectors.SurfacePercentage = 5;
            connectors.Draw(graphics);

            ShapeCollection clip = new ShapeCollection();
            foreach (AtomicShape glyph in textRenderer)
            {
                clip.Add(glyph);
            }
            foreach (AtomicShape line in connectors)
            {
                clip.Add(line);
            }

            // random collage lines seen through the "viewport"
            RandomLines lines = new RandomLines();
            lines.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(Math.Max(5, graphics.Height / 10), lineColor)
            );
            lines.SurfacePercentage = 200;
            lines.Draw(graphics, clip);
        }
	}		
}
