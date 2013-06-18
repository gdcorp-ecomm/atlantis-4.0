using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class ArabicCharacterSet : CharacterSet
    {
        /// <summary>
        /// Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        /// </summary>
        private static readonly Int32[] _alphaCodePoints = { 
            //0x0627, // (Alef)   ا
            0x0628, // (Beh)    ب
            0x062A, // (Teh)    ت
            0x062B, // (Theh)   ث
            0x062C, // (Jeem)   ج
            0x062D, // (Hah)    ح
            0x062E, // (Khah)   خ
            0x062F, // (Dal)    د
            0x0630, // (Thal)   ذ
            0x0631, // (Reh)    ر 
            0x0632, // (Zain)   ز
            0x0633, // (Seen)   س
            0x0634, // (Sheen)  ش
            0x0635, // (Sad)    ص
            0x0636, // (Dad)    ض
            0x0637, // (Tah)    ط
            0x0638, // (Zah)    ظ
            0x0639, // (Ain)    ع
            0x063A, // (Ghain)  غ
            0x0641, // (Feh)    ف
            0x0642, // (Qaf)    ق
            0x0643, // (Kaf)    ك
            0x0644, // (Lam)    ل
            0x0645, // (Meem)   م
            0x0646, // (Noon)   ن
            0x0647, // (Heh)    ه
            0x0648, // (Waw)    و
            0x064A  // (Yeh)    ي  
        };

        /// <summary>
        /// Unicode Code points (32-bit integers) of characters to use for Numeric code generation
        /// </summary>
        private static readonly Int32[] _numericCodePoints = { 
            0x0030, // 0
            0x0031, // 1
            0x0032, // 2
            0x0033, // 3
            0x0034, // 4
            0x0035, // 5
            0x0036, // 6
            //0x0037, // 7
            0x0038, // 8
            0x0039  // 9
        };

        /// <summary>
        /// Unicode Code points (32-bit integers) of characters to use for AlphaNumeric code generation
        /// </summary>
        private static readonly Int32[] _alphanumericCodePoints = {
            //0x0627, // (Alef)   ا
            0x0628, // (Beh)    ب
            0x062A, // (Teh)    ت
            0x062B, // (Theh)   ث
            0x062C, // (Jeem)   ج
            0x062D, // (Hah)    ح
            0x062E, // (Khah)   خ
            0x062F, // (Dal)    د
            0x0630, // (Thal)   ذ
            0x0631, // (Reh)    ر 
            0x0632, // (Zain)   ز
            0x0633, // (Seen)   س
            0x0634, // (Sheen)  ش
            0x0635, // (Sad)    ص
            0x0636, // (Dad)    ض
            0x0637, // (Tah)    ط
            0x0638, // (Zah)    ظ
            0x0639, // (Ain)    ع
            0x063A, // (Ghain)  غ
            0x0641, // (Feh)    ف
            0x0642, // (Qaf)    ق
            0x0643, // (Kaf)    ك
            0x0644, // (Lam)    ل
            0x0645, // (Meem)   م
            0x0646, // (Noon)   ن
            0x0647, // (Heh)    ه
            0x0648, // (Waw)    و
            0x064A, // (Yeh)    ي  
            0x0030, // 0
            //0x0031, // 1
            0x0032, // 2
            0x0033, // 3
            0x0034, // 4
            0x0035, // 5
            0x0036, // 6
            //0x0037, // 7
            0x0038, // 8
            0x0039  // 9
        };

        // singleton
        private ArabicCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_ArabicCharacterSet")
        {
        }

        private static readonly ArabicCharacterSet _instance = new ArabicCharacterSet();

        public static ArabicCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
