using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio.Wav;

namespace BotDetect.Audio
{
    internal class WavPcm16bit8kHzMonoConverter : FormatConverter, IFormatConverter
    {
        public WavPcm16bit8kHzMonoConverter()
        {
        }

        public override byte[] ConvertFromWavPcm16bit8kHzMono(byte[] inputHeaderlessData)
        {
            return inputHeaderlessData;
        }

        public override byte[] ConvertToWavPcm16bit8kHzMono(byte[] inputHeaderlessData)
        {
            return inputHeaderlessData;
        }
    }
}
