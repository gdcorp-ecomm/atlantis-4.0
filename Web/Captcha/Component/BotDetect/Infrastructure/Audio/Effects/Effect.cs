using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BotDetect.Audio
{
    internal abstract class Effect : IEffect
    {
        private bool _hasSettings;

        // effect duration, in milliseconds
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

        // effect startingDelay, in milliseconds
        private RandomRange _startingDelayRange;

        public RandomRange StartingDelayRange
        {
            get
            {
                return _startingDelayRange;
            }

            set
            {
                _startingDelayRange = value;
                _hasSettings = true;
            }
        }

        public int StartingDelay
        {
            get
            {
                if (null == _startingDelayRange)
                {
                    return 0;
                }

                return _startingDelayRange.Next;
            }
            set
            {
                _startingDelayRange = new RandomRange(value);
                _hasSettings = true;
            }
        }

        // effect separatingDelay, in milliseconds
        private RandomRange _separatingDelayRange;

        public RandomRange SeparatingDelayRange
        {
            get
            {
                return _separatingDelayRange;
            }

            set
            {
                _separatingDelayRange = value;
                _hasSettings = true;
            }
        }

        public int SeparatingDelay
        {
            get
            {
                if (null == _separatingDelayRange)
                {
                    return 0;
                }

                return _separatingDelayRange.Next;
            }
            set
            {
                _separatingDelayRange = new RandomRange(value);
                _hasSettings = true;
            }
        }

        // effect endingDelay, in milliseconds
        private RandomRange _endingDelayRange;

        public RandomRange EndingDelayRange
        {
            get
            {
                return _endingDelayRange;
            }

            set
            {
                _endingDelayRange = value;
                _hasSettings = true;
            }
        }

        public int EndingDelay
        {
            get
            {
                if (null == _endingDelayRange)
                {
                    return 0;
                }

                return _endingDelayRange.Next;
            }
            set
            {
                _endingDelayRange = new RandomRange(value);
                _hasSettings = true;
            }
        }
        

        public void Apply(AudioTrack track)
        {
            if (_hasSettings)
            {
                ApplyWithSettings(track);
            }
            else
            {
                // by default, we apply the effect to the whole track
                ApplyDefault(track);
            }
        }

        protected void ApplyWithSettings(AudioTrack track)
        {
            track.SeekStart();
            int processedDuration = this.StartingDelay;
            track.Spin(processedDuration);
            int endingSamples = track.Count - PcmSound.MillisecondToSampleCount(this.EndingDelay);
            int endingDuration = PcmSound.SampleCountToMilliseconds(endingSamples);

            while (processedDuration < endingDuration)
            {
                int processedSamples = PcmSound.MillisecondToSampleCount(processedDuration);
                int nextBoundary = PcmSound.MillisecondToSampleCount(processedDuration + this.Duration);
                if (nextBoundary > endingSamples)
                {
                    nextBoundary = endingSamples;
                }

                // replace base track samples with processed ones
                Int16[] baseSamples = track[processedSamples, nextBoundary];
                this.Apply(baseSamples);
                track[processedSamples, nextBoundary] = baseSamples;
                processedDuration = PcmSound.SampleCountToMilliseconds(nextBoundary);

                int delay = this.SeparatingDelay;
                track.Spin(delay);
                processedDuration += delay;
            }
        }

        protected void ApplyDefault(AudioTrack track)
        {
            this.Apply(track.Samples);
        }

        /// <summary>
        /// Apply the given sound effect to the given Wav file fragment
        /// </summary>
        /// <param name="inputAudio">Wav fragment to apply the effect to</param>
        /// <param name="effectType">Type of effect to apply to the input sound fragment</param>
        /// <param name="effectParams">Various effect settings - amplitude, delay, etc. - dependinging on the effect</param>
        /// <returns></returns>
        public abstract void Apply(Int16[] inputSamples);
    }
}
