using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class KatakanaCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        private static readonly Int32[] _alphaCodePoints = { 
            0x30A2, // (A)   ア
            0x30A4, // (I)   イ
            0x30A6, // (U)   ウ
            0x30A8, // (E)   エ
            0x30AA, // (O)   オ
            0x30AB, // (Ka)  カ
            0x30AD, // (Ki)  キ
            0x30AF, // (Ku)  ク
            0x30B1, // (Ke)  ケ
            0x30B3, // (Ko)  コ
            0x30B5, // (Sa)  サ
            0x30B7, // (Si)  シ
            0x30B9, // (Su)  ス
            0x30BB, // (Se)  セ
            0x30BD, // (So)  ソ
            0x30BF, // (Ta)  タ
            0x30C1, // (Ti)  チ
            0x30C4, // (Tu)  ツ
            0x30C6, // (Te)  テ
            0x30C8, // (To)  ト
            0x30CA, // (Na)  ナ
            0x30CB, // (Ni)  ニ
            0x30CC, // (Nu)  ヌ
            0x30CD, // (Ne)  ネ
            0x30CE, // (No)  ノ
            0x30CF, // (Ha)  ハ
            0x30D2, // (Hi)  ヒ
            0x30D5, // (Hu)  フ
            0x30D8, // (He)  ヘ
            0x30DB, // (Ho)  ホ
            0x30DE, // (Ma)  マ
            0x30DF, // (Mi)  ミ
            0x30E0, // (Mu)  ム
            0x30E1, // (Me)  メ
            0x30E2, // (Mo)  モ
            0x30E4, // (Ya)  ヤ
            0x30E6, // (Yu)  ユ
            0x30E8, // (Yo)  ヨ
            0x30E9, // (Ra)  ラ
            0x30EA, // (Ri)  リ
            0x30EB, // (Ru)  ル
            0x30EC, // (Re)  レ
            0x30ED, // (Ro)  ロ
            0x30EF, // (Wa)  ワ
            0x30F3  // (N)   ン
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
            0x30A2, // (A)   ア
            0x30A4, // (I)   イ
            0x30A6, // (U)   ウ
            0x30A8, // (E)   エ
            0x30AA, // (O)   オ
            0x30AB, // (Ka)  カ
            0x30AD, // (Ki)  キ
            0x30AF, // (Ku)  ク
            0x30B1, // (Ke)  ケ
            0x30B3, // (Ko)  コ
            0x30B5, // (Sa)  サ
            0x30B7, // (Si)  シ
            0x30B9, // (Su)  ス
            0x30BB, // (Se)  セ
            0x30BD, // (So)  ソ
            0x30BF, // (Ta)  タ
            0x30C1, // (Ti)  チ
            0x30C4, // (Tu)  ツ
            0x30C6, // (Te)  テ
            0x30C8, // (To)  ト
            0x30CA, // (Na)  ナ
            0x30CB, // (Ni)  ニ
            0x30CC, // (Nu)  ヌ
            0x30CD, // (Ne)  ネ
            0x30CE, // (No)  ノ
            0x30CF, // (Ha)  ハ
            0x30D2, // (Hi)  ヒ
            0x30D5, // (Hu)  フ
            0x30D8, // (He)  ヘ
            0x30DB, // (Ho)  ホ
            0x30DE, // (Ma)  マ
            0x30DF, // (Mi)  ミ
            0x30E0, // (Mu)  ム
            0x30E1, // (Me)  メ
            0x30E2, // (Mo)  モ
            0x30E4, // (Ya)  ヤ
            0x30E6, // (Yu)  ユ
            0x30E8, // (Yo)  ヨ
            0x30E9, // (Ra)  ラ
            0x30EA, // (Ri)  リ
            0x30EB, // (Ru)  ル
            0x30EC, // (Re)  レ
            0x30ED, // (Ro)  ロ
            0x30EF, // (Wa)  ワ
            0x30F3, // (N)   ン
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
        private KatakanaCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_KatakanaCharacterSet")
        {
        }

        private static readonly KatakanaCharacterSet _instance = new KatakanaCharacterSet();

        public static KatakanaCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
