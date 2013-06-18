using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Phaser : Effect, IEffect
    {
        // Phaser delay, in milliseconds
        public const int DefaultPhaserDelay = 3; // milliseconds
        private RandomRange _phaserDelayRange;

        /// <summary>
        /// Randomized phaser delay, in milliseconds
        /// </summary>
        public RandomRange PhaserDelayRange
        {
            get
            {
                return _phaserDelayRange;
            }

            set
            {
                _phaserDelayRange = value;
            }
        }

        /// <summary>
        /// Phaser delay, in milliseconds
        /// </summary>
        public int PhaserDelay
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _phaserDelayRange)
                {
                    return DefaultPhaserDelay;
                }

                return _phaserDelayRange.Next;
            }
            set
            {
                _phaserDelayRange = new RandomRange(value);
            }
        }

        // Phaser sweep, in milliseconds
        private RandomRange _phaserSweepRange;
        public const int DefaultPhaserSweep = 32; // milliseconds

        /// <summary>
        /// Randomized phaser sweep, in milliseconds
        /// </summary>
        public RandomRange PhaserSweepRange
        {
            get
            {
                return _phaserSweepRange;
            }

            set
            {
                _phaserSweepRange = value;
            }
        }

        /// <summary>
        /// Phaser sweep, in milliseconds
        /// </summary>
        public int PhaserSweep
        {
            get
            {
                // we always use this method to access the value
                // so it's randomized properly
                if (null == _phaserSweepRange)
                {
                    return DefaultPhaserSweep;
                }

                return _phaserSweepRange.Next;
            }
            set
            {
                _phaserSweepRange = new RandomRange(value);
            }
        }

        // Phaser depth (feedback volume attenuation), as a relative percentage
        public const int DefaultAttenuationPercentage = 50;
        private RandomRange _attenuationPercentageRange;

        /// <summary>
        /// Randomized phaser depth (feedback volume attenuation), as a relative percentage
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
        /// Phaser depth (feedback volume attenuation), as a relative percentage
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

        public int[] InitDelays(int sampleCount)
        {
            // the delay varies with the sweep
            int[] delays = new int[sampleCount];
            int delaySamples = PcmSound.MillisecondToSampleCount(this.PhaserDelay);
            int sweepSamples = PcmSound.MillisecondToSampleCount(this.PhaserSweep);

            int currentSweepSamples = sweepSamples;
            bool ascending = true;
            for (int i = 0; i < sampleCount; i++)
            {
                // the delay changes exponentially

                //TODO: param
                if (i % sweepSamples == 0)
                {
                    if (ascending)
                    {
                        if (currentSweepSamples < sweepSamples)
                        {
                            currentSweepSamples *= 2;
                        }
                        else
                        {
                            if (currentSweepSamples > sweepSamples)
                            {
                                currentSweepSamples = sweepSamples;
                            }
                            ascending = false;
                        }
                    }
                    else
                    {
                        if (currentSweepSamples > 0)
                        {
                            currentSweepSamples /= 2;
                        }
                        else
                        {
                            currentSweepSamples = 1;
                            ascending = true;
                        }
                    }
                }

                delays[i] = delaySamples + currentSweepSamples;
            }

            return delays;
        }

        public override void Apply(Int16[] inputSamples)
        {
            int sampleCount = inputSamples.Length;
            Int16[] mixedSamples = new Int16[sampleCount];

            // calculate changing delays
            int[] delays = InitDelays(sampleCount);

            // the samples are mixed with a delayed version of themselves
            double[] sampleSums = new double[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                sampleSums[i] = inputSamples[i];
            }

            double maxSample = double.MinValue;
            double minSample = double.MaxValue;
            double feedbackRatio = 1.0;

            if (null != _attenuationPercentageRange && _attenuationPercentageRange.IsRandomized)
            {
                while (feedbackRatio > 0.1)
                {
                    feedbackRatio *= this.AttenuationPercentage / (double)100;
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
            else
            {
                double attenuationFactor = this.AttenuationPercentage / (double)100;
                while (feedbackRatio > 0.1)
                {
                    feedbackRatio *= attenuationFactor;
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

            // we crop the extra samples resulting from the delay mixing
            List<Int16> mixed = new List<Int16>(mixedSamples);
            mixed.CopyTo(0, inputSamples, 0, sampleCount);
        }

        /*public override void Apply(Int16[] inputSamples)
        {
            // the samples are mixed with a delayed version of themselves
            Int16[] mixedSamples = inputSamples.Clone() as Int16[];

            // calculate changing delays
            int[] delays = InitDelays(inputSamples.Length);

            // mix the original samples with delayed ones, with feedback
            double feedbackRatio = 1.0;
            while (feedbackRatio > 0.1)
            {
                feedbackRatio *= this.AttenuationPercentage / (double)100;
                Int16[] delayedSamples = new Int16[inputSamples.Length];
                for (int i = 0; i < inputSamples.Length; i++)
                {
                    if (i + delays[i] < inputSamples.Length)
                    {
                        delayedSamples[i] = (Int16) Math.Round(feedbackRatio * mixedSamples[i + delays[i]]);
                    }
                }
                mixedSamples = Mixer.Merge(mixedSamples, delayedSamples);
            }

            mixedSamples = Mixer.Merge(mixedSamples, inputSamples);

            // we crop the extra samples resulting from the delay mixing
            List<Int16> mixed = new List<Int16>(mixedSamples);
            mixed.CopyTo(0, inputSamples, 0, inputSamples.Length);
        }*/
    }
}
