using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio.Wav
{
    internal sealed class Defaults
    {
        private Defaults()
        {
        }

        public const UInt16 FormatTag = 1;             // PCM
        public const UInt16 ChanellCount = 1;          // Mono
        public const UInt32 SamplingRate = 8000;       // 8kHz
        public const UInt32 BitsPerSample = 16;        // 16bit
        public const UInt32 SamplesPerMillisecond = 8;  // SamplingRate / 1000
        public const UInt32 BytesPerMillisecond = 16;   // SamplingRate / 1000 * BitsPerSample / 8
    }
}
