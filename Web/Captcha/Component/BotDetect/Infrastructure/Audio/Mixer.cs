using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;
using BotDetect.Audio.Wav;

namespace BotDetect.Audio
{
    internal sealed class Mixer
    {
        private Mixer()
        {
        }

        /// <summary>
        /// Mix two AudioTracks together as equally loud
        /// </summary>
        public static AudioTrack Merge(AudioTrack track1, AudioTrack track2)
        {
            AudioTrack output = new AudioTrack();
            Int16[] mixedSamples = Merge(track1.Samples, track2.Samples);
            output.AddData(mixedSamples);

            return output;
        }

        /// <summary>
        /// Mix two Pcm sample arrays together as equally loud
        /// </summary>
        public static Int16[] Merge(Int16[] track1Samples, Int16[] track2Samples)
        {
            return Merge(track1Samples, track2Samples, 1.0);
        }

        /// <summary>
        /// Mix two AudioTracks together, adjusting the volume of the second one
        /// by gainFactor
        /// </summary>
        public static AudioTrack Merge(AudioTrack track1, AudioTrack track2, double gainFactor)
        {
            AudioTrack output = new AudioTrack();
            Int16[] mixedSamples = Merge(track1.Samples, track2.Samples, gainFactor);
            output.AddData(mixedSamples);

            return output;
        }

        /// <summary>
        /// Mix two Pcm sample arrays together, adjusting the volume of the second one
        /// by gainFactor times 
        /// </summary>
        public static Int16[] Merge(Int16[] track1Samples, Int16[] track2Samples, double gainFactor)
        {
            int outputLength = Math.Max(track1Samples.Length, track2Samples.Length);

            Int32[] sampleSums = new Int32[outputLength];
            Int16[] mixedSamples = new Int16[outputLength];

            Int32 minSample = Int32.MaxValue;
            Int32 maxSample = Int32.MinValue;

            // sum all samples, where they exist
            for (int i = 0; i < outputLength; i++)
            {
                Int32 sample1 = 0;
                if (i < track1Samples.Length)
                {
                    sample1 = (Int32)track1Samples[i];
                }

                Int32 sample2 = 0;
                if (i < track2Samples.Length)
                {
                    sample2 = (Int32)Math.Round(track2Samples[i] * gainFactor);
                }

                Int32 sampleSum = sample1 + sample2;
                if (sampleSum > maxSample)
                {
                    maxSample = sampleSum;
                }
                else if (sampleSum < minSample)
                {
                    minSample = sampleSum;
                }

                sampleSums[i] = sampleSum;
            }

            // adjust all sums to fit into sample length + normalized
            Int32 peakSum = Math.Max(maxSample, -1 * minSample);
            double targetPeak = (((double)(Int16.MaxValue - 1)) * Normalize.DefaultNormalizationPeak / ((double)100));
            double ratio = (targetPeak) / (double)peakSum;
            for (int i = 0; i < outputLength; i++)
            {
                mixedSamples[i] = (Int16)(sampleSums[i] * ratio);
            }

            return mixedSamples;
        }


        /// <summary>
        /// Mix two AudioTracks together, limiting the volume of the first one
        /// according to the volume level of the second one
        /// </summary>
        public static AudioTrack CrossLimit(AudioTrack track1, AudioTrack track2, double gainFactor)
        {
            AudioTrack output = new AudioTrack();
            Int16[] mixedSamples = CrossLimit(track1.Samples, track2.Samples, gainFactor);
            output.AddData(mixedSamples);

            return output;
        }

        /// <summary>
        /// Mix two Pcm sample arrays together, limiting the volume of the second one
        /// according to the volume level of the first one
        /// </summary>
        public static Int16[] CrossLimit(Int16[] track1Samples, Int16[] track2Samples, double gainFactor)
        {
            int outputLength = Math.Max(track1Samples.Length, track2Samples.Length);

            Int32[] sampleSums = new Int32[outputLength];
            Int16[] mixedSamples = new Int16[outputLength];

            Int32 minSample = Int32.MaxValue;
            Int32 maxSample = Int32.MinValue;

            Int16[] rmsAveraged1 = MathHelper.RmsAveraged(track1Samples, 5);
            Int16[] rmsAveraged2 = MathHelper.RmsAveraged(track2Samples, 5);

            // sum all samples, where they exist
            for (int i = 0; i < outputLength; i++)
            {
                Int16 limit = 0;
                Int32 sample1 = 0;
                if (i < track1Samples.Length)
                {
                    limit = (Int16) ((Int16.MaxValue - 1) - rmsAveraged1[i]);
                    sample1 = (Int32)track1Samples[i];
                }

                Int32 sample2 = 0;
                if (i < track2Samples.Length)
                {
                    if (rmsAveraged2[i] > limit)
                    {
                        sample2 = (Int32)Math.Round(track2Samples[i] * gainFactor);
                    }
                    else
                    {
                        sample2 = (Int32)track2Samples[i];
                    }
                }

                Int32 sampleSum = sample1 + sample2;
                if (sampleSum > maxSample)
                {
                    maxSample = sampleSum;
                }
                else if (sampleSum < minSample)
                {
                    minSample = sampleSum;
                }

                sampleSums[i] = sampleSum;
            }

            // adjust all sums to fit into sample length + normalize
            Int32 peakSum = Math.Max(maxSample, -1 * minSample);//MathHelper.PeakAmplitude(sampleSums);
            double targetPeak = (((double)(Int16.MaxValue - 1)) * Normalize.DefaultNormalizationPeak / ((double)100));
            double ratio = (targetPeak) / (double)peakSum;
            for (int i = 0; i < outputLength; i++)
            {
                mixedSamples[i] = (Int16)(sampleSums[i] * ratio);
            }

            return mixedSamples;
        }

        /// <summary>
        /// Mix two AudioTracks together, multiplying invidividual values and producing
        /// a signal containing the sum and differences of input frequencies
        /// </summary>
        public static AudioTrack RingModulate(AudioTrack track1, AudioTrack track2)
        {
            AudioTrack output = new AudioTrack();
            Int16[] mixedSamples = RingModulate(track1.Samples, track2.Samples);
            output.AddData(mixedSamples);

            return output;
        }

        /// <summary>
        /// Mix two Pcm sample arrays together, multiplying invidividual values and producing
        /// a signal containing the sum and differences of input frequencies
        /// </summary>
        public static Int16[] RingModulate(Int16[] track1Samples, Int16[] track2Samples)
        {
            int outputLength = Math.Max(track1Samples.Length, track2Samples.Length);

            Int32[] sampleProducts = new Int32[outputLength];
            Int16[] mixedSamples = new Int16[outputLength];

            Int32 minSample = Int32.MaxValue;
            Int32 maxSample = Int32.MinValue;

            // sum all samples, where they exist
            for (int i = 0; i < outputLength; i++)
            {
                Int32 sample1 = 0;
                if (i < track1Samples.Length)
                {
                    sample1 = (Int32)track1Samples[i];
                }

                Int32 sample2 = 0;
                if (i < track2Samples.Length)
                {
                    sample2 = (Int32)track2Samples[i];
                }

                Int32 sampleProduct = sample1 * sample2;
                if (sampleProduct > maxSample)
                {
                    maxSample = sampleProduct;
                }
                else if (sampleProduct < minSample)
                {
                    minSample = sampleProduct;
                }

                sampleProducts[i] = sampleProduct;
            }

            // adjust all sums to fit into sample length + normalize
            Int32 peakProduct = Math.Max(maxSample, -1 * minSample);
            double targetPeak = (((double)(Int16.MaxValue - 1)) * 99 / ((double)100));
            double ratio = (targetPeak) / (double)peakProduct;
            for (int i = 0; i < outputLength; i++)
            {
                mixedSamples[i] = (Int16)(sampleProducts[i] * ratio);
            }

            return mixedSamples;
        }
    }
}
