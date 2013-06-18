using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal enum FontWeight
    {
        Normal = 0,
        Bold,
        Random
    }

    internal sealed class FontWeightHelper
    {
        private FontWeightHelper()
        {
        }

        public static int GdiStyleValue(FontWeight weight)
        {
            return (int) GdiStyle(weight);
        }

        public static System.Drawing.FontStyle GdiStyle(FontWeight weight)
        {
            System.Drawing.FontStyle style = System.Drawing.FontStyle.Regular;

            switch (weight)
            {
                case FontWeight.Normal:
                    style = System.Drawing.FontStyle.Regular;
                    break;

                case FontWeight.Bold:
                    style = System.Drawing.FontStyle.Bold;
                    break;

                case FontWeight.Random:
                    FontWeight determinedWeight = (FontWeight)RandomGenerator.Next(0, 2);
                    style = GdiStyle(determinedWeight);
                    break;
            }

            return style;
        }
    }
}
