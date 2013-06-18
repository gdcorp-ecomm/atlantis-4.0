using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using BotDetect.Audio;

namespace BotDetect.Audio
{
    internal abstract class Noise : INoise
    {
        public const int DefaultVolume = 50;

        private bool _hasSettings = false;

        // noise volume, as percentage of absolute sound peak
        private RandomRange _volumeRange;

        public RandomRange VolumeRange
        {
            get
            {
                return _volumeRange;
            }

            set
            {
                _volumeRange = value;
                _hasSettings = true;
            }
        }

        public int Volume
        {
            get
            {
                if (null == _volumeRange)
                {
                    return DefaultVolume;
                }

                return _volumeRange.Next;
            }
            set
            {
                _volumeRange = new RandomRange(value);
                _hasSettings = true;
            }
        }

        // noise duration, in milliseconds
        private RandomRange _durationRange;

        public RandomRange DurationRange
        {
            get
            {
                return _durationRange;
            }

            set
            {
                _durationRange = value;
                _hasSettings = true;
            }
        }

        public int Duration
        {
            get
            {
                if (null == _durationRange)
                {
                    return 0;
                }

                return _durationRange.Next;
            }
            set
            {
                _durationRange = new RandomRange(value);
                _hasSettings = true;
            }
        }

        // noise startDelay, in milliseconds
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

        // noise separatingDelay, in milliseconds
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


        // noise endDelay, in milliseconds
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

        public AudioTrack Generate(int totalDuration)
        {
            AudioTrack track;

            if (_hasSettings)
            {
                // apply user-set silences/delays
                track = GenerateWithSettings(totalDuration);
            }
            else
            {
                // by default, we fill the whole track with noise
                track = GenerateDefault(totalDuration);
            }
            return track;
        }

        protected AudioTrack GenerateWithSettings(int totalDuration)
        {
            AudioTrack track = new AudioTrack();
            track.Spin(this.StartingSilence);

            while (track.Duration < totalDuration - this.EndingSilence)
            {
                Int16[] noiseSamples = this.Generate(this.Duration, this.Volume);
                track.AddData(noiseSamples);

                track.Spin(this.SeparatingSilence);
            }

            return track;
        }

        protected AudioTrack GenerateDefault(int totalDuration)
        {
            AudioTrack track = new AudioTrack();

            Int16[] noiseSamples = this.Generate(totalDuration, DefaultVolume);
            track.AddData(noiseSamples);

            return track;
        }

        /// <summary>
        /// Get the specified type of noise with the specified parameters
        /// </summary>
        /// <param name="noiseType">the type of noise to generate</param>
        /// <param name="length">the number of milliseconds the noise should last</param>
        /// <param name="volume">how loud should the noise be relative to the peak level - on a scale 0 to 100</param>
        /// <returns></returns>
        public abstract Int16[] Generate(int length, int volume);
    }
}
