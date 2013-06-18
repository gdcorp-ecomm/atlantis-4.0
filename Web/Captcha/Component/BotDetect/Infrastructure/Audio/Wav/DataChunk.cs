using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BotDetect.Audio.Wav
{
    internal struct DataChunk
    {
        public string DataId;           // byte[4]
        public UInt32 DataSize;         // byte[4]

        public DataChunk(byte[] bRawHeaderData)
        {
            DataId = "data";
            byte[] bTemp;
            List<byte> rawData = new List<byte>(bRawHeaderData);

            // look for the "data" subchunk within the header
            int dataIdPosition = -1;
            bTemp = new byte[4];
            for (int i = 0; i < bRawHeaderData.Length; i++)
            {
                rawData.CopyTo(i, bTemp, 0, 4);
                if ("data" == Encoding.ASCII.GetString(bTemp))
                {
                    dataIdPosition = i;
                    break;
                }
            }
            if (-1 == dataIdPosition)
            {
                throw new ArgumentException("'data' subchunk not found");
            }

            // data size
            rawData.CopyTo(dataIdPosition + 4, bTemp, 0, 4);
            DataSize = BitConverter.ToUInt32(bTemp, 0);
        }

        public byte[] Bytes
        {
            get
            {
                byte[] bDataChunk = new byte[8];
                byte[] bTemp;
                int writtenDataSize = 0;

                // data id
                bTemp = Encoding.ASCII.GetBytes(DataId);
                bTemp.CopyTo(bDataChunk, writtenDataSize);
                writtenDataSize += bTemp.Length;

                // data size
                bTemp = BitConverter.GetBytes(DataSize);
                bTemp.CopyTo(bDataChunk, writtenDataSize);
                writtenDataSize += bTemp.Length;

                Debug.Assert(writtenDataSize == 8);

                return bDataChunk;
            }
        }
    }
}
