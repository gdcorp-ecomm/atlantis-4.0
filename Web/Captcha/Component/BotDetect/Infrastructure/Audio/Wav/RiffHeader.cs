using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BotDetect.Audio.Wav
{
    internal struct RiffHeader
    {
        public string RiffId;	        // byte[4]
        public UInt32 RiffSize;         // byte[4]
        public string RiffFormat;       // byte[4]

        public RiffHeader(byte[] rawHeaderData)
        {
            byte[] bTemp;
            List<byte> rawData = new List<byte>(rawHeaderData);

            // the 'RIFF' id must always be at this location
            RiffId = Encoding.ASCII.GetString(rawHeaderData, 0, 4);

            // total riff size, header size + size of all chunks
            bTemp = new byte[4];
            rawData.CopyTo(4, bTemp, 0, 4);
            RiffSize = BitConverter.ToUInt32(bTemp, 0);

            // the 'WAVE' tag must always be at this location
            RiffFormat = Encoding.ASCII.GetString(rawHeaderData, 8, 4);
        }

        public byte[] Bytes
        {
            get
            {
                byte[] bRiffHeader = new byte[12];
                byte[] bTemp;
                int writtenDataSize = 0;

                // riff id
                bTemp = Encoding.ASCII.GetBytes(RiffId);
                bTemp.CopyTo(bRiffHeader, writtenDataSize);
                writtenDataSize += bTemp.Length;

                // riff size
                bTemp = BitConverter.GetBytes(RiffSize);
                bTemp.CopyTo(bRiffHeader, writtenDataSize);
                writtenDataSize += bTemp.Length;

                // riff format
                bTemp = Encoding.ASCII.GetBytes(RiffFormat);
                bTemp.CopyTo(bRiffHeader, writtenDataSize);
                writtenDataSize += bTemp.Length;

                Debug.Assert(writtenDataSize == 12);

                return bRiffHeader;
            }
        }
    }
}
