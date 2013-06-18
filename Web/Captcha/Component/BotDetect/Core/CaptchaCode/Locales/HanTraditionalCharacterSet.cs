using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class HanTraditionalCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        private static readonly Int32[] _alphaCodePoints = { 
            //0x4E00, // (one)              一
            0x4E2D, // (centre)           中
            0x4EBA, // (person)           人
            0x5341, // (ten)              十
            0x535C, // (fortune telling)  卜
            0x53E3, // (mouth)            口
            0x571F, // (earth)            土
            0x5927, // (big)              大
            0x5973, // (woman)            女
            0x5C38, // (corpse)           尸
            0x5C71, // (mountain)         山
            0x5EFF, // (twenty)           廿
            0x5F13, // (bow)              弓
            0x5FC3, // (heart)            心
            0x6208, // (weapon)           戈
            0x624B, // (hand)             手
            0x65E5, // (sun)              日
            0x6708, // (moon)             月
            0x6728, // (wood)             木
            0x6C34, // (water)            水
            0x706B, // (fire)             火
            0x7530, // (field)            田
            0x7AF9, // (bamboo)           竹
            0x91D1  // (gold)             金
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
            //0x4E00, // (one)              一
            0x4E2D, // (centre)           中
            0x4EBA, // (person)           人
            0x5341, // (ten)              十
            0x535C, // (fortune telling)  卜
            0x53E3, // (mouth)            口
            0x571F, // (earth)            土
            0x5927, // (big)              大
            0x5973, // (woman)            女
            0x5C38, // (corpse)           尸
            0x5C71, // (mountain)         山
            0x5EFF, // (twenty)           廿
            0x5F13, // (bow)              弓
            0x5FC3, // (heart)            心
            0x6208, // (weapon)           戈
            0x624B, // (hand)             手
            0x65E5, // (sun)              日
            0x6708, // (moon)             月
            0x6728, // (wood)             木
            0x6C34, // (water)            水
            0x706B, // (fire)             火
            0x7530, // (field)            田
            0x7AF9, // (bamboo)           竹
            0x91D1, // (gold)             金
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
        private HanTraditionalCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_HanTraditionalCharacterSet")
        {
        }

        private static readonly HanTraditionalCharacterSet _instance = new HanTraditionalCharacterSet();

        public static HanTraditionalCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
