using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect
{
    internal sealed class MathHelper
    {
        private MathHelper()
        {
        }

        public static Int16 Abs(Int16 input)
        {
            // funny egde case workaround to avoid exceptions;
            // the small mathematical inaccuracy is a lesser evil
            // than exceptions in common code paths
            if (-32768 == input)
            {
                return 32767;
            }

            return Math.Abs(input);
        }

        public static Int32 Abs(Int32 input)
        {
            // funny egde case workaround to avoid exceptions;
            // the small mathematical inaccuracy is a lesser evil
            // than exceptions in common code paths
            if (-2147483648 == input)
            {
                return 2147483647;
            }

            return Math.Abs(input);
        }

        /// <summary>
        /// Calculate a RMS of the input values
        /// </summary>
        public static Int16 RootMeanSquare(Int16[] values)
        {
            double squareSum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                squareSum += Math.Pow(values[i], 2);
            }
            double meanSquare = squareSum / values.Length;

            return (Int16)Math.Round(Math.Sqrt(meanSquare));
        }

        public static Int16[] RmsAveraged(Int16[] values, int averagedValuesCount)
        {
            Int16[] averagedValues = values.Clone() as Int16[];
            for (int i = 0; i < values.Length - averagedValuesCount; i++)
            {
                if (i >= averagedValuesCount)
                {
                    Int16[] sample = new Int16[averagedValuesCount];
                    for (int j = 0; j < averagedValuesCount; j++)
                    {
                        sample[j] = values[i - j];
                    }
                    Int16 averaged = MathHelper.RootMeanSquare(sample);
                    for (int j = 0; j < averagedValuesCount; j++)
                    {
                        averagedValues[i - j] = averaged;
                    }
                }
            }

            return averagedValues;
        }

        public static Int16 AverageAmplitude(Int16[] values)
        {
            double sum = 0;

            // sum up the elements, use double to avoid overflows
            for (int i = 0; i < values.Length; i++)
            {
                Int16 currentValue = MathHelper.Abs(values[i]);
                sum += currentValue;
            }

            Int16 average = (Int16)(Math.Round(sum / values.Length));
            return average;
        }

        public static Int16 PeakAmplitude(Int16[] values)
        {
            Int16[] sorted = values.Clone() as Int16[];
            Array.Sort(sorted);
            return Math.Max(MathHelper.Abs(sorted[0]), MathHelper.Abs(sorted[values.Length - 1]));

            //List<Int16> list = new List<Int16>(values);
            //list.Sort();
            //return Math.Max(MathHelper.Abs(list[0]), MathHelper.Abs(list[values.Length - 1]));
        }

        public static Int32 PeakAmplitude(Int32[] values)
        {
            Int32[] sorted = values.Clone() as Int32[];
            Array.Sort<Int32>(sorted);
            return Math.Max(MathHelper.Abs(sorted[0]), MathHelper.Abs(sorted[values.Length - 1]));
        }

        /// <summary>
        /// smart peak - high values that only occur in one sample per file
        /// are not really heard; we're looking for the highest value that
        /// occurs in enough samples to be noticeable
        /// </summary>
        /// <param name="values"></param>
        /// <param name="requiredCount"></param>
        /// <returns></returns>
        /*public static Int16 PeakAmplitude(Int16[] values, int requiredCount)
        {
            Int16 peakValue = 0;

            int[] amplitudeCounts = new int[Int16.MaxValue + 1];

            // look for the absolute peak within the sample, 
            // and count how many times each value appeared
            for (int i = 0; i < values.Length; i++)
            {
                Int16 currentValue = MathHelper.Abs(values[i]);
                amplitudeCounts[currentValue]++;
                if (currentValue > peakValue)
                {
                    peakValue = MathHelper.Abs(currentValue);
                }
            }

            // look for the first value that is high enough and
            // occurs often enough to be hearable
            while (amplitudeCounts[peakValue] < requiredCount && peakValue > 0)
            {
                peakValue--;
            }

            // flatten (average out) higher values, 
            // counting them as statistical errors 
            for (int i = 0; i < values.Length; i++)
            {
                if (MathHelper.Abs(values[i]) > peakValue)
                {
                    Int16 prevValue = 0;
                    if (i > 0)
                    {
                        prevValue = values[i - 1];
                    }

                    Int16 nextValue = 0;
                    if (i < values.Length - 1)
                    {
                        nextValue = values[i + 1];
                    }

                    values[i] = (Int16)(((double)prevValue + nextValue) / 2);
                }
            }

            return peakValue;
        }*/
    }
}
