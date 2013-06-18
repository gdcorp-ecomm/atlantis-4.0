using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    [Serializable]
    internal class Font
    {
        protected FontCase fontCase;

        public FontCase Case
        {
            get
            {
                return fontCase;
            }

            set
            {
                fontCase = value;
            }
        }

        protected FontWeight fontWeight;

        public FontWeight Weight
        {
            get
            {
                return fontWeight;
            }

            set
            {
                fontWeight = value;
            }
        }

        protected string fontFamily;

        public string FontFamily
        {
            get
            {
                return fontFamily;
            }

            set
            {
                fontFamily = value;
            }
        }

        public Font(string familyName, FontCase FontCase, FontWeight FontWeight)
        {
            this.FontFamily = familyName;
            this.Case = FontCase;
            this.Weight = FontWeight;
        }

        public static Font From(string familyName, FontCase FontCase, FontWeight FontWeight)
        {
            return new Font(familyName, FontCase, FontWeight);
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture,
                "{0} {1} {2}",
                fontFamily, fontCase, fontWeight);
        }
    }
}
