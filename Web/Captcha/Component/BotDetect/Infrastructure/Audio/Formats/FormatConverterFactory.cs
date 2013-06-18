using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio.Wav;

namespace BotDetect.Audio
{
    internal abstract class FormatConverterFactory
    {
        public static IFormatConverter CreateConverter(SoundFormat format)
        {
            IFormatConverter converter = null;

            switch (format)
            {
                case SoundFormat.WavPcm16bit8kHzMono:
                {
                    converter = new WavPcm16bit8kHzMonoConverter();
                    break;
                }
                case SoundFormat.WavPcm8bit8kHzMono:
                {
                    converter = new WavPcm8bit8kHzMonoConverter();
                    break;
                }
                default:
                {
                    throw new NotImplementedException("SoundFormatConverter not implemented!");
                }
            }

            return converter;
        }
    }
}
