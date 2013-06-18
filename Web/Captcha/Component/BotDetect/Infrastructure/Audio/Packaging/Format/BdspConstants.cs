using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio.Packaging
{
    internal class BdspConstants
    {
        private BdspConstants()
        {
        }

        public const int NumberLength = 4;
        public const int FormatNameLength = 64;
        public const int FormatVersionLength = 16;
        public const int TocLength = 16;
        public const int FormatMarkSize = 2;
        public const int DataHashSize = 20;
        public const int MinStringLength = 5;
        public const int MinHeaderLength = 106;
        public const int MinLookupLength = 13;
    }
}
