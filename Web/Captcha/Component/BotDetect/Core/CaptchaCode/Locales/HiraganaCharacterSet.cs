using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class HiraganaCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        private static readonly Int32[] _alphaCodePoints = { 
            0x3042, // (A)    あ
            0x3044, // (I)    い
            0x3046, // (U)    う
            0x3048, // (E)    え
            0x304A, // (O)    お
            0x304B, // (Ka)   か
            0x304D, // (Ki)   き
            0x304F, // (Ku)   く
            0x3051, // (Ke)   け
            0x3053, // (Ko)   こ
            0x3055, // (Sa)   さ
            0x3057, // (Si)   し
            0x3059, // (Su)   す
            0x305B, // (Se)   せ
            0x305D, // (So)   そ
            0x305F, // (Ta)   た
            0x3061, // (Ti)   ち
            0x3064, // (Tu)   つ
            0x3066, // (Te)   て
            0x3068, // (To)   と
            0x306A, // (Na)   な
            0x306B, // (Ni)   に
            0x306C, // (Nu)   ぬ
            0x306D, // (Ne)   ね
            0x306E, // (No)   の
            0x306F, // (Ha)   は
            0x3072, // (Hi)   ひ
            0x3075, // (Hu)   ふ
            0x3078, // (He)   へ
            0x307B, // (Ho)   ほ
            0x307E, // (Ma)   ま
            0x307F, // (Mi)   み
            0x3080, // (Mu)   む
            0x3081, // (Me)   め
            0x3082, // (Mo)   も
            0x3084, // (Ya)   や
            0x3086, // (Yu)   ゆ
            0x3088, // (Yo)   よ
            0x3089, // (Ra)   ら
            0x308A, // (Ri)   り
            0x308B, // (Ru)   る
            0x308C, // (Re)   れ
            0x308D, // (Ro)   ろ
            0x308F, // (Wa)   わ
            0x3093  // (N)    ん
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
            0x3042, // (A)    あ
            0x3044, // (I)    い
            0x3046, // (U)    う
            0x3048, // (E)    え
            0x304A, // (O)    お
            0x304B, // (Ka)   か
            0x304D, // (Ki)   き
            0x304F, // (Ku)   く
            0x3051, // (Ke)   け
            0x3053, // (Ko)   こ
            0x3055, // (Sa)   さ
            0x3057, // (Si)   し
            0x3059, // (Su)   す
            0x305B, // (Se)   せ
            0x305D, // (So)   そ
            0x305F, // (Ta)   た
            0x3061, // (Ti)   ち
            0x3064, // (Tu)   つ
            0x3066, // (Te)   て
            0x3068, // (To)   と
            0x306A, // (Na)   な
            0x306B, // (Ni)   に
            0x306C, // (Nu)   ぬ
            0x306D, // (Ne)   ね
            0x306E, // (No)   の
            0x306F, // (Ha)   は
            0x3072, // (Hi)   ひ
            0x3075, // (Hu)   ふ
            0x3078, // (He)   へ
            0x307B, // (Ho)   ほ
            0x307E, // (Ma)   ま
            0x307F, // (Mi)   み
            0x3080, // (Mu)   む
            0x3081, // (Me)   め
            0x3082, // (Mo)   も
            0x3084, // (Ya)   や
            0x3086, // (Yu)   ゆ
            0x3088, // (Yo)   よ
            0x3089, // (Ra)   ら
            0x308A, // (Ri)   り
            0x308B, // (Ru)   る
            0x308C, // (Re)   れ
            0x308D, // (Ro)   ろ
            0x308F, // (Wa)   わ
            0x3093, // (N)    ん
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
        private HiraganaCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_HiraganaCharacterSet")
        {
        }

        private static readonly HiraganaCharacterSet _instance = new HiraganaCharacterSet();

        public static HiraganaCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
