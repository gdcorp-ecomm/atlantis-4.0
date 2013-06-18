using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BotDetect.Audio.Wav
{
    internal struct FormatChunk 
    {
        public string FormatId;         // byte[4]
        public UInt32 FormatSize;       // byte[4]
        public WaveFormat WaveFormat;   // byte[16]

        public FormatChunk(byte[] bRawHeaderData)
        {
            FormatId = "fmt ";
            byte[] bTemp;
            List<byte> rawData = new List<byte>(bRawHeaderData);

            // look for the "fmt " subchunk within the header
            int formatIdPosition = -1;
            bTemp = new byte[4];
            for (int i = 0; i < bRawHeaderData.Length; i++)
            {
                rawData.CopyTo(i, bTemp, 0, 4);
                if ("fmt " == Encoding.ASCII.GetString(bTemp))
                {
                    formatIdPosition = i;
                    break;
                }
            }
            if (-1 == formatIdPosition)
            {
                throw new ArgumentException("'fmt ' subchunk not found");
            }

            int readDataSize = 4; // sizeof("fmt ")

            // format size
            rawData.CopyTo(formatIdPosition + readDataSize, bTemp, 0, 4);
            FormatSize = BitConverter.ToUInt32(bTemp, 0);
            readDataSize += sizeof(UInt32);

            // format tag
            rawData.CopyTo(formatIdPosition + readDataSize, bTemp, 0, 2);
            WaveFormat.FormatTag = BitConverter.ToUInt16(bTemp, 0);
            readDataSize += sizeof(UInt16);

            // number of channels
            rawData.CopyTo(formatIdPosition + readDataSize, bTemp, 0, 2);
            WaveFormat.Channels = BitConverter.ToUInt16(bTemp, 0);
            readDataSize += sizeof(UInt16);

            // sampling rate
            rawData.CopyTo(formatIdPosition + readDataSize, bTemp, 0, 4);
            WaveFormat.SamplesPerSec = BitConverter.ToUInt32(bTemp, 0);
            readDataSize += sizeof(UInt32);

            // avgBytesPerSec
            rawData.CopyTo(formatIdPosition + readDataSize, bTemp, 0, 4);
            WaveFormat.AvgBytesPerSec = BitConverter.ToUInt32(bTemp, 0);
            readDataSize += sizeof(UInt32);

            // blockAlign
            rawData.CopyTo(formatIdPosition + readDataSize, bTemp, 0, 2);
            WaveFormat.BlockAlign = BitConverter.ToUInt16(bTemp, 0);
            readDataSize += sizeof(UInt16);

            // bitsPerSample
            rawData.CopyTo(formatIdPosition + readDataSize, bTemp, 0, 2);
            WaveFormat.BitsPerSample = BitConverter.ToUInt16(bTemp, 0);
            readDataSize += sizeof(UInt16);
        }

        public byte[] Bytes
        {
            get
            {
                byte[] bFormatChunk = new byte[24];
                byte[] bTemp;
                int writtenDataSize = 0;

                // format id
                bTemp = Encoding.ASCII.GetBytes(FormatId);
                bTemp.CopyTo(bFormatChunk, writtenDataSize);
                writtenDataSize += bTemp.Length;

                // format size
                bTemp = BitConverter.GetBytes(FormatSize);
                bTemp.CopyTo(bFormatChunk, writtenDataSize);
                writtenDataSize += bTemp.Length;

                // WaveFormat struct
                WaveFormat.Bytes.CopyTo(bFormatChunk, writtenDataSize);
                writtenDataSize += WaveFormat.Bytes.Length;

                Debug.Assert(writtenDataSize == 24);

                return bFormatChunk;
            }
        }
    }

    internal struct WaveFormat
    {
        public UInt16 FormatTag;        // byte[2]
        public UInt16 Channels;         // byte[2]
        public UInt32 SamplesPerSec;    // byte[4]
        public UInt32 AvgBytesPerSec;   // byte[4]
        public UInt16 BlockAlign;       // byte[2]
        public UInt16 BitsPerSample;    // byte[2]

        public byte[] Bytes
        {
            get
            {
                byte[] bWaveFormat = new byte[16];
                byte[] bTemp;
                int writtenDataSize = 0;

                // format tag
                bTemp = BitConverter.GetBytes(FormatTag);
                bTemp.CopyTo(bWaveFormat, writtenDataSize);
                writtenDataSize += bTemp.Length;

                // channels
                bTemp = BitConverter.GetBytes(Channels);
                bTemp.CopyTo(bWaveFormat, writtenDataSize);
                writtenDataSize += bTemp.Length;

                // samples per sec
                bTemp = BitConverter.GetBytes(SamplesPerSec);
                bTemp.CopyTo(bWaveFormat, writtenDataSize);
                writtenDataSize += bTemp.Length;

                // avg bytes per sec
                bTemp = BitConverter.GetBytes(AvgBytesPerSec);
                bTemp.CopyTo(bWaveFormat, writtenDataSize);
                writtenDataSize += bTemp.Length;

                // block align
                bTemp = BitConverter.GetBytes(BlockAlign);
                bTemp.CopyTo(bWaveFormat, writtenDataSize);
                writtenDataSize += bTemp.Length;

                // bits per sampl
                bTemp = BitConverter.GetBytes(BitsPerSample);
                bTemp.CopyTo(bWaveFormat, writtenDataSize);
                writtenDataSize += bTemp.Length;

                Debug.Assert(writtenDataSize == 16);

                return bWaveFormat;
            }
        }
    }
}
