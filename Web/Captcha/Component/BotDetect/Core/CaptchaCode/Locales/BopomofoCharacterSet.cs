using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class BopomofoCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        private static readonly Int32[] _alphaCodePoints = { 
            0x3105, // (B)  ㄅ
            0x3106, // (P)  ㄆ
            0x3107, // (M)  ㄇ
            0x3108, // (F)  ㄈ
            0x3109, // (D)  ㄉ
            0x310A, // (T)  ㄊ
            0x310B, // (N)  ㄋ
            0x310C, // (L)  ㄌ
            0x310D, // (G)  ㄍ
            0x310E, // (K)  ㄎ
            0x310F, // (H)  ㄏ
            0x3110, // (J)  ㄐ
            0x3111, // (Q)  ㄑ
            0x3112, // (X)  ㄒ
            0x3113, // (Zh)  ㄓ
            0x3114, // (Ch)  ㄔ
            0x3115, // (Sh)  ㄕ
            0x3116, // (R)  ㄖ
            0x3117, // (Z)  ㄗ
            0x3118, // (C)  ㄘ
            0x3119, // (S)  ㄙ
            0x311A, // (A)  ㄚ
            0x311B, // (O)  ㄛ
            0x311C, // (E)  ㄜ
            0x311D, // (Eh)  ㄝ
            0x311E, // (Ai)  ㄞ
            //0x311F, // (Ei)  ㄟ
            0x3120, // (Au)  ㄠ
            0x3121, // (Ou)  ㄡ
            0x3122, // (An)  ㄢ
            0x3123, // (En)  ㄣ
            0x3124, // (Ang)  ㄤ
            0x3125, // (Eng)  ㄥ
            0x3126, // (Er)  ㄦ
            //0x3127, // (I)  ㄧ
            0x3128, // (U)  ㄨ
            0x3129  // (Iu)  ㄩ 
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
            0x3105, // (B)  ㄅ
            0x3106, // (P)  ㄆ
            0x3107, // (M)  ㄇ
            0x3108, // (F)  ㄈ
            0x3109, // (D)  ㄉ
            0x310A, // (T)  ㄊ
            0x310B, // (N)  ㄋ
            0x310C, // (L)  ㄌ
            0x310D, // (G)  ㄍ
            0x310E, // (K)  ㄎ
            0x310F, // (H)  ㄏ
            0x3110, // (J)  ㄐ
            0x3111, // (Q)  ㄑ
            0x3112, // (X)  ㄒ
            0x3113, // (Zh)  ㄓ
            0x3114, // (Ch)  ㄔ
            0x3115, // (Sh)  ㄕ
            0x3116, // (R)  ㄖ
            0x3117, // (Z)  ㄗ
            0x3118, // (C)  ㄘ
            0x3119, // (S)  ㄙ
            0x311A, // (A)  ㄚ
            0x311B, // (O)  ㄛ
            0x311C, // (E)  ㄜ
            0x311D, // (Eh)  ㄝ
            0x311E, // (Ai)  ㄞ
            //0x311F, // (Ei)  ㄟ
            0x3120, // (Au)  ㄠ
            0x3121, // (Ou)  ㄡ
            0x3122, // (An)  ㄢ
            0x3123, // (En)  ㄣ
            0x3124, // (Ang)  ㄤ
            0x3125, // (Eng)  ㄥ
            0x3126, // (Er)  ㄦ
            //0x3127, // (I)  ㄧ
            0x3128, // (U)  ㄨ
            0x3129, // (Iu)  ㄩ 
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
        private BopomofoCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_BopomofoCharacterSet")
        {
        }

        private static readonly BopomofoCharacterSet _instance = new BopomofoCharacterSet();

        public static BopomofoCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
