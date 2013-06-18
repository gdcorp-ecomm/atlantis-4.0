using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Chorus : Effect, IEffect
    {
        // Chorus delay, in milliseconds
        public const int DefaultChorusDelay = 40; // milliseconds
        private RandomRange _chorusDelayRange;

        /// <summary>
        /// Randomized chorus delay, in milliseconds
        /// </summary>
        public RandomRange ChorusDelayRange
        {
            get
            {
                return _chorusDelayRange;
            }

            set
            {
                _chorusDelayRange = value;
            }
        }

        /// <summary>
        /// Chorus delay, in milliseconds
        /// </summary>
        public int ChorusDelay
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _chorusDelayRange)
                {
                    return DefaultChorusDelay;
                }

                return _chorusDelayRange.Next;
            }
            set
            {
                _chorusDelayRange = new RandomRange(value);
            }
        }

        // Chorus sweep, in milliseconds
        private RandomRange _chorusSweepRange;
        public const int DefaultChorusSweep = 10; // milliseconds

        /// <summary>
        /// Randomized chorus sweep, in milliseconds
        /// </summary>
        public RandomRange ChorusSweepRange
        {
            get
            {
                return _chorusSweepRange;
            }

            set
            {
                _chorusSweepRange = value;
            }
        }

        /// <summary>
        /// Chorus sweep, in milliseconds
        /// </summary>
        public int ChorusSweep
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _chorusSweepRange)
                {
                    return DefaultChorusSweep;
                }

                return _chorusSweepRange.Next;
            }
            set
            {
                _chorusSweepRange = new RandomRange(value);
            }
        }

        // Chorus sweep frequency, in milliseconds
        private RandomRange _chorusSweepFrequencyRange;
        public const int DefaultChorusSweepFrequency = 1; // hertz

        /// <summary>
        /// Randomized chorus sweep frequency, in milliseconds
        /// </summary>
        public RandomRange ChorusSweepFrequencyRange
        {
            get
            {
                return _chorusSweepFrequencyRange;
            }

            set
            {
                _chorusSweepFrequencyRange = value;
            }
        }

        /// <summary>
        /// Chorus sweep frequency, in milliseconds
        /// </summary>
        public int ChorusSweepFrequency 
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _chorusSweepFrequencyRange)
                {
                    return DefaultChorusSweepFrequency;
                }

                return _chorusSweepFrequencyRange.Next;
            }
            set
            {
                _chorusSweepFrequencyRange = new RandomRange(value);
            }
        }

        public override void Apply(Int16[] inputSamples)
        {
            // the samples are mixed with a delayed version of themselves
            Int16[] delayedSamples = inputSamples.Clone() as Int16[];
            int sampleCount = inputSamples.Length;

            // the delay varies with the sweep
            int delaySamples = PcmSound.MillisecondToSampleCount(this.ChorusDelay);
            int sweepSamples = PcmSound.MillisecondToSampleCount(this.ChorusSweep);
            int maxDelaySamples = delaySamples + sweepSamples;

            if (null !=_chorusSweepFrequencyRange && _chorusSweepFrequencyRange.IsRandomized)
            {
                // changing delay
                for (int i = 0; i < (sampleCount - maxDelaySamples); i++)
                {
                    // the delay changes with a slow sine wave
                    double periodMilliseconds = (1 / (double)this.ChorusSweepFrequency * 1000);
                    int periodSamples = PcmSound.MillisecondToSampleCount(periodMilliseconds);
                    double sampleToRadianRatio = (Math.PI) / periodSamples;
                    double currentRadianEquivalent = i * sampleToRadianRatio;
                    double currentSweep = ((Math.Sin(currentRadianEquivalent) + 1) / 2) * (sweepSamples);
                    int currentDelay = delaySamples + (int)currentSweep;
                    delayedSamples[i] = (Int16)inputSamples[i + currentDelay];
                }
            }
            else
            {
                // non-random frequency, an optimization
                double periodMilliseconds = (1 / (double)this.ChorusSweepFrequency * 1000);
                int periodSamples = PcmSound.MillisecondToSampleCount(periodMilliseconds);
                double sampleToRadianRatio = (Math.PI) / periodSamples;

                // changing delay
                for (int i = 0; i < (sampleCount - maxDelaySamples); i++)
                {
                    // the delay changes with a slow sine wave
                    double currentRadianEquivalent = i * sampleToRadianRatio;
                    double currentSweep = ((Math.Sin(currentRadianEquivalent) + 1) / 2) * (sweepSamples);
                    int currentDelay = delaySamples + (int)currentSweep;
                    delayedSamples[i] = (Int16)inputSamples[i + currentDelay];
                }
            }

            // mix the original samples with delayed ones
            Int16[] mixedSamples = Mixer.Merge(inputSamples, delayedSamples);

            // we crop the extra samples resulting from the delay mixing
            List<Int16> mixed = new List<Int16>(mixedSamples);
            mixed.CopyTo(0, inputSamples, 0, inputSamples.Length);
        }
    }
}
