using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio.Wav;

namespace BotDetect.Audio
{
    internal class WavPcm8bit8kHzMonoConverter : FormatConverter, IFormatConverter
    {
        public WavPcm8bit8kHzMonoConverter()
        {
    
        }

        public override byte[] ConvertFromWavPcm16bit8kHzMono(byte[] inputHeaderlessData)
        {
            // downsample data, dropping 8 least significant bits of data 
            Int16[] originalSamples = PcmSound.GetSamples(inputHeaderlessData);
            byte[] newSamples = new byte[originalSamples.Length];

            for (int i = 0; i < originalSamples.Length; i++)
            {
                // discard least significant 8 bits, and flip the sign bit on the remaining 8
                newSamples[i] = (byte)((originalSamples[i] >> 8) ^ 0x80); // 0x80 = 0b1000000
            }

            return newSamples;
        }

        public override byte[] ConvertToWavPcm16bit8kHzMono(byte[] inputHeaderlessData)
        {
            // "upsample" data, adding empty 8 least significant bits of data 
            byte[] originalSamples = inputHeaderlessData;
            Int16[] newSamples = new Int16[originalSamples.Length];

            for (int i = 0; i < originalSamples.Length; i++)
            {
                byte[] bTemp = new byte[2];
                bTemp[1] = (byte) (originalSamples[i] ^ 0x80); // flip the sign bit
                bTemp[0] = 0x00; // append zeroes as least significant 8 bits
                Int16 newSample = BitConverter.ToInt16(bTemp, 0);
                newSamples[i] = newSample;
            }

            return PcmSound.GetBytes(newSamples);
        }
    }
}
