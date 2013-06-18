using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class FadeIn : Effect, IEffect
    {
        // the fade-in duration, in milliseconds
        public const int DefaultFadeInDuration = 1000;
        private RandomRange _fadeInDurationRange;

        /// <summary>
        /// Randomized fade-in duration, in milliseconds
        /// </summary>
        public RandomRange FadeInDurationRange
        {
            get
            {
                return _fadeInDurationRange;
            }
            set
            {
                _fadeInDurationRange = value;
            }
        }

        /// <summary>
        /// Fade-in duration, in milliseconds
        /// </summary>
        public int FadeInDuration
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _fadeInDurationRange)
                {
                    return DefaultFadeInDuration;
                }

                return _fadeInDurationRange.Next;
            }
            set
            {
                _fadeInDurationRange = new RandomRange(value);
            }
        }


        public override void Apply(Int16[] inputSamples)
        {
            int fadeInSamples = PcmSound.MillisecondToSampleCount(this.FadeInDuration);
            for (int i = 0; i < fadeInSamples; i++)
            {
                // gradually increase the volume, until 100% at fadeInSamples
                inputSamples[i] = (Int16)Math.Round(inputSamples[i] * i / (double) fadeInSamples);
            }
        }
    }
}
