using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BotDetect.Audio.Wav
{
    internal class Header 
    {
        public RiffHeader RiffHeader;          // byte[12]
        public FormatChunk FormatChunk;        // byte[24]
        public DataChunk DataChunk;            // byte[8]

        public Header(byte[] headerlessData, SoundFormat format)
        {
            // used for constructing a new header for known headerless data

            UInt16 formatTag;
            UInt16 channelCount; 
            UInt32 samplingRate;
            UInt16 bitsPerSample;

            switch (format)
            {
                case SoundFormat.WavPcm16bit8kHzMono:
                    formatTag = 1;          // Pcm
                    channelCount = 1;       // Mono
                    samplingRate = 8000;    // 8kHz
                    bitsPerSample = 16;     // 16 bit
                    break;

                case SoundFormat.WavPcm8bit8kHzMono:
                    formatTag = 1;          // Pcm
                    channelCount = 1;       // Mono
                    samplingRate = 8000;    // 8kHz
                    bitsPerSample = 8;      // 8 bit
                    break;

                default:
                    throw new NotImplementedException("Unknown audio format");
            }

            Initialize(headerlessData, formatTag, channelCount, samplingRate, bitsPerSample);
        }

        private void Initialize(byte[] headerlessData, UInt16 formatTag, UInt16 channelCount, UInt32 samplingRate, UInt16 bitsPerSample)
        {
            // used for constructing a new header for known headerless data

            RiffHeader.RiffId = "RIFF";
            RiffHeader.RiffFormat = "WAVE";
            RiffHeader.RiffSize = 36 + Convert.ToUInt32(headerlessData.Length);

            FormatChunk.FormatId = "fmt ";
            FormatChunk.FormatSize = 16;
            FormatChunk.WaveFormat.FormatTag = formatTag; 
            FormatChunk.WaveFormat.Channels = channelCount;
            FormatChunk.WaveFormat.SamplesPerSec = samplingRate;
            FormatChunk.WaveFormat.BitsPerSample = bitsPerSample;

            UInt16 bytesPerSample = Convert.ToUInt16(bitsPerSample / 8);

            FormatChunk.WaveFormat.BlockAlign =
                Convert.ToUInt16(channelCount * bytesPerSample);

            FormatChunk.WaveFormat.AvgBytesPerSec =
                samplingRate * FormatChunk.WaveFormat.BlockAlign;

            DataChunk.DataId = "data";
            DataChunk.DataSize = Convert.ToUInt32(headerlessData.Length);
        }

        public Header(byte[] rawHeaderData)
        {
            // used for parsing an unknown header

            RiffHeader = new RiffHeader(rawHeaderData);
            FormatChunk = new FormatChunk(rawHeaderData);
            DataChunk = new DataChunk(rawHeaderData);
        }

        public byte[] Bytes
        {
            get
            {
                byte[] bWavHeader = new byte[44];
                int writtenDataSize = 0;

                // riff header
                RiffHeader.Bytes.CopyTo(bWavHeader, 0);
                writtenDataSize += RiffHeader.Bytes.Length;

                // format chunk
                FormatChunk.Bytes.CopyTo(bWavHeader, writtenDataSize);
                writtenDataSize += FormatChunk.Bytes.Length;

                // data chunk
                DataChunk.Bytes.CopyTo(bWavHeader, writtenDataSize);
                writtenDataSize += DataChunk.Bytes.Length;

                // at the moment we support only the most basic Wav header
                Debug.Assert(writtenDataSize == 44);

                return bWavHeader;
            }
        }
    }
}
