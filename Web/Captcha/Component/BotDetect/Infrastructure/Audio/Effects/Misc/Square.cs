using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Square : Effect, IEffect
    {
        private int _peakPercentage = Normalize.DefaultNormalizationPeak;
        public int PeakPercentage
        {
            get
            {
                return _peakPercentage;
            }
            set
            {
                _peakPercentage = value;
            }
        }

        private int _peak = Int16.MaxValue - 1;
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

        public override void Apply(Int16[] inputSamples)
        {
            int outputLength = inputSamples.Length;
            Int32[] sampleSquares = new Int32[outputLength];

            // square all samples
            for (int i = 0; i < outputLength; i++)
            {
                int sign = Math.Sign(inputSamples[i]);
                sampleSquares[i] = (Int32)(sign * inputSamples[i] * inputSamples[i]);
            }

            // adjust all squares to fit into 16bit sound
            Int32 peakSquare = MathHelper.PeakAmplitude(sampleSquares);
            double ratio = _peak / (double)peakSquare;
            for (int i = 0; i < outputLength; i++)
            {
                inputSamples[i] = (Int16)(sampleSquares[i] * ratio);
            }

            Normalize normalize = new Normalize();
            normalize.PeakPercentage = _peakPercentage;
            normalize.Apply(inputSamples);

        }
    }
}
