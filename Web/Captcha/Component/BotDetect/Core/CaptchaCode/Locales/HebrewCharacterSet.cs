using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class HebrewCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        private static readonly Int32[] _alphaCodePoints = { 
            0x05D0, // (Alef)    א
            0x05D1, // (Bet)     ב
            0x05D2, // (Gimel)   ג
            0x05D3, // (Dalet)   ד
            0x05D4, // (He)      ה
            //0x05D5, // (Vav)     ו
            0x05D6, // (Zayin)   ז
            0x05D7, // (Het)     ח
            0x05D8, // (Tet)     ט
            0x05DB, // (Kaf)     כ
            0x05DC, // (Lamed)   ל
            0x05DE, // (Mem)     מ
            0x05E0, // (Nun)     נ
            0x05E1, // (Samekh)  ס
            0x05E2, // (Ayin)    ע
            0x05E4, // (Pe)      פ
            0x05E6, // (Tsadi)   צ
            0x05E7, // (Qof)     ק
            0x05E8, // (Resh)    ר
            0x05E9, // (Shin)    ש
            0x05EA  // (Tav)     ת
        };

        // Unicode Code points (32-bit integers) of characters to use for Numeric code generation
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

        // Unicode Code points (32-bit integers) of characters to use for AlphaNumeric code generation
        private static readonly Int32[] _alphanumericCodePoints = { 
            0x05D0, // (Alef)    א
            0x05D1, // (Bet)     ב
            0x05D2, // (Gimel)   ג
            0x05D3, // (Dalet)   ד
            0x05D4, // (He)      ה
            //0x05D5, // (Vav)     ו
            0x05D6, // (Zayin)   ז
            0x05D7, // (Het)     ח
            0x05D8, // (Tet)     ט
            0x05DB, // (Kaf)     כ
            0x05DC, // (Lamed)   ל
            0x05DE, // (Mem)     מ
            0x05E0, // (Nun)     נ
            0x05E1, // (Samekh)  ס
            0x05E2, // (Ayin)    ע
            0x05E4, // (Pe)      פ
            0x05E6, // (Tsadi)   צ
            0x05E7, // (Qof)     ק
            0x05E8, // (Resh)    ר
            0x05E9, // (Shin)    ש
            0x05EA, // (Tav)     ת
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

        // singleton
        private HebrewCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_HebrewCharacterSet")
        {
        }

        private static readonly HebrewCharacterSet _instance = new HebrewCharacterSet();

        public static HebrewCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
