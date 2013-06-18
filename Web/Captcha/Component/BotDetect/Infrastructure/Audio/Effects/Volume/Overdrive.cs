using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio
{
    internal class Overdrive : Effect, IEffect
    {
        public const int DefaultLevel = 10;
        private int _level = Overdrive.DefaultLevel;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        public const int DefaultAjdustedVolume = 25;
        private int _adjustedVolume = Overdrive.DefaultAjdustedVolume;
        public int AdjustedVolume
        {
            get
            {
                return _adjustedVolume;
            }
            set
            {
                _adjustedVolume = value;
            }
        }

        public override void Apply(Int16[] inputSamples)
        {
            // 10 -> 1.000, 50 -> 25.000, 100 -> 100.000
            int peakPercentage = (int)Math.Pow(_level, 2) * 10;

            // overdrive
            Normalize normalize = new Normalize();
            normalize.PeakPercentage = peakPercentage;
            normalize.Apply(inputSamples);

            // volume adjustment
            normalize.PeakPercentage = _adjustedVolume;
            normalize.Apply(inputSamples);
        }
    }
}
