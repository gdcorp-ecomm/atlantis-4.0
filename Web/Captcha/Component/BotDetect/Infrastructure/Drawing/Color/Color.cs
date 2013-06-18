using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

using BotDetect.Serialization;

namespace BotDetect.Drawing
{
    [Serializable]
    public class Color : ISerializable
    {
        // red
        private RandomRange _redRange;

        public RandomRange RedRange
        {
            get
            {
                return _redRange;
            }

            set
            {
                _redRange = value;
            }
        }

        public byte R
        {
            get
            {
                if (null == _redRange)
                {
                    return 0;
                }

                return (byte)_redRange.Next;
            }
            set
            {
                _redRange = new RandomRange(value);
            }
        }

        // green
        private RandomRange _greenRange;

        public RandomRange GreenRange
        {
            get
            {
                return _greenRange;
            }

            set
            {
                _greenRange = value;
            }
        }

        public byte G
        {
            get
            {
                if (null == _greenRange)
                {
                    return 0;
                }

                return (byte)_greenRange.Next;
            }
            set
            {
                _greenRange = new RandomRange(value);
            }
        }

        // blue
        private RandomRange _blueRange;

        public RandomRange BlueRange
        {
            get
            {
                return _blueRange;
            }

            set
            {
                _blueRange = value;
            }
        }

        public byte B
        {
            get
            {
                if (null == _blueRange)
                {
                    return 0;
                }

                return (byte)_blueRange.Next;
            }
            set
            {
                _blueRange = new RandomRange(value);
            }
        }

        private Color()
        {
        }

        protected Color(int red, int green, int blue)
        {
            if ( !IsValidColorValue(red) ||
                 !IsValidColorValue(green) ||
                 !IsValidColorValue(blue))
            {
                throw new DrawingException("Color component must be between 0 and 255", red, green, blue);
            }

            this.R = (byte) red;
            this.G = (byte) green;
            this.B = (byte) blue;
        }

        private static bool IsValidColorValue(int value)
        {
            return (0 <= value || 255 >= value);
        }

        protected Color(Color value)
        {
            _redRange = new RandomRange(value.R);
            _greenRange = new RandomRange(value.G);
            _blueRange = new RandomRange(value.B);
        }

        public static Color Copy(Color value)
        {
            return new Color(value);
        }

        protected Color(Color min, Color max)
        {
            _redRange = new RandomRange(min.R, max.R);
            _greenRange = new RandomRange(min.G, max.G);
            _blueRange = new RandomRange(min.B, max.B);
        }

        public static Color Rgb(int red, int green, int blue)
        {
            return new Color(red, green, blue);
        }

        public static Color BetweenRgb(int red, int green, int blue)
        {
            return new Color(red, green, blue);
        }

        public Color AndRgb(int red, int green, int blue)
        {
            return new Color(this, new Color(red, green, blue));
        }

        public static Color Randomized(Color value, int rangeSize)
        {
            Color expanded = new Color();
            int halfRange = rangeSize / 2;

            int minR = value.R - halfRange; if (minR < 0) { minR = 0; }
            int maxR = value.R + halfRange; if (maxR > 255) { maxR = 255; }
            expanded.RedRange = new RandomRange(minR, maxR);

            int minG = value.G - halfRange; if (minG < 0) { minG = 0; }
            int maxG = value.G + halfRange; if (maxG > 255) { maxG = 255; }
            expanded.GreenRange = new RandomRange(minG, maxG);

            int minB = value.B - halfRange; if (minB < 0) { minB = 0; }
            int maxB = value.B + halfRange; if (maxB > 255) { maxB = 255; }
            expanded.BlueRange = new RandomRange(minB, maxB);

            return expanded;
        }

        public static Color RandomizedR(Color value, int rangeSize)
        {
            Color expanded = new Color();
            int halfRange = rangeSize / 2;

            int minR = value.R - halfRange; if (minR < 0) { minR = 0; }
            int maxR = value.R + halfRange; if (maxR > 255) { maxR = 255; }
            expanded.RedRange = new RandomRange(minR, maxR);

            expanded.G = value.G;

            expanded.B = value.B;

            return expanded;
        }

        public static Color RandomizedG(Color value, int rangeSize)
        {
            Color expanded = new Color();
            int halfRange = rangeSize / 2;

            int minG = value.G - halfRange; if (minG < 0) { minG = 0; }
            int maxG = value.G + halfRange; if (maxG > 255) { maxG = 255; }
            expanded.GreenRange = new RandomRange(minG, maxG);

            expanded.R = value.R;

            expanded.B = value.B;

            return expanded;
        }

        public static Color RandomizedB(Color value, int rangeSize)
        {
            Color expanded = new Color();
            int halfRange = rangeSize / 2;

            int minB = value.B - halfRange; if (minB < 0) { minB = 0; }
            int maxB = value.B + halfRange; if (maxB > 255) { maxB = 255; }
            expanded.BlueRange = new RandomRange(minB, maxB);

            expanded.G = value.G;

            expanded.R = value.R;

            return expanded;
        }

        public static Color Darkened(Color value, int reductionPercentage)
        {
            System.Drawing.Color gc = value.GdiColor;

            float h = gc.GetHue() / 360.0f;
            float s = gc.GetSaturation();
            float l = gc.GetBrightness() * reductionPercentage / 100;
            if (l > 1.0f) { l = 1.0f; }

            return Color.Hsl(h, s, l);
        }

        public static Color Lightened(Color value, float expansionFactor)
        {
            System.Drawing.Color gc = value.GdiColor;

            float h = gc.GetHue() / 360.0f;
            float s = gc.GetSaturation();
            float l = gc.GetBrightness() * expansionFactor;
            if (0.1f > l) { l = 0.25f; }
            if (l > 1.0f) { l = 1.0f; }

            return Color.Hsl(h, s, l);
        }

        public static Color SaturationAdjusted(Color value, float expansionFactor)
        {
            System.Drawing.Color gc = value.GdiColor;

            float h = gc.GetHue() / 360.0f;
            float l = gc.GetBrightness();
            float s = gc.GetSaturation() * expansionFactor;
            if (s > 1.0f) { s = 1.0f; }

            return Color.Hsl(h, s, l);
        }

        public static Color Median(Color color1, Color color2)
        {
            Color median = new Color();

            median.R = (byte)((color1.R + color2.R) / 2);
            median.G = (byte)((color1.G + color2.G) / 2);
            median.B = (byte)((color1.B + color2.B) / 2);

            return median;
        }

        public static Color Hsl(double H, double S, double L)
        {
            double v;
            double r, g, b;

            r = L;   // default to gray
            g = L;
            b = L;
            v = (L <= 0.5) ? (L * (1.0 + S)) : (L + S - L * S);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = L + L - v;
                sv = (v - m) / v;
                H *= 6.0;
                sextant = (int)H;
                fract = H - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            Color rgb = new Color();
            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);
            return rgb;
        }

        public static Color Gdi(System.Drawing.Color color)
        {
            return new Color(color);
        }

        protected Color(byte red, byte green, byte blue)
        {
            this.R = red;
            this.G = green;
            this.B = blue;
        }

        public Color(System.Drawing.Color color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }

        public System.Drawing.Color GdiColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(this.R, this.G, this.B);
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("BotDetect.Drawing.Color { ");

            str.Append("red: ");
            str.Append(StringHelper.ToString(_redRange));

            str.Append(", green: ");
            str.Append(StringHelper.ToString(_greenRange));

            str.Append(", blue: ");
            str.Append(StringHelper.ToString(_blueRange));

            str.Append(" }");

            return str.ToString();
        }

        public Color Complement
        {
            get
            {
                Color complement = new Color();
                complement.RedRange = new RandomRange(255 - this.RedRange.Max, 255 - this.RedRange.Min);
                complement.GreenRange = new RandomRange(255 - this.GreenRange.Max, 255 - this.GreenRange.Min);
                complement.BlueRange = new RandomRange(255 - this.BlueRange.Max, 255 - this.BlueRange.Min);

                return complement;
            }
        }

        public void Freeze()
        {
            _blueRange.Freeze();
            _greenRange.Freeze();
            _redRange.Freeze();
        }

        public Color Frozen
        {
            get
            {
                this.Freeze();
                return this;
            }
        }

        public bool IsRandomized
        {
            get
            {
                return _redRange.IsRandomized || _greenRange.IsRandomized || _blueRange.IsRandomized;
            }
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter writer = new SerializationWriter();
            writer.WriteObject(_redRange);
            writer.WriteObject(_greenRange);
            writer.WriteObject(_blueRange);
            //writer.WriteOptimized(_redRange.Min);
            //writer.WriteOptimized(_redRange.Max);
            //writer.WriteOptimized(_greenRange.Min);
            //writer.WriteOptimized(_greenRange.Max);
            //writer.WriteOptimized(_blueRange.Min);
            //writer.WriteOptimized(_blueRange.Max);

            info.AddValue("data", writer.ToArray());
        }

        protected Color(SerializationInfo info, StreamingContext context)
        {
            SerializationReader reader = new SerializationReader((byte[])info.GetValue("data", typeof(byte[])));
            _redRange = reader.ReadObject() as RandomRange;
            _greenRange = reader.ReadObject() as RandomRange;
            _blueRange = reader.ReadObject() as RandomRange;
        }

        #endregion
    }
}
