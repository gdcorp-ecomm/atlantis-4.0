using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using BotDetect.Audio.Wav;
using BotDetect.Audio;

namespace BotDetect.Audio
{
    internal class WhiteNoise : Noise, INoise
    {
        public override Int16[] Generate(int length, int volume)
        {
            int noiseLength = (int)PcmSound.MillisecondToSampleCount(length);
            Int16[] noiseData = RandomGenerator.NextInt16(noiseLength);

            Gain gain = new Gain();
            gain.GainPercentage = volume;
            gain.Apply(noiseData);

            return noiseData;
        }
    }
}
