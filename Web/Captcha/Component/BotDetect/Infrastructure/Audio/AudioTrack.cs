using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BotDetect.Audio;
using BotDetect.Audio.Wav;

namespace BotDetect.Audio
{
    internal class AudioTrack
    {
        private Int16[] _samples;
        public Int16[] Samples
        {
            get
            {
                return _samples;
            }
        }

        public static List<Int16> SampleList(Int16[] samples)
        {
            if (null != samples && 0 < samples.Length)
            {
                return new List<Int16>(samples);
            }

            return new List<Int16>();
        }

        public static Int16[] GetSamples(List<Int16> sampleList)
        {
            Int16[] samples = new Int16[sampleList.Count];
            sampleList.CopyTo(0, samples, 0, sampleList.Count);
            return samples;
        }

        // pointing to the next selected sample
        private int _position;

        public void SeekStart()
        {
            _position = 0;
        }

        public AudioTrack()
        {
        }

        public AudioTrack(AudioTrack track)
        {
            this.AddData(track.Samples);
        }

        // access a single sample by index
        public Int16 this[int index]
        {
            get
            {
                return (Int16)_samples[index];
            }

            set
            {
                _samples[index] = value;
            }
        }

        // access a sample range by boundaries
        public Int16[] this[int start, int end]
        {
            get
            {
                int sampleCount = end - start;
                if (sampleCount <= 0)
                {
                    throw new AudioException("Ending index has to be bigger than the starting one", start, end);
                }
                if (start < 0 || end > _samples.Length)
                {
                    throw new AudioException("Index outside the range of existing samples", start, end, _samples.Length);
                }

                Int16[] samples = new Int16[sampleCount];
                List<Int16> sampleList = new List<Int16>(_samples);
                sampleList.CopyTo(start, samples, 0, sampleCount);

                return samples;
            }

            set
            {
                int sampleCount = end - start;
                if (sampleCount <= 0)
                {
                    throw new AudioException("Ending index has to be bigger than the starting one", start, end);
                }
                if (start < 0 || end > _samples.Length)
                {
                    throw new AudioException("Index outside the range of existing samples", start, end, _samples.Length);
                }

                for (int i = 0; i < sampleCount; i++)
                {
                    _samples[start + i] = value[i];
                }
            }
        }

        public Int16 PeakAmplitude
        {
            get
            {
                return MathHelper.PeakAmplitude(_samples);
                //TODO:return MathHelper.PeakAmplitude(_samples, 4);
            }
        }

        public Int16 AverageAmplitude
        {
            get
            {
                return MathHelper.AverageAmplitude(_samples);
            }
        }

        public byte[] Bytes
        {
            get
            {
                return PcmSound.GetBytes(_samples);
            }
        }

        public int Count
        {
            get
            {
                if (null == _samples)
                {
                    return 0;
                }

                return _samples.Length;
            }
        }

        public int Duration
        {
            get
            {
                return PcmSound.SampleCountToMilliseconds(_samples.Length);
            }
        }

        public void AddData(Int16[] soundData)
        {
            if (_position >= 0)
            {
                // no starting crop pending, just add the data
                List<Int16> sampleList = SampleList(_samples);
                sampleList.AddRange(soundData);
                _position += soundData.Length;
                _samples = GetSamples(sampleList);
            }
            else
            {
                if (MathHelper.Abs(_position) < soundData.Length)
                {
                    // we can crop some samples from the begining of current data
                    int remainingSamples = soundData.Length - MathHelper.Abs(_position);
                    Int16[] croppedSamples = new Int16[remainingSamples];
                    List<Int16> dataList = SampleList(soundData);
                    dataList.CopyTo(MathHelper.Abs(_position), croppedSamples, 0, remainingSamples);

                    // and add the rest
                    _position = 0;
                    AddData(croppedSamples);
                }
                else
                {
                    // we just skip adding the current data and count it as cropped
                    _position += soundData.Length;
                }
            }
        }

        public void Spin(long millisecondsMin, long millisecondsMax)
        {
            long milliseconds = RandomGenerator.Next(millisecondsMin, millisecondsMax);
            this.Spin(milliseconds);
        }

        public void Spin(long milliseconds)
        {
            if (0 < milliseconds)
            {
                this.Forward(milliseconds);
            }
            else
            {
                this.Rewind(-1 * milliseconds);
            }
        }

        public void Rewind(long milliseconds)
        {
            int samplesToMove = (int) PcmSound.MillisecondToSampleCount(milliseconds);
            _position -= samplesToMove;

            if (_position >= 0)
            {
                // we can remove the existing data straight away
                List<Int16> sampleList = SampleList(_samples);

                sampleList.RemoveRange(_position, samplesToMove);

                _samples = GetSamples(sampleList);
            }
            else
            {
                // we remove any existing data and
                // wait until enough data has been added to the track
                _samples = null;
            }
        }

        public void Forward(long milliseconds)
        {
            int samplesToMove = (int)PcmSound.MillisecondToSampleCount(milliseconds);
            _position += samplesToMove;

            // if we go ahead of any existing data, add silence
            int samplesToAdd = _position - this.Count;
            if (samplesToAdd > 0)
            {
                List<Int16> sampleList = SampleList(_samples);

                Int16[] silence = new Int16[samplesToAdd];
                silence.Initialize(); // set to zeros
                sampleList.AddRange(silence);

                _samples = GetSamples(sampleList);
            }
        }

        
    }
}
