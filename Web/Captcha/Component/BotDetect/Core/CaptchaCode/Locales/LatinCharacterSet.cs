using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class LatinCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        internal static readonly Int32[] _alphaCodePoints = { 
            0x0041, // A
            0x0042, // B
            0x0043, // C
            0x0044, // D
            0x0045, // E
            //0x0046, // F
            //0x0047, // G
            0x0048, // H
            //0x0049, // I
            0x004A, // J
            0x004B, // K
            //0x004C, // L
            0x004D, // M
            0x004E, // N
            0x004F, // O
            0x0050, // P
            //0x0051, // Q
            0x0052, // R
            0x0053, // S
            0x0054, // T
            0x0055, // U
            0x0056, // V
            0x0057, // W
            0x0058, // X
            0x0059, // Y
            0x005A  // Z
        };

        // Unicode Code points (32-bit integers) of characters to use for Numeric code generation
        internal static readonly Int32[] _numericCodePoints = { 
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
        internal static readonly Int32[] _alphanumericCodePoints = { 
            0x0041, // A
            0x0042, // B
            0x0043, // C
            0x0044, // D
            0x0045, // E
            //0x0046, // F
            //0x0047, // G
            0x0048, // H
            //0x0049, // I
            0x004A, // J
            0x004B, // K
            //0x004C, // L
            0x004D, // M
            0x004E, // N
            //0x004F, // O
            0x0050, // P
            //0x0051, // Q
            0x0052, // R
            0x0053, // S
            0x0054, // T
            0x0055, // U
            0x0056, // V
            0x0057, // W
            0x0058, // X
            0x0059, // Y
            //0x005A, // Z 
            //0x0030, // 0
            //0x0031, // 1
            //0x0032, // 2
            0x0033, // 3
            0x0034, // 4
            0x0035, // 5
            0x0036, // 6
            //0x0037, // 7
            0x0038, // 8
            0x0039  // 9
        };

        // singleton
        private LatinCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_LatinCharacterSet")
        {
        }

        private static readonly LatinCharacterSet _instance = new LatinCharacterSet();

        public static LatinCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
