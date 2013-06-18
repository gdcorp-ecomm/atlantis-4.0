using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class FadeOut : Effect, IEffect
    {
        // the fade out duration, in milliseconds
        public const int DefaultFadeOutDuration = 1000;
        private RandomRange _fadeOutDurationRange;

        /// <summary>
        /// Randomized fade out duration, in milliseconds
        /// </summary>
        public RandomRange FadeOutDurationRange
        {
            get
            {
                return _fadeOutDurationRange;
            }
            set
            {
                _fadeOutDurationRange = value;
            }
        }

        /// <summary>
        /// Fade out duration, in milliseconds
        /// </summary>
        public int FadeOutDuration
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _fadeOutDurationRange)
                {
                    return DefaultFadeOutDuration;
                }

                return _fadeOutDurationRange.Next;
            }
            set
            {
                _fadeOutDurationRange = new RandomRange(value);
            }
        }


        public override void Apply(Int16[] inputSamples)
        {
            int fadeOutSamples = PcmSound.MillisecondToSampleCount(this.FadeOutDuration);
            for (int i = 0; i < fadeOutSamples; i++)
            {
                // same as fade-in, but going from the opposite direction
                inputSamples[inputSamples.Length - i -1] = (Int16)Math.Round(inputSamples[inputSamples.Length - i -1] * i / (double)fadeOutSamples);
            }
        }
    }
}
