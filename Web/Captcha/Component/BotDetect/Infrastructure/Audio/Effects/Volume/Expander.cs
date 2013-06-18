using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Expander : Effect, IEffect
    {
        // Threshold amplitude as percentage of peak
        public const int DefaultMinAmplitudePercentage = 20;

        private int _minAmplitudePercentage = DefaultMinAmplitudePercentage;

        /// <summary>
        /// Threshold amplitude as percentage of peak
        /// </summary>
        public int MinAmplitudePercentage
        {
            get
            {
                return _minAmplitudePercentage;
            }
            set
            {
                _minAmplitudePercentage = value;
            }
        }

        // Absolute peak value the threshold is calculated against
        private int _peak = Int16.MaxValue - 1;

        /// <summary>
        /// Absolute peak value the threshold is calculated against
        /// </summary>
        public int Peak
        {
            get
            {
                return _peak;
            }
            set
            {
                _peak = value;
            }
        }

        // how many samples before and after are averaged in threshold calculation
        private int _averagedSamples = 5;

        /// <summary>
        /// How many samples before and after are averaged in threshold calculation
        /// </summary>
        public int AveragedSamples
        {
            get
            {
                return _averagedSamples;
            }
            set
            {
                _averagedSamples = value;
            }
        }

        // Expander volume attenuation, as a relative percentage
        public const int DefaultAttenuationPercentage = 10;
        private RandomRange _attenuationPercentageRange;

        /// <summary>
        /// Randomized expander volume attenuation, as a relative percentage
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
        /// Expander volume gain, as a relative percentage
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

        public override void Apply(Int16[] inputSamples)
        {
            // all values at fragments with RMS < min get attenuated
            Int16 minAmplitude = (Int16)(_peak * _minAmplitudePercentage / (double)100);
            int lookahead = this.AveragedSamples;
            double attenuationFactor = this.AttenuationPercentage / (double)100;

            for (int i = lookahead; i < inputSamples.Length; i += lookahead)
            {
                // calculate RMS of selected fragment
                double squareSum = 0;
                int actualCount = 0;
                for (int j = 0; j < lookahead; j++)
                {
                    int absIndex = i - j;
                    if (absIndex < inputSamples.Length)
                    {
                        squareSum += Math.Pow(inputSamples[absIndex], 2);
                        actualCount++;
                    }
                }
                Int16 averaged = (Int16)Math.Round(Math.Sqrt(squareSum / actualCount));

                // attenuate quiet fragments
                if (averaged < minAmplitude)
                {
                    for (int j = 0; j < lookahead; j++)
                    {
                        int absIndex = i - j;
                        if (absIndex < inputSamples.Length)
                        {
                            inputSamples[absIndex] = (Int16)(inputSamples[absIndex] * attenuationFactor);
                        }
                    }
                }
                else
                {
                    // leave as is
                }
            }

            Normalize normalize = new Normalize();
            normalize.Apply(inputSamples);
        }


        /*public override void Apply(Int16[] inputSamples)
        {
            Int16 minAmplitude = (Int16)(_peak * _minAmplitudePercentage / (double)100);
            Int16[] rmsAveraged = MathHelper.RmsAveraged(inputSamples, this.AveragedSamples);

            for (int i = 0; i < inputSamples.Length - this.AveragedSamples; i++)
            {
                if (rmsAveraged[i] < minAmplitude)
                {
                    inputSamples[i] = (Int16)(inputSamples[i] * this.AttenuationPercentage / (double)100);
                }
                else
                {
                    // leave as is
                }
            }

            Normalize normalize = new Normalize();
            normalize.Apply(inputSamples);
        }*/
    }
}
