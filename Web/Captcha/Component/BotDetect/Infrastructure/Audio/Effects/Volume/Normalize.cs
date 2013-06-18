using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Normalize : Effect, IEffect
    {
        public const int DefaultNormalizationPeak = 75;

        private int _peakPercentage = Normalize.DefaultNormalizationPeak;
        public int PeakPercentage
        {
            get
            {
                return _peakPercentage;
            }
            set
            {
                _peakPercentage = value;
            }
        }

        public override void Apply(Int16[] inputSamples)
        {
            // caclulate normalization ratio
            //TODO: Int16 oldPeak = MathHelper.PeakAmplitude(inputSamples, 1);
            Int16 oldPeak = MathHelper.PeakAmplitude(inputSamples);
            if (oldPeak == 0) { return; } // if completely silent (max amplitude = 0), leave as is

            double targetPeak = (((double)(Int16.MaxValue - 1)) * _peakPercentage / ((double)100));
            double ratio = targetPeak / oldPeak;

            // apply normalization ratio
            for (int i = 0; i < inputSamples.Length; i++)
            {
                double newValue = (double)inputSamples[i] * ratio;

                // clipping, for peakPercentage values larger than 100 (overdrive)
                if (newValue > (double)(Int16.MaxValue - 1))
                {
                    inputSamples[i] = Int16.MaxValue - 1;
                }
                else if (newValue < (double)(Int16.MinValue + 1))
                {
                    inputSamples[i] = Int16.MinValue + 1;
                }
                else
                {
                    inputSamples[i] = (Int16)newValue;
                }
            }
        }
    }
}
