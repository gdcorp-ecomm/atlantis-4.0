using System;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;

namespace BotDetect.CaptchaSound
{
    internal abstract class SoundGenerator : ISoundGenerator
    {
        private int _volume = Defaults.Volume;
        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (0 >= value || 100 < value)
                {
                    throw new CaptchaSoundGenerationException("Sound volume must be between 1 and 100", value);
                }
                _volume = value;
            }
        }

        internal AudioTrack track;

        protected string code;
        protected Localization localization;

        protected virtual void GeneratePronunciation()
        {
            // pronunciation
            IPronunciation pronouncer = PronunciationFactory.Get(localization);
            track = pronouncer.Pronounce(code);
        }

        public virtual IAudio GenerateSound(string code, Localization localization)
        {
            this.code = code;
            this.localization = localization;

            GeneratePronunciation();
            GenerateNoiseAndEffects();

            return new PcmSound(track);
        }

        protected virtual void GenerateNoiseAndEffects()
        {
            // default volume normalization
            Normalize normalize = new Normalize();
            normalize.PeakPercentage = _volume;
            normalize.Apply(track);
        }
    }
}
