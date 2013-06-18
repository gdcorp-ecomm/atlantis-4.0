using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class HanSimplifiedCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        private static readonly Int32[] _alphaCodePoints = { 
            0x91D1, // (gold)     金
            0x4EBA, // (person)   人
            0x6708, // (moon)     月
            0x767D, // (white)    白
            0x79BE, // (grain)    禾
            0x8A00, // (speech)   言
            0x7ACB, // (stand)    立
            0x6C34, // (water)    水
            0x706B, // (fire)     火
            0x4E4B, // (it)       之
            0x5DE5, // (work)     工
            0x6728, // (wood)     木
            0x5927, // (big)      大
            0x571F, // (earth)    土 
            0x738B, // (king)     王  
            0x76EE, // (eye)      目
            0x65E5, // (sun)      日
            0x53E3, // (mouth)    口
            0x7530, // (field)    田
            0x5C71, // (mountain) 山
            0x53C8, // (again)    又
            0x5973, // (woman)    女
            0x5B50, // (child)    子
            0x5DF2  // (already)  已
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
            0x91D1, // (gold)     金
            0x4EBA, // (person)   人
            0x6708, // (moon)     月
            0x767D, // (white)    白
            0x79BE, // (grain)    禾
            0x8A00, // (speech)   言
            0x7ACB, // (stand)    立
            0x6C34, // (water)    水
            0x706B, // (fire)     火
            0x4E4B, // (it)       之
            0x5DE5, // (work)     工
            0x6728, // (wood)     木
            0x5927, // (big)      大
            0x571F, // (earth)    土 
            0x738B, // (king)     王  
            0x76EE, // (eye)      目
            0x65E5, // (sun)      日
            0x53E3, // (mouth)    口
            0x7530, // (field)    田
            0x5C71, // (mountain) 山
            0x53C8, // (again)    又
            0x5973, // (woman)    女
            0x5B50, // (child)    子
            0x5DF2, // (already)  已
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
        private HanSimplifiedCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_HanSimplifiedCharacterSet")
        {
        }

        private static readonly HanSimplifiedCharacterSet _instance = new HanSimplifiedCharacterSet();

        public static HanSimplifiedCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
