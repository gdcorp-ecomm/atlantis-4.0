using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using BotDetect.Audio;
using BotDetect.Audio.Packaging;

namespace BotDetect.Audio
{
    internal class Pronunciation : IPronunciation
    {
        public Pronunciation(Localization localization)
        {
            _localization = localization;

            _soundPackageFilePath = Path.Combine(localization.SoundPackagesFolder, localization.PronunciationFilename);
        }

        private Localization _localization;

        private string _soundPackageFilePath;

        public string SoundPackageFilePath
        {
            get
            {
                return _soundPackageFilePath;
            }
        }

        public bool IsPronunciationAvailable()
        {
            byte[] rawSoundData = SoundPackageFacade.GetRawSoundData(_soundPackageFilePath, "0");

            return (null != rawSoundData && 0 < rawSoundData.Length);
        }

        private bool _hasSettings;

        // pronunciation starting silence, in milliseconds
        private RandomRange _startingSilenceRange;

        public RandomRange StartingSilenceRange
        {
            get
            {
                return _startingSilenceRange;
            }

            set
            {
                _startingSilenceRange = value;
                _hasSettings = true;
            }
        }

        public int StartingSilence
        {
            get
            {
                if (null == _startingSilenceRange)
                {
                    return 0;
                }

                return _startingSilenceRange.Next;
            }
            set
            {
                _startingSilenceRange = new RandomRange(value);
                _hasSettings = true;
            }
        }

        // pronunciation separating silence, in milliseconds
        private RandomRange _separatingSilenceRange;

        public RandomRange SeparatingSilenceRange
        {
            get
            {
                return _separatingSilenceRange;
            }

            set
            {
                _separatingSilenceRange = value;
                _hasSettings = true;
            }
        }

        public int SeparatingSilence
        {
            get
            {
                if (null == _separatingSilenceRange)
                {
                    return 0;
                }

                return _separatingSilenceRange.Next;
            }
            set
            {
                _separatingSilenceRange = new RandomRange(value);
                _hasSettings = true;
            }
        }

        // effect ending silence, in milliseconds
        private RandomRange _endingSilenceRange;

        public RandomRange EndingSilenceRange
        {
            get
            {
                return _endingSilenceRange;
            }

            set
            {
                _endingSilenceRange = value;
                _hasSettings = true;
            }
        }

        public int EndingSilence
        {
            get
            {
                if (null == _endingSilenceRange)
                {
                    return 0;
                }

                return _endingSilenceRange.Next;
            }
            set
            {
                _endingSilenceRange = new RandomRange(value);
                _hasSettings = true;
            }
        }

        public AudioTrack Pronounce(string textToPronounce)
        {
            AudioTrack track;

            if (_hasSettings)
            {
                track = PronounceWithSettings(textToPronounce);
            }
            else
            {
                // by default, we use some reasonable silence settings
                track = DefaultPronounce(textToPronounce);
            }

            return track;
        }

        protected AudioTrack PronounceWithSettings(string textToPronounce)
        {
            AudioTrack track = new AudioTrack();

            // starting padding
            track.Spin(this.StartingSilence);

            int[] separatingSilences = CalculateSilences(textToPronounce.Length);

            // pronunciations
            for (int i = 0; i < textToPronounce.Length; i++)
            {
                char c = textToPronounce[i];
                Int16[] charPronunciation = this.Pronounce(c);
                track.AddData(charPronunciation);

                // spacing
                if (i < textToPronounce.Length - 1)
                {
                    track.Spin(separatingSilences[i]);
                }
            }

            // final padding
            track.Spin(this.EndingSilence);

            return track;
        }

        public int[] CalculateSilences(int count)
        {
            int[] silences = new int[count];

            // if more than N characters are pronuounced very fast, 
            // the pronunciation will be hard to retype; 
            // so we keep track of grouped silence durations and adjust if needed
            int groupSize = 2;
            int minGroupSilence = (groupSize - 1) * Defaults.MinAverageSeparatingSilence;
            int groupSilence = 0;

            for (int i = 0; i < count; i++)
            {
                silences[i] = this.SeparatingSilence;
                groupSilence += silences[i];

                if (0 == i % groupSize)
                {
                    // if the previous N characters have been spoken very fast,
                    // slow down on this char to make up
                    if (groupSilence < minGroupSilence)
                    {
                        silences[i] = minGroupSilence - groupSilence;
                    }
                    groupSilence = 0;
                }
            }

            return silences;
        }

        public AudioTrack TestPronounce(string textToPronounce)
        {
            // fixed values for algorithm testing
            this.StartingSilence = 100;
            this.SeparatingSilence = 100;
            this.EndingSilence = 100;

            AudioTrack pronunciationTrack = this.Pronounce("TEST");

            return pronunciationTrack;
        }

        protected AudioTrack DefaultPronounce(string textToPronounce)
        {
            // the defaults
            this.StartingSilenceRange = Defaults.StartingSilence;
            this.SeparatingSilenceRange = Defaults.SeparatingSilence;
            this.EndingSilenceRange = Defaults.EndingSilence;

            AudioTrack pronunciationTrack = this.Pronounce(textToPronounce);

            return pronunciationTrack;
        }

        /// <summary>
        /// Get the pronunciation of the given character in the specified language
        /// </summary>
        public virtual Int16[] Pronounce(char character)
        {
            byte[] rawSoundData = SoundPackageFacade.GetRawSoundData(_soundPackageFilePath, character.ToString().ToLowerInvariant());
            return PcmSound.GetSamples(rawSoundData);
        }
    }
}
