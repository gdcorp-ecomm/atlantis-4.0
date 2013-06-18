using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio.Packaging
{
    internal struct BdspToc
    {
        public UInt32 LookupSectionStart;
        public UInt32 LookupSectionLength;
        public UInt32 DataSectionStart;
        public UInt32 DataSectionLength;
    }
}
