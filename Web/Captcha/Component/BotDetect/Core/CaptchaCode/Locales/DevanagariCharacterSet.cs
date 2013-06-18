using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class DevanagariCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        private static readonly Int32[] _alphaCodePoints = { 
            0x0905, // (A)  अ
            0x0906, // (Aa)  आ
            0x0907, // (I)  इ
            //0x0908, // (Ii)  ई
            0x0909, // (U)  उ
            //0x090A, // (Uu)  ऊ
            0x090B, // (Vocalic R)  ऋ
            0x090C, // (Vocalic L)  ऌ
            //0x090D, // (Candra E)  ऍ
            //0x090E, // (Short E)  ऎ
            0x090F, // (E)  ए
            //0x0910, // (Ai)  ऐ
            //0x0911, // (Candra O)  ऑ
            //0x0912, // (Short O)  ऒ
            0x0913, // (O)  ओ
            //0x0914, // (Au)  औ
            0x0915, // (Ka)  क
            0x0916, // (Kha)  ख
            0x0917, // (Ga)  ग
            0x0918, // (Gha)  घ
            0x0919, // (Nga)  ङ
            0x091A, // (Ca)  च
            0x091B, // (Cha)  छ
            0x091C, // (Ja)  ज
            0x091D, // (Jha)  झ
            0x091E, // (Nya)  ञ
            0x091F, // (Tta)  ट
            0x0920, // (Ttha)  ठ
            0x0921, // (Dda)  ड
            0x0922, // (Ddha)  ढ
            0x0923, // (Nna)  ण
            0x0924, // (Ta)  त
            0x0925, // (Tha)  थ
            0x0926, // (Da)  द
            0x0927, // (Dha)  ध
            0x0928, // (Na)  न
            0x0929, // (Nnna)  ऩ
            0x092A, // (Pa)  प
            0x092B, // (Pha)  फ
            0x092C, // (Ba)  ब
            0x092D, // (Bha)  भ
            0x092E, // (Ma)  म
            0x092F, // (Ya)  य
            0x0930, // (Ra)   र
            0x0931, // (Rra)  ऱ
            0x0932, // (La)  ल
            0x0933, // (Lla)  ळ
            //0x0934, // (Llla)  ऴ
            //0x0935, // (Va)  व
            //0x0936, // (Sha)  श
            //0x0937, // (Ssa)  ष
            //0x0938, // (Sa)  स
            //0x0939, // (Ha)  ह
            //0x0958, // (Qa)  क़
            //0x0959, // (Khha)  ख़
            //0x095A, // (Ghha)  ग़
            //0x095B, // (Za)  ज़
            //0x095C, // (Dddha)  ड़
            //0x095D, // (Rha)  ढ़
            //0x095E, // (Fa)  फ़
            //0x095F, // (Yya)  य़
            //0x0960, // (Vocalic Rr)  ॠ
            //0x0961 // (Vocalic Ll)  ॡ
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
        /*private static readonly int[] _numericCodePoints = { 
            0x0966, // (Digit Zero)  ०
            0x0967, // (Digit One)  १
            0x0968, // (Digit Two)  २
            0x0969, // (Digit Three)  ३
            0x096A, // (Digit Four)  ४
            0x096B, // (Digit Five)  ५
            0x096C, // (Digit Six)  ६
            0x096D, // (Digit Seven)  ७
            0x096E, // (Digit Eight)  ८
            0x096F // (Digit Nine)  ९
        };*/

        // Unicode Code points (32-bit integers) of characters to use for AlphaNumeric code generation
        private static readonly Int32[] _alphanumericCodePoints = { 
            0x0905, // (A)  अ
            0x0906, // (Aa)  आ
            0x0907, // (I)  इ
            //0x0908, // (Ii)  ई
            0x0909, // (U)  उ
            //0x090A, // (Uu)  ऊ
            0x090B, // (Vocalic R)  ऋ
            0x090C, // (Vocalic L)  ऌ
            //0x090D, // (Candra E)  ऍ
            //0x090E, // (Short E)  ऎ
            0x090F, // (E)  ए
            //0x0910, // (Ai)  ऐ
            //0x0911, // (Candra O)  ऑ
            //0x0912, // (Short O)  ऒ
            0x0913, // (O)  ओ
            0x0914, // (Au)  औ
            0x0915, // (Ka)  क
            0x0916, // (Kha)  ख
            0x0917, // (Ga)  ग
            0x0918, // (Gha)  घ
            0x0919, // (Nga)  ङ
            0x091A, // (Ca)  च
            0x091B, // (Cha)  छ
            0x091C, // (Ja)  ज
            0x091D, // (Jha)  झ
            0x091E, // (Nya)  ञ
            0x091F, // (Tta)  ट
            0x0920, // (Ttha)  ठ
            0x0921, // (Dda)  ड
            0x0922, // (Ddha)  ढ
            0x0923, // (Nna)  ण
            0x0924, // (Ta)  त
            0x0925, // (Tha)  थ
            0x0926, // (Da)  द
            0x0927, // (Dha)  ध
            0x0928, // (Na)  न
            0x0929, // (Nnna)  ऩ
            0x092A, // (Pa)  प
            0x092B, // (Pha)  फ
            0x092C, // (Ba)  ब
            0x092D, // (Bha)  भ
            0x092E, // (Ma)  म
            0x092F, // (Ya)  य
            0x0930, // (Ra)   र
            0x0931, // (Rra)  ऱ
            0x0932, // (La)  ल
            0x0933, // (Lla)  ळ
            //0x0934, // (Llla)  ऴ
            //0x0935, // (Va)  व
            //0x0936, // (Sha)  श
            //0x0937, // (Ssa)  ष
            //0x0938, // (Sa)  स
            //0x0939, // (Ha)  ह
            //0x0958, // (Qa)  क़
            //0x0959, // (Khha)  ख़
            //0x095A, // (Ghha)  ग़
            //0x095B, // (Za)  ज़
            //0x095C, // (Dddha)  ड़
            //0x095D, // (Rha)  ढ़
            //0x095E, // (Fa)  फ़
            //0x095F, // (Yya)  य़
            //0x0960, // (Vocalic Rr)  ॠ
            //0x0961, // (Vocalic Ll)  ॡ
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
            /*0x0966, // (Digit Zero)  ०
            0x0967, // (Digit One)  १
            0x0968, // (Digit Two)  २
            0x0969, // (Digit Three)  ३
            0x096A, // (Digit Four)  ४
            0x096B, // (Digit Five)  ५
            0x096C, // (Digit Six)  ६
            0x096D, // (Digit Seven)  ७
            0x096E, // (Digit Eight)  ८
            0x096F // (Digit Nine)  ९*/
        };

        // singleton
        private DevanagariCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_DevanagariCharacterSet")
        {
        }

        private static readonly DevanagariCharacterSet _instance = new DevanagariCharacterSet();

        public static DevanagariCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
