using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Gain : Effect, IEffect
    {
        public const int DefaultGainPercentage = 100;

        private int _gainPercentage = Gain.DefaultGainPercentage;
        public int GainPercentage
        {
            get
            {
                return _gainPercentage;
            }
            set
            {
                _gainPercentage = value;
            }
        }

        public override void Apply(Int16[] inputSamples)
        {
            double ratio = _gainPercentage / (double)100;
            double max = (double)(Int16.MaxValue - 1);
            double min = (double)(Int16.MinValue + 1);

            for (int i = 0; i < inputSamples.Length; i++)
            {
                // apply amplitude adjustment
                double newValue = Math.Round(inputSamples[i] * ratio);

                // clipping
                if (newValue > max)
                {
                    inputSamples[i] = Int16.MaxValue - 1;
                }
                else if (newValue < min)
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
