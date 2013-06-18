using System;
using System.Collections;
using System.Collections.Generic;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
	/// <summary>
	/// Summary description for IImageGenerator.
	/// </summary>
    internal abstract class ImageGenerator : IImageGenerator, IDisposable
    {
        // graphics wrapper infrastructure object
        protected IGraphics graphics;

        // text to be rendered
        protected string text;

        // captcha image locale
        protected Localization localization;

        // all colors used for captcha image generation
        protected ColorCollection colors = new ColorCollection();

        // color values overriden by the user
        protected Color customLightColor;
        protected Color customDarkColor;

        /// <summary>
        /// Color used to fill the text glyphs
        /// </summary>
        protected Color textColor
        {
            get
            {
                return colors["textColor"];
            }
            set
            {
                colors["textColor"] = value;
            }
        }

        /// <summary>
        /// Color used for text glyph outlines
        /// </summary>
        protected Color outlineColor
        {
            get
            {
                return colors["outlineColor"];
            }
            set
            {
                colors["outlineColor"] = value;
            }
        }

        /// <summary>
        /// Color used to fill the image before text drawing
        /// </summary>
        protected Color backColor
        {
            get
            {
                return colors["backColor"];
            }
            set
            {
                colors["backColor"] = value;
            }
        }

        protected FontCollection fonts;

        // glyph transformation rules
        protected Transform transform = new Transform();

        // text rendering is delegated to this object
        protected Text textRenderer;

        /// <summary>
        /// main captcha-drawing method
        /// </summary>
        /// <param name="text"></param>
        /// <param name="locale"></param>
        /// <param name="imageSize"></param>
        /// <param name="customColors"></param>
        /// <returns></returns>
        public IGraphics GenerateImage(string text, Localization localization, ImageSize imageSize, Color customLightColor, Color customDarkColor)
        {
            this.text = text;
            this.localization = localization;
            this.customLightColor = customLightColor;
            this.customDarkColor = customDarkColor;
            graphics = new GdiGraphics(imageSize);

            if (graphics.Height < CaptchaDefaults.MinImageSize.Height ||
                graphics.Width < CaptchaDefaults.MinImageSize.Width)
            {
                // very small images are not supported

                // The BotDetect.Drawing namespace is not a general drawing
                // library, but a Captcha domain-specific interface.
                // Instead of scaling all drawing techniques to unrealistic
                // sizes, we simply skip generating images outside a certain
                // size range (which makes sense for Captcha images).
                return graphics;
            }

            Init();

            Draw();

            return graphics;
        }

        /// <summary>
        /// Initialization phase
        /// </summary>
        protected virtual void Init()
        {
            InitColors();

            OverrideColors();

            InitText();

            CustomInit();
        }

        /// <summary>
        /// Define the color scheme used for captcha rendering
        /// </summary>
        protected virtual void InitColors()
        {
            // default color scheme
            backColor = Color.Rgb(255, 255, 255); // white background
            textColor = Color.Rgb(0, 0, 0); // black text
            outlineColor = null; // text without outline
        }

        /// <summary>
        /// override any component-code-defined values with
        /// customer-code-defined values
        /// </summary>
        protected virtual void OverrideColors()
        {
            if (null != customLightColor)
            {
                backColor = customLightColor;
            }

            if (null != customDarkColor)
            {
                if (null != textColor)
                {
                    textColor = customDarkColor;
                }
                else
                {
                    outlineColor = customDarkColor;
                }
            }
        }

        /// <summary>
        /// Text property initialization
        /// </summary>
        protected virtual void InitText()
        {
            InitFonts();
            InitGlyphTransform();
            InitTextRenderer();
        }

        protected virtual void InitFonts()
        {
            this.fonts = localization.Fonts.Clone();
        }

        /// <summary>
        /// Glyph tranformation rules
        /// </summary>
        protected virtual void InitGlyphTransform()
        {
            transform.Translation.xOffsetRange = new RandomRange(-7, 7);
            transform.Translation.yOffsetRange = new RandomRange(-7, 7);
            transform.Scaling.xScalingPercentageRange = new RandomRange(95, 105);
            transform.Scaling.yScalingPercentageRange = new RandomRange(95, 105);
            transform.Rotation.AngleRange = new RandomRange(-10, 10);
            transform.Warp.WarpPercentageRange = new RandomRange(5, 5);
        }

        /// <summary>
        /// Text renderer object properties
        /// </summary>
        protected virtual void InitTextRenderer()
        {
            if (localization.IsRtl)
            {
                textRenderer = new RtlText();
            }
            else
            {
                textRenderer = new LtrText();
            }

            textRenderer.Prototype.FillColor = textColor;
            textRenderer.Prototype.Outline = LineStyle.Single(LineLayer.Solid(1, outlineColor));
            textRenderer.TextToRender = this.text;
            textRenderer.Fonts = fonts;
            textRenderer.Transform = transform;

            // pre-calculate text placement
            textRenderer.Bounds = Text.DefaultTextBounds(graphics);
        }

        /// <summary>
        /// Placeholder for any additional initialization commands 
        /// </summary>
        protected virtual void CustomInit()
        {
        }

        /// <summary>
        /// Draw the captcha image
        /// </summary>
        protected virtual void Draw()
        {
            DrawBackground();

            AddTrademark();

            DrawText();

            DrawEffects();
        }


        protected void AddTrademark()
        {
            
        }


        /// <summary>
        /// Image background is rendered first, along with any background effects
        /// </summary>
        protected virtual void DrawBackground()
        {
            graphics.Fill(backColor);
        }

        /// <summary>
        /// Captcha code glyphs rendering
        /// </summary>
        protected virtual void DrawText()
        {
            textRenderer.Draw(graphics);
        }

        /// <summary>
        /// Various effects and noises applied after text rendering
        /// </summary>
        protected virtual void DrawEffects()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                    graphics = null;
                }

                if (textRenderer != null)
                {
                    textRenderer.Dispose();
                    textRenderer = null;
                }
            }
        }
    }
}
