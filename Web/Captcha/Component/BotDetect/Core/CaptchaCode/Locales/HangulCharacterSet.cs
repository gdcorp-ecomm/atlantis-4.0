using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class HangulCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        private static readonly Int32[] _alphaCodePoints = { 
            0x1100, // (Choseong Kiyeok)   ᄀ
            0x1102, // (Choseong Nieun)    ᄂ
            0x1103, // (Choseong Tikeut)   ᄃ
            0x1105, // (Choseong Rieul)    ᄅ
            0x1106, // (Choseong Mieum)    ᄆ
            0x1107, // (Choseong Pieup)    ᄇ
            0x1109, // (Choseong Sios)     ᄉ
            0x110B, // (Choseong Ieung)    ᄋ
            0x110C, // (Choseong Cieuc)    ᄌ
            0x110E, // (Choseong Chieuch)  ᄎ
            0x110F, // (Choseong Khieukh)  ᄏ
            0x1110, // (Choseong Thieuth)  ᄐ
            0x1111, // (Choseong Phieuph)  ᄑ
            0x1112, // (Choseong Hieuh)    ᄒ
            0x1161, // (Jungseong A)       ᅡ
            0x1162, // (Jungseong Ae)      ᅢ
            0x1163, // (Jungseong Ya)      ᅣ
            0x1165, // (Jungseong Eo)      ᅥ
            0x1166, // (Jungseong E)       ᅦ
            0x1167, // (Jungseong Yeo)     ᅧ
            0x1169, // (Jungseong O)       ᅩ
            0x116D, // (Jungseong Yo)      ᅭ
            0x116E, // (Jungseong U)       ᅮ
            0x1172, // (Jungseong Yu)      ᅲ
            //0x1173, // (Jungseong Eu)      ᅳ
            //0x1175  // (Jungseong I)       ᅵ
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
            0x1100, // (Choseong Kiyeok)   ᄀ
            0x1102, // (Choseong Nieun)    ᄂ
            0x1103, // (Choseong Tikeut)   ᄃ
            0x1105, // (Choseong Rieul)    ᄅ
            0x1106, // (Choseong Mieum)    ᄆ
            0x1107, // (Choseong Pieup)    ᄇ
            0x1109, // (Choseong Sios)     ᄉ
            //0x110B, // (Choseong Ieung)    ᄋ
            0x110C, // (Choseong Cieuc)    ᄌ
            0x110E, // (Choseong Chieuch)  ᄎ
            0x110F, // (Choseong Khieukh)  ᄏ
            0x1110, // (Choseong Thieuth)  ᄐ
            0x1111, // (Choseong Phieuph)  ᄑ
            0x1112, // (Choseong Hieuh)    ᄒ
            0x1161, // (Jungseong A)       ᅡ
            0x1162, // (Jungseong Ae)      ᅢ
            0x1163, // (Jungseong Ya)      ᅣ
            0x1165, // (Jungseong Eo)      ᅥ
            0x1166, // (Jungseong E)       ᅦ
            0x1167, // (Jungseong Yeo)     ᅧ
            0x1169, // (Jungseong O)       ᅩ
            0x116D, // (Jungseong Yo)      ᅭ
            0x116E, // (Jungseong U)       ᅮ
            0x1172, // (Jungseong Yu)      ᅲ
            //0x1173, // (Jungseong Eu)      ᅳ
            //0x1175, // (Jungseong I)       ᅵ
            //0x0030, // 0
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
        private HangulCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_HangulCharacterSet")
        {
        }

        private static readonly HangulCharacterSet _instance = new HangulCharacterSet();

        public static HangulCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
