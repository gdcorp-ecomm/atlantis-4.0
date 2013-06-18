using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    class Glyph : AtomicShape
    {
        protected Rectangle glyphBounds;

        public Rectangle Bounds
        {
            get
            {
                return glyphBounds;
            }

            set
            {
                glyphBounds = value;
            }
        }

        protected Font font;

        public Font Font
        {
            get
            {
                return font;
            }

            set
            {
                font = value;
            }
        }

        protected string character;

        public string Character
        {
            get
            {
                return character;
            }

            set
            {
                character = value;
            }
        }

        public Glyph()
        {
        }

        public override int Surface
        {
            get 
            {
                // aproximation of glyph surface with the bounding rectangle
                return new Rectangle(this.GdiPath.GetBounds()).Surface / 2;
            }
        }

        public override System.Drawing.Drawing2D.GraphicsPath GdiPath
        {
            get
            {
                if (null == gdiPath)
                {
                    gdiPath = new System.Drawing.Drawing2D.GraphicsPath();

                    System.Drawing.Font gdiFont = GetGdiFont(this.Font.FontFamily, this.Font.Weight);
                    System.Drawing.Rectangle gdiRectangle = this.Bounds.GdiRectangle;
                    int fontSize = CalculateFontSize(this.Character, gdiFont, gdiRectangle);

                    gdiPath.AddString(this.Character, gdiFont.FontFamily, (int)gdiFont.Style, fontSize, gdiRectangle, new System.Drawing.StringFormat());
                    
                    ApplyTransform(gdiPath, base.Transform, this.Bounds);
                 }

                return gdiPath;
            }
        }


        protected static string GdiFontKey(string fontFamily, FontWeight fontWeight)
        {
            return fontFamily.ToLowerInvariant() + fontWeight.ToString();
        }

        private static readonly Dictionary<string, System.Drawing.Font> _gdiFonts = new Dictionary<string, System.Drawing.Font>();
        private static readonly object _gdiFontLock = new object();

        public static System.Drawing.Font GetGdiFont(string fontFamily, FontWeight fontWeight)
        {
            string fontKey = GdiFontKey(fontFamily, fontWeight);

            // create each font object only once
            if (!_gdiFonts.ContainsKey(fontKey))
            {
                lock (_gdiFontLock)
                {
                    // re-check, in case the lock could not be acquired immediately
                    if (!_gdiFonts.ContainsKey(fontKey))
                    {
                        System.Drawing.Font gdiFont = new System.Drawing.Font(fontFamily, 10, FontWeightHelper.GdiStyle(fontWeight), System.Drawing.GraphicsUnit.Pixel);
                        _gdiFonts[fontKey] = gdiFont;
                    }
                }
            }

            return _gdiFonts[fontKey] as System.Drawing.Font;
        }

        protected int CalculateFontSize(string character, System.Drawing.Font fontPrototype, System.Drawing.Rectangle glyphBounds)
        {
            // reduce font size until the text fits inside the container
            int fontSize = glyphBounds.Height;

            /*System.Drawing.SizeF size;
            // not used, because of transform scaling that does the same thing
            do
            {
                fontSize--;
                System.Drawing.Font font = new System.Drawing.Font(fontPrototype.FontFamily, fontSize, fontPrototype.Style, System.Drawing.GraphicsUnit.Pixel);
                size = base.Graphics.MeasureString(character, font);
            } while (size.Width > glyphBounds.Width || size.Height > glyphBounds.Height);*/

            return fontSize;
        }

        public override void DrawOutline()
        {
            // text outline is drawn in reverse - first the outer layers and then the inner,
            // using cumulative layer thickness to get the right result (larger background outlines
            // and smaller foreground outlines)
            int totalThickness = this.Outline.Thickness;

            for (int i = this.Outline.LayerCount; i > 0; i--)
            {
                LineLayer layer = this.Outline[i - 1];
                if (null == layer)
                {
                    break;
                }

                using (System.Drawing.Pen pen = new System.Drawing.Pen(layer[0].Color.GdiColor, totalThickness))
                {
                    base.Graphics.DrawPath(pen, this.GdiPath);
                }

                totalThickness -= layer.Thickness;
            }
        }
    }
}
