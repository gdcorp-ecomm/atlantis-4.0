using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Tremolo : Effect, IEffect
    {
        public const int DefaultTremoloSpacing = 80; // milliseconds

        // spacing between amplitude adjustments, in milliseconds
        private RandomRange _tremoloSpacingRange;

        /// <summary>
        /// Randomized spacing between amplitude adjustments, in milliseconds
        /// </summary>
        public RandomRange TremoloSpacingRange
        {
            get
            {
                return _tremoloSpacingRange;
            }

            set
            {
                _tremoloSpacingRange = value;
            }
        }

        /// <summary>
        /// Spacing between amplitude adjustments, in milliseconds
        /// </summary>
        public int TremoloSpacing
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _tremoloSpacingRange)
                {
                    return DefaultTremoloSpacing;
                }

                return _tremoloSpacingRange.Next;
            }
            set
            {
                _tremoloSpacingRange = new RandomRange(value);
            }
        }

        public const int DefaultTremoloDuration = 40; // milliseconds

        // amplitude adjustement duration, in milliseconds
        private RandomRange _tremoloDurationRange;

        /// <summary>
        /// Randomized amplitude adjustement duration, in milliseconds
        /// </summary>
        public RandomRange TremoloDurationRange
        {
            get
            {
                return _tremoloDurationRange;
            }

            set
            {
                _tremoloDurationRange = value;
            }
        }

        /// <summary>
        /// Amplitude adjustement duration, in milliseconds
        /// </summary>
        public int TremoloDuration
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _tremoloDurationRange)
                {
                    return DefaultTremoloDuration;
                }

                return _tremoloDurationRange.Next;
            }
            set
            {
                _tremoloDurationRange = new RandomRange(value);
            }
        }

        // relative amplitude adjustment, as percentage of original
        public const int DefaultTremoloGainPercentage = 20; // percent

        // relative amplitude adjustment, as percentage of original
        private RandomRange _tremoloGainPercentageRange;

        /// <summary>
        /// Randomized relative amplitude adjustment, as percentage of original
        /// </summary>
        public RandomRange TremoloGainPercentageRange
        {
            get
            {
                return _tremoloGainPercentageRange;
            }

            set
            {
                _tremoloGainPercentageRange = value;
            }
        }

        /// <summary>
        /// Relative amplitude adjustment, as percentage of original
        /// </summary>
        public int TremoloGainPercentage
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _tremoloGainPercentageRange)
                {
                    return DefaultTremoloGainPercentage;
                }

                return _tremoloGainPercentageRange.Next;
            }
            set
            {
                _tremoloGainPercentageRange = new RandomRange(value);
            }
        }

        public override void Apply(Int16[] inputSamples)
        {
            int adjustedSampleCount = PcmSound.MillisecondToSampleCount(this.TremoloDuration);
            int spacingSampleCount = PcmSound.MillisecondToSampleCount(this.TremoloSpacing);
            for (int i = 0; i < (inputSamples.Length - adjustedSampleCount); i += spacingSampleCount)
            {
                // get the samples to be affected by amplitude adjustment
                Int16[] adjustedSamples = new Int16[adjustedSampleCount];
                for (int j = 0; j < adjustedSampleCount; j++)
                {
                    adjustedSamples[j] = inputSamples[i + j];
                }

                // ajdust selected sample amplitude
                Gain gain = new Gain();
                gain.GainPercentage = this.TremoloGainPercentage;
                gain.Apply(adjustedSamples);

                // copy to original sound
                for (int j = 0; j < adjustedSampleCount; j++)
                {
                    inputSamples[i + j] = adjustedSamples[j];
                }

                // get new parameter values, if randomized 
                adjustedSampleCount = PcmSound.MillisecondToSampleCount(this.TremoloDuration);
                spacingSampleCount = PcmSound.MillisecondToSampleCount(this.TremoloSpacing);
            }
        }
    }
}
