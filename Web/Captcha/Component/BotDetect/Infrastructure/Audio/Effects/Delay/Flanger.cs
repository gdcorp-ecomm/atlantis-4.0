using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Flanger : Effect, IEffect
    {
        // Flanger delay, in milliseconds
        public const int DefaultFlangerDelay = 3; // milliseconds
        private RandomRange _flangerDelayRange;

        /// <summary>
        /// Randomized flanger delay, in milliseconds
        /// </summary>
        public RandomRange FlangerDelayRange
        {
            get
            {
                return _flangerDelayRange;
            }

            set
            {
                _flangerDelayRange = value;
            }
        }

        /// <summary>
        /// Flanger delay, in milliseconds
        /// </summary>
        public int FlangerDelay
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _flangerDelayRange)
                {
                    return DefaultFlangerDelay;
                }

                return _flangerDelayRange.Next;
            }
            set
            {
                _flangerDelayRange = new RandomRange(value);
            }
        }

        // Flanger sweep, in milliseconds
        private RandomRange _flangerSweepRange;
        public const int DefaultFlangerSweep = 2; // milliseconds

        /// <summary>
        /// Randomized flanger sweep, in milliseconds
        /// </summary>
        public RandomRange FlangerSweepRange
        {
            get
            {
                return _flangerSweepRange;
            }

            set
            {
                _flangerSweepRange = value;
            }
        }

        /// <summary>
        /// Flanger sweep, in milliseconds
        /// </summary>
        public int FlangerSweep
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _flangerSweepRange)
                {
                    return DefaultFlangerSweep;
                }

                return _flangerSweepRange.Next;
            }
            set
            {
                _flangerSweepRange = new RandomRange(value);
            }
        }

        // Flanger sweep frequency, in milliseconds
        private RandomRange _flangerSweepFrequencyRange;
        public const int DefaultFlangerSweepFrequency = 1; // hertz

        /// <summary>
        /// Randomized flanger sweep frequency, in milliseconds
        /// </summary>
        public RandomRange FlangerSweepFrequencyRange
        {
            get
            {
                return _flangerSweepFrequencyRange;
            }

            set
            {
                _flangerSweepFrequencyRange = value;
            }
        }

        /// <summary>
        /// Flanger sweep frequency, in milliseconds
        /// </summary>
        public int FlangerSweepFrequency
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _flangerSweepFrequencyRange)
                {
                    return DefaultFlangerSweepFrequency;
                }

                return _flangerSweepFrequencyRange.Next;
            }
            set
            {
                _flangerSweepFrequencyRange = new RandomRange(value);
            }
        }

        // Flanger depth (feedback volume attenuation), as a relative percentage
        public const int DefaultAttenuationPercentage = 50;
        private RandomRange _attenuationPercentageRange;

        /// <summary>
        /// Randomized flanger depth (feedback volume attenuation), as a relative percentage
        /// </summary>
        public RandomRange AttenuationPercentageRange
        {
            get
            {
                return _attenuationPercentageRange;
            }

            set
            {
                _attenuationPercentageRange = value;
            }
        }

        /// <summary>
        /// Flanger depth (feedback volume attenuation), as a relative percentage
        /// </summary>
        public int AttenuationPercentage
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _attenuationPercentageRange)
                {
                    return DefaultAttenuationPercentage;
                }

                return _attenuationPercentageRange.Next;
            }
            set
            {
                _attenuationPercentageRange = new RandomRange(value);
            }
        }

        public int[] InitConstantDelays(int sampleCount)
        {
            // the delay varies with the sweep
            int[] delays = new int[sampleCount];
            int delaySamples = PcmSound.MillisecondToSampleCount(this.FlangerDelay);
            int sweepSamples = PcmSound.MillisecondToSampleCount(this.FlangerSweep);

            // the delay changes with a slow sine wave
            double periodMilliseconds = (1 / (double)this.FlangerSweepFrequency * 1000);
            int periodSamples = PcmSound.MillisecondToSampleCount(periodMilliseconds);
            double sampleToRadianRatio = Math.PI / periodSamples;
            int fullCircle = 2 * periodSamples;

            // calculate periodic delays once, and then apply to all periods 
            for (int i = 0; i < fullCircle; i++)
            {
                double currentRadianEquivalent = i * sampleToRadianRatio;
                int delay = delaySamples + (int)Math.Round((((Math.Sin(currentRadianEquivalent) + 1) / 2)) * sweepSamples);
                for (int j = 0; j < sampleCount - fullCircle; j += fullCircle)
                {
                    delays[i + j] = delay;
                }
            }

            return delays;
        }

        public int[] InitRandomizedDelays(int sampleCount)
        {
            // the delay varies with the sweep
            int[] delays = new int[sampleCount];
            int delaySamples = PcmSound.MillisecondToSampleCount(this.FlangerDelay);
            int sweepSamples = PcmSound.MillisecondToSampleCount(this.FlangerSweep);

            for (int i = 0; i < sampleCount; i++)
            {
                // the delay changes with a slow sine wave
                double periodMilliseconds = (1 / (double)this.FlangerSweepFrequency * 1000);
                int periodSamples = PcmSound.MillisecondToSampleCount(periodMilliseconds);
                double currentRadianEquivalent = i * (Math.PI) / periodSamples;
                delays[i] = delaySamples + (int)Math.Round((((Math.Sin(currentRadianEquivalent) + 1) / 2)) * sweepSamples);
            }

            return delays;
        }

        public override void Apply(Int16[] inputSamples)
        {
            // the samples are mixed with a delayed version of themselves
            int sampleCount = inputSamples.Length;
            Int16[] mixedSamples = new Int16[sampleCount];

            // calculate changing delays
            int[] delays;
            if (_flangerSweepFrequencyRange != null && _flangerSweepFrequencyRange.IsRandomized)
            {
                delays = InitRandomizedDelays(sampleCount);
            }
            else
            {
                delays = InitConstantDelays(sampleCount);
            }

            // mix the original samples with delayed ones, with feedback
            double feedbackRatio = 1.0;
            double[] sampleSums = new double[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                sampleSums[i] = inputSamples[i];
            }

            double maxSample = double.MinValue;
            double minSample = double.MaxValue;

            if (null != _attenuationPercentageRange && _attenuationPercentageRange.IsRandomized)
            {
                while (feedbackRatio > 0.1)
                {
                    feedbackRatio *= this.AttenuationPercentage / (double)100;
                    for (int i = 0; i < sampleCount; i++)
                    {
                        if (i + delays[i] < sampleCount)
                        {
                            sampleSums[i] += feedbackRatio * inputSamples[i + delays[i]];

                            // re-use the loop to find the peak
                            if (sampleSums[i] > maxSample)
                            {
                                maxSample = sampleSums[i];
                            }
                            else if (sampleSums[i] < minSample)
                            {
                                minSample = sampleSums[i];
                            }
                        }
                    }
                }
            }
            else
            {
                double ratioFactor = this.AttenuationPercentage / (double)100;
                while (feedbackRatio > 0.1)
                {
                    feedbackRatio *= ratioFactor;
                    for (int i = 0; i < sampleCount; i++)
                    {
                        if (i + delays[i] < sampleCount)
                        {
                            sampleSums[i] += feedbackRatio * sampleSums[i + delays[i]];

                            // re-use the loop to find the peak
                            if (sampleSums[i] > maxSample)
                            {
                                maxSample = sampleSums[i];
                            }
                            else if (sampleSums[i] < minSample)
                            {
                                minSample = sampleSums[i];
                            }
                        }
                    }
                }
            }

            // adjust all sums to fit into sample length + normalize
            double peakSum = Math.Max(maxSample, -1 * minSample);
            double targetPeak = (((double)(Int16.MaxValue - 1)) * Normalize.DefaultNormalizationPeak / ((double)100));
            double ratio = targetPeak / peakSum;
            for (int i = 0; i < sampleCount; i++)
            {
                mixedSamples[i] = (Int16)(sampleSums[i] * ratio);
            }

            //mixedSamples = Mixer.Merge(mixedSamples, inputSamples);

            // we crop the extra samples resulting from the delay mixing
            List<Int16> mixed = new List<Int16>(mixedSamples);
            mixed.CopyTo(0, inputSamples, 0, sampleCount);
        }
    }
}
