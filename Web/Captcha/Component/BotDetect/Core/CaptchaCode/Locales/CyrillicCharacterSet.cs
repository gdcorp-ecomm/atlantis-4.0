using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class CyrillicCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        private static readonly Int32[] _alphaCodePoints = { 
            0x0410, // (A)    А
            0x0411, // (Be)   Б
            0x0412, // (Ve)   В
            0x0413, // (Ghe)  Г
            0x0414, // (De)   Д
            0x0415, // (Ie)   Е
            0x0416, // (Zhe)  Ж
            0x0417, // (Ze)   З
            0x0418, // (I)    И
            0x041A, // (Ka)   К
            0x041B, // (El)   Л
            0x041C, // (Em)   М
            0x041D, // (En)   Н
            0x041E, // (O)    О
            0x041F, // (Pe)   П
            0x0420, // (Er)   Р
            0x0421, // (Es)   С
            0x0422, // (Te)   Т
            0x0423, // (U)    У
            0x0424, // (Ef)   Ф
            0x0425, // (Ha)   Х
            0x0426, // (Tse)  Ц
            0x0427, // (Che)  Ч
            0x0428, // (Sha)  Ш
            0x042F  // (Ya)   Я
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
            0x0410, // (A)    А
            0x0411, // (Be)   Б
            0x0412, // (Ve)   В
            0x0413, // (Ghe)  Г
            0x0414, // (De)   Д
            0x0415, // (Ie)   Е
            0x0416, // (Zhe)  Ж
            //0x0417, // (Ze)   З
            0x0418, // (I)    И
            0x041A, // (Ka)   К
            0x041B, // (El)   Л
            0x041C, // (Em)   М
            0x041D, // (En)   Н
            0x041F, // (Pe)   П
            0x0420, // (Er)   Р
            0x0421, // (Es)   С
            0x0422, // (Te)   Т
            0x0423, // (U)    У
            0x0424, // (Ef)   Ф
            0x0425, // (Ha)   Х
            0x0426, // (Tse)  Ц
            0x0427, // (Che)  Ч
            0x0428, // (Sha)  Ш
            0x042F, // (Ya)   Я
            0x0031, // 1
            0x0032, // 2
            //0x0033, // 3
            0x0034, // 4
            0x0035, // 5
            0x0036, // 6
            //0x0037, // 7
            0x0038, // 8
            0x0039  // 9
        };

        // singleton
        private CyrillicCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_CyrillicCharacterSet")
        {
        }

        private static readonly CyrillicCharacterSet _instance = new CyrillicCharacterSet();

        public static CyrillicCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
