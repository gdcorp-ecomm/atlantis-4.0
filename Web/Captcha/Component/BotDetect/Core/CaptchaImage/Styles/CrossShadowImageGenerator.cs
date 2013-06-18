using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// CrossShadow Captcha Image Generator
    /// </summary>
    internal class CrossShadowImageGenerator : ImageGenerator, IImageGenerator
	{
        // shadow color - between text and background
        protected Color shadowColor
        {
            get
            {
                return colors["shadowColor"];
            }
            set
            {
                colors["shadowColor"] = value;
            }
        }

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            base.InitColors();

            backColor = Color.Rgb(255, 255, 255); // white background
            textColor = Color.Rgb(45, 45, 45); // black text
            shadowColor = Color.Rgb(150, 150, 150); // gray shadows
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = customLightColor;
            }

            if (null != customDarkColor)
            {
                textColor = customDarkColor;
            }

            if (null != customLightColor || null != customDarkColor)
            {
                shadowColor = Color.Median(backColor, textColor);
            }
        }

        protected override void InitGlyphTransform()
        {
            base.InitGlyphTransform();

            // no glyph transform by default
            transform = Transform.None;
        }

        protected override void CustomInit()
        {
            // create a separate text renderer for glyph shadows
            if (localization.IsRtl)
            {
                shadowRenderer = new RtlText();
            }
            else
            {
                shadowRenderer = new LtrText();
            }

            shadowRenderer.Prototype.FillColor = textColor;
            shadowRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(2, outlineColor)
            );
            shadowRenderer.TextToRender = this.text;
            shadowRenderer.Fonts = fonts;
            shadowRenderer.Transform = transform;
        }

        // text rendering is delegated to this object
        protected Text shadowRenderer;

        protected override void DrawText()
        {
            DrawTextShadow();
            ShapeCollection shadowBounds = shadowRenderer;

            DrawOriginalText(shadowBounds.Bounds);
            ShapeCollection textBounds = textRenderer;

            DrawCrossFade(shadowBounds, textBounds);
        }

        protected void DrawTextShadow()
        {
            // using full size letters...
            shadowRenderer.Transform.Scaling.xScalingPercentage = 95;
            shadowRenderer.Transform.Scaling.yScalingPercentage = 100;

            // shadows are rotated to the right a bit
            shadowRenderer.Transform.Rotation.AngleRange = new RandomRange(6, 8);

            shadowRenderer.Prototype.FillColor = shadowColor;
            shadowRenderer.Draw(graphics);
        }

        protected void DrawOriginalText(Rectangle bounds)
        {
            // non-shadow text is drawn in a slightly smaller rectangle
            textRenderer.Bounds = new Rectangle(bounds, 0.95);

            textRenderer.Transform.Scaling.xScalingPercentage = 100;
            textRenderer.Transform.Scaling.yScalingPercentage = 110;

            // the original text is rotated very slightly to the left
            textRenderer.Transform.Rotation.Angle = -2;

            textRenderer.Prototype.FillColor = textColor;
            textRenderer.Draw(graphics);
        }

        protected void DrawCrossFade(ShapeCollection bounds1, ShapeCollection bounds2)
        {
            // make the shadows and text intersection transparent
            graphics.Clip = bounds1;
            foreach (AtomicShape s in bounds2)
            {
                Glyph g = s as Glyph;
                if (null != g)
                {
                    g.FillColor = backColor;
                    g.Draw(graphics);
                }
            }
            graphics.Clip = null;
        }
    }
}