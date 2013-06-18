using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using BotDetect.Audio.Wav;

namespace BotDetect.Audio
{
    internal class ToneNoise : Noise, INoise
    {
        // tone frequency, in Herz
        private RandomRange _frequencyRange;

        public RandomRange FrequencyRange
        {
            get
            {
                return _frequencyRange;
            }

            set
            {
                _frequencyRange = value;
            }
        }

        public int Frequency
        {
            get
            {
                if (null == _frequencyRange)
                {
                    return 0;
                }

                return _frequencyRange.Next;
            }
            set
            {
                _frequencyRange = new RandomRange(value);
            }
        }


        public override Int16[] Generate(int length, int volume)
        {
            int noiseLength = (int)PcmSound.MillisecondToSampleCount(length);
            Int16[] noiseData = new Int16[noiseLength];
            double volumeRatio = volume / (double)100;

            if (null != _frequencyRange && _frequencyRange.IsRandomized)
            {
                for (int i = 0; i < noiseData.Length; i++)
                {
                    // calculate how many Pcm samples are equivalen to one Sine period
                    double periodMilliseconds = (1 / (double)this.Frequency * 1000);
                    int periodSamples = PcmSound.MillisecondToSampleCount(periodMilliseconds);
                    double sampleToRadianRatio = (Math.PI) / periodSamples;

                    // current sample index as a radian equivalent + calculate amplitude
                    double currentRadianEquivalent = i * sampleToRadianRatio;
                    double amplitude = Math.Sin(currentRadianEquivalent) * (Int16.MaxValue - 1);
                    noiseData[i] = (Int16)(amplitude * volumeRatio);
                }
            }
            else
            {
                // calculate how many Pcm samples are equivalen to one Sine period
                double periodMilliseconds = (1 / (double)this.Frequency * 1000);
                int periodSamples = PcmSound.MillisecondToSampleCount(periodMilliseconds);
                double sampleToRadianRatio = (Math.PI) / periodSamples;
                int fullCircle = 2 * periodSamples;

                // non-random frequency, an optimization
                for (int i = 0; i < fullCircle; i++)
                {
                    double currentRadianEquivalent = i * sampleToRadianRatio;
                    double amplitude = Math.Sin(currentRadianEquivalent) * (Int16.MaxValue - 1);
                    Int16 adjustedAmplitude = (Int16)(amplitude * volumeRatio);
                    for (int j = 0; j < noiseData.Length - fullCircle; j += fullCircle)
                    {
                        noiseData[i + j] = adjustedAmplitude;
                    }
                }
            }

            return noiseData;
        }
    }
}
