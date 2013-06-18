using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Delay : Effect, IEffect
    {
        public const int DefaultDelay = 250; // milliseconds

        // delay in milliseconds
        private RandomRange _delayRange;

        /// <summary>
        /// Randomized delay in milliseconds
        /// </summary>
        public RandomRange DelayRange
        {
            get
            {
                return _delayRange;
            }

            set
            {
                _delayRange = value;
            }
        }

        /// <summary>
        /// Delay in milliseconds
        /// </summary>
        public int DelayMilliseconds
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _delayRange)
                {
                    return DefaultDelay;
                }

                return _delayRange.Next;
            }
            set
            {
                _delayRange = new RandomRange(value);
            }
        }

        public override void Apply(Int16[] inputSamples)
        {
            //  a delayed version of the samples
            if (this.DelayRange.IsRandomized)
            {
                for (int i = inputSamples.Length - 1; i > 0; i--)
                {
                    int sampleDelay = PcmSound.MillisecondToSampleCount(this.DelayMilliseconds);
                    if (i - sampleDelay >= 0)
                    {
                        inputSamples[i] = inputSamples[i - sampleDelay];
                    }
                }
            }
            else
            {
                int sampleDelay = PcmSound.MillisecondToSampleCount(this.DelayMilliseconds);
                for (int i = inputSamples.Length - 1; i > 0; i--)
                {
                    if (i - sampleDelay >= 0)
                    {
                        inputSamples[i] = inputSamples[i - sampleDelay];
                    }
                }
            }
        }
    }
}