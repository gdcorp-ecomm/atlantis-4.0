using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

using BotDetect.Audio;
using BotDetect.Audio.Wav;

namespace BotDetect.Audio
{
    internal class PcmSound : Audio, IAudio
    {
        // Wav sound header
        private Wav.Header _header;
        public Wav.Header Header
        {
            get
            {
                return _header;
            }
        }

        // sound binary data
        public byte[] HeaderlessData
        {
            get
            {
                return PcmSound.GetBytes(_samples);
            }
        }

        // PCM samples
        private Int16[] _samples;
        public Int16[] Samples
        {
            get
            {
                return _samples;
            }
        }

        public PcmSound()
        {
            // empty constructor is allowed
        }

        public PcmSound(byte[] bRawData)
        {
            // if other parts of code handle the actual file reading etc.
            ParseWavPcm(bRawData);
        }

        public PcmSound(byte[] bHeaderlessData, SoundFormat format)
        {
            if (SoundFormatFamily.WavPcm != SoundFormatHelper.GetFamily(format))
            {
                throw new AudioException("The given format is not of the WavPcm format family: {0}", format);
            }

            this.format = format;
            _header = new Wav.Header(bHeaderlessData, format);
            _samples = PcmSound.GetSamples(bHeaderlessData);
        }

        public PcmSound(AudioTrack track)
        {
            format = SoundFormat.WavPcm16bit8kHzMono;
            _samples = track.Samples;
            _header = new Wav.Header(PcmSound.GetBytes(_samples), format);
        }

        public void Open(FileInfo info)
        {
            // check file extension
            if (!(".wav" == info.Extension.ToLowerInvariant()))
            {
                return;
            }

            // read the file
            using (FileStream input = info.OpenRead())
            {
                byte[] bRawData = new byte[input.Length];
                if (input.Read(bRawData, 0, (int)input.Length) == input.Length)
                {
                    ParseWavPcmAndNormalize(bRawData);
                }
            }
        }

        private void ParseWavPcm(byte[] bRawData)
        {
            int[] dataChunkPosition = FindDataChunk(bRawData);
            int dataOffset = (int)dataChunkPosition[0];
            int dataLength = (int)dataChunkPosition[1];

            // initialize header
            byte[] bRawHeader = new byte[dataOffset];
            (new List<byte>(bRawData)).CopyTo(0, bRawHeader, 0, dataOffset);
            _header = new Wav.Header(bRawHeader);

            // determine format
            if (_header.FormatChunk.WaveFormat.BitsPerSample == 16 &&
                _header.FormatChunk.WaveFormat.SamplesPerSec == 8000 &&
                _header.FormatChunk.WaveFormat.Channels == 1)
            {
                format = SoundFormat.WavPcm16bit8kHzMono;
            }
            else if (_header.FormatChunk.WaveFormat.BitsPerSample == 8 &&
                _header.FormatChunk.WaveFormat.SamplesPerSec == 8000 &&
                _header.FormatChunk.WaveFormat.Channels == 1)
            {
                format = SoundFormat.WavPcm8bit8kHzMono;
            }
            else
            {
                format = SoundFormat.Unknown;
            }

            // parse sound data into PCM samples
            byte[] bHeaderlessData = new byte[dataLength];
            (new List<byte>(bRawData)).CopyTo(dataOffset, bHeaderlessData, 0, dataLength);

            _samples = PcmSound.GetSamples(bHeaderlessData);
        }

        private void ParseWavPcmAndNormalize(byte[] bRawData)
        {
            int[] dataChunkPosition = FindDataChunk(bRawData);
            int dataOffset = (int)dataChunkPosition[0];
            int dataLength = (int)dataChunkPosition[1];

            // initialize header
            byte[] bRawHeader = new byte[dataOffset];
            (new List<byte>(bRawData)).CopyTo(0, bRawHeader, 0, dataOffset);
            _header = new Wav.Header(bRawHeader);

            // determine format
            if (_header.FormatChunk.WaveFormat.BitsPerSample == 16 &&
                _header.FormatChunk.WaveFormat.SamplesPerSec == 8000 &&
                _header.FormatChunk.WaveFormat.Channels == 1)
            {
                format = SoundFormat.WavPcm16bit8kHzMono;
            }
            else if (_header.FormatChunk.WaveFormat.BitsPerSample == 8 &&
                _header.FormatChunk.WaveFormat.SamplesPerSec == 8000 &&
                _header.FormatChunk.WaveFormat.Channels == 1)
            {
                format = SoundFormat.WavPcm8bit8kHzMono;
            }
            else
            {
                format = SoundFormat.Unknown;
            }

            // parse sound data into PCM samples
            byte[] bHeaderlessData = new byte[dataLength];
            (new List<byte>(bRawData)).CopyTo(dataOffset, bHeaderlessData, 0, dataLength);

            _samples = PcmSound.GetNormalizedSamples(bHeaderlessData);
        }


        /// <summary>
        /// searches for the Data chunk in a given Wav buffer
        /// </summary>
        /// <returns>result[0] - sound data offset in the buffer;
        /// result[1] - length of sound data (in bytes)
        /// </returns>
        public static int[] FindDataChunk(byte[] wavBuffer)
        {
            int[] dataPosition = new int[2];

            //search for "data" chunk by name and get data size            
            for (int i = 0; i < wavBuffer.Length; i++)
            {
                if ((wavBuffer[i] == 0x64) && (wavBuffer[i + 1] == 0x61)
                    && (wavBuffer[i + 2] == 0x74) && (wavBuffer[i + 3] == 0x61))
                {
                    dataPosition[0] = (i + 8);
                    dataPosition[1] = BitConverter.ToInt32(wavBuffer, i + 4);
                    break;
                }
            }
            if (dataPosition[1] == 0)
            {
                throw new AudioException("Wave file contains no data");
            }
            return dataPosition;
        }

        public void Save(FileInfo info, SoundFormat format)
        {
            IAudio outputSound;

            // create a .wav file at the given path
            if (format != this.format)
            {
                IFormatConverter converter = FormatConverterFactory.CreateConverter(this.format);
                outputSound = converter.Convert(this, format);
            }
            else
            {
                outputSound = this;
            }

            using (FileStream output = info.OpenWrite())
            {
                output.Write(outputSound.Bytes, 0, outputSound.Bytes.Length);
                //Debug.Assert(info.Length == outputSound.Bytes.Length);
            }
        }

        public byte[] Bytes
        {
            get
            {
                byte[] bHeaderlessData = PcmSound.GetBytes(_samples);
                byte[] sound = new byte[bHeaderlessData.Length + _header.Bytes.Length];
                _header.Bytes.CopyTo(sound, 0);
                bHeaderlessData.CopyTo(sound, _header.Bytes.Length);

                return sound;
            }
        }

        public bool IsValidSoundFile(FileInfo info)
        {
            // check file extension
            if (".wav" != info.Extension.ToLowerInvariant())
            {
                return false;
            }

            // check can the file be read
            try
            {
                using (FileStream input = info.OpenRead())
                {
                    // only the first 12 bytes are used to validate the file for now
                    byte[] bRawData = new byte[12];
                    if (input.Read(bRawData, 0, 12) == 12)
                    {
                        return IsWavPcm(bRawData);
                    }
                }
            }
            catch (Exception ex) 
			{
                // ignore file access errors and mark the file as invalid 
				Debug.Assert(false, ex.Message);
			}

            return false;
        }

        public static bool IsWavPcm(byte[] buffer)
        {
            // a simple sanity check, not a comprehensive validity check

            if ("RIFF" != Encoding.ASCII.GetString(buffer, 0, 4))
            {
                return false;
            }

            if ("WAVE" != Encoding.ASCII.GetString(buffer, 8, 4))
            {
                return false;
            }

            return true;
        }

        // get values from a 16bit buffer
        public static Int16[] GetSamples(byte[] bytes)
        {
            Int16[] samples = new Int16[bytes.Length / sizeof(Int16)];
            Buffer.BlockCopy(bytes, 0, samples, 0, bytes.Length);

            return samples;
        }

        // get values from a 16bit buffer
        public static Int16[] GetNormalizedSamples(byte[] bytes)
        {
            Int16[] samples = new Int16[bytes.Length / sizeof(Int16)];
            Buffer.BlockCopy(bytes, 0, samples, 0, bytes.Length);

            Normalize normalize = new Normalize();
            normalize.PeakPercentage = 98;
            normalize.Apply(samples);

            return samples;
        }

        // convert 16bit values to a byte buffer
        public static byte[] GetBytes(Int16[] samples)
        {
            byte[] bytes = new byte[samples.Length * sizeof(Int16)];
            Buffer.BlockCopy(samples, 0, bytes, 0, bytes.Length);

            return bytes;
        }

        public static int MillisecondToSampleCount(double duration)
        {
            return (int)(Math.Round(Wav.Defaults.SamplesPerMillisecond * duration));
        }

        public static int MillisecondToSampleCount(long duration)
        {
            return (int)(Wav.Defaults.SamplesPerMillisecond * duration);
        }

        public static int MillisecondToSampleCount(int duration)
        {
            return (int) (Wav.Defaults.SamplesPerMillisecond * duration);
        }

        public static int SampleCountToMilliseconds(int sampleCount)
        {
            return (int)Math.Round(sampleCount / (double)Wav.Defaults.SamplesPerMillisecond);
        }

        public static long ByteCountToMilliseconds(long byteCount, SoundFormat format)
        {
            if (SoundFormatFamily.WavPcm != SoundFormatHelper.GetFamily(format))
            {
                throw new ArgumentException("Only Wav PCM sounds are allowed");
            }

            UInt16 channelCount;
            UInt32 samplingRate;
            UInt16 bitsPerSample;

            switch (format)
            {
                case SoundFormat.WavPcm16bit8kHzMono:
                    channelCount = 1;
                    samplingRate = 8000;
                    bitsPerSample = 16;
                    break;

                case SoundFormat.WavPcm8bit8kHzMono:
                    channelCount = 1;
                    samplingRate = 8000;
                    bitsPerSample = 8;
                    break;

                default:
                    throw new NotImplementedException("Unknown audio format");
            }

            long bytePerSecond = channelCount * samplingRate * bitsPerSample / 8;

            return byteCount * 1000 / bytePerSecond;
        }

        public static long MillisecondToByteCount(long duration, SoundFormat format)
        {
            if (SoundFormatFamily.WavPcm != SoundFormatHelper.GetFamily(format))
            {
                throw new ArgumentException("Only Wav PCM sounds are allowed");
            }

            UInt16 channelCount;
            UInt32 samplingRate;
            UInt16 bitsPerSample;

            switch (format)
            {
                case SoundFormat.WavPcm16bit8kHzMono:
                    channelCount = 1;
                    samplingRate = 8000;
                    bitsPerSample = 16;
                    break;

                case SoundFormat.WavPcm8bit8kHzMono:
                    channelCount = 1;
                    samplingRate = 8000;
                    bitsPerSample = 8;
                    break;

                default:
                    throw new NotImplementedException("Unknown audio format");
            }

            long bytePerSecond = channelCount * samplingRate * bitsPerSample / 8;

            return duration * (bytePerSecond / 1000);
        }

        // format conversion indexer
        public byte[] this[SoundFormat format]
        {
            get
            {
                if (format == this.format)
                {
                    return this.Bytes;
                }
                else
                {
                    // convert raw data
                    IFormatConverter converter = FormatConverterFactory.CreateConverter(format);
                    byte[] bHeaderlessData = PcmSound.GetBytes(_samples);
                    byte[] bConvertedData = converter.ConvertFromWavPcm16bit8kHzMono(bHeaderlessData);

                    // construct appropriate header
                    Header convertedHeader = new Header(bConvertedData, format);
                    byte[] bConvertedHeader = convertedHeader.Bytes;

                    // raw converted sound
                    byte[] bConvertedSound = new byte[bConvertedData.Length + bConvertedHeader.Length];
                    bConvertedHeader.CopyTo(bConvertedSound, 0);
                    bConvertedData.CopyTo(bConvertedSound, bConvertedHeader.Length);

                    return bConvertedSound;
                }
            }
        }

        public MemoryStream GetStream(SoundFormat format)
        {
            MemoryStream soundStream = new MemoryStream();
            byte[] bSound = this[format];
            soundStream.Write(bSound, 0, bSound.Length);
            return soundStream;
        }
    }
}
