using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio.Packaging
{
    internal sealed class BdspString
    {
        private BdspString()
        {
        }

        public static byte[] GetBytes(string dotNetString)
        {
            // calculate the resulting byte[] size
            int rawDataLength = 0;
            UInt32 stringLength = Convert.ToUInt32(Encoding.UTF8.GetByteCount(dotNetString));
            rawDataLength += BdspConstants.NumberLength; 
            rawDataLength += Encoding.UTF8.GetByteCount(dotNetString);

            // allocate empty byte[]
            byte[] rawData = new byte[rawDataLength];

            // set string length bytes
            byte[] stringLengthBytes = BitConverter.GetBytes(stringLength);
            stringLengthBytes.CopyTo(rawData, 0);

            // set string value bytes
            byte[] stringValueBytes = Encoding.UTF8.GetBytes(dotNetString);
            stringValueBytes.CopyTo(rawData, BdspConstants.NumberLength);

            return rawData;
        }

        public static string GetString(byte[] rawData)
        {
            // extract the string length bytes from the raw data
            byte[] stringLengthBytes = new byte[BdspConstants.NumberLength];
            for (int i = 0; i < BdspConstants.NumberLength; i++)
            {
                stringLengthBytes[i] = rawData[i];
            }
            UInt32 length = BitConverter.ToUInt32(stringLengthBytes, 0);

            // extraxt the string value bytes from the raw data
            int stringValueSize = Convert.ToInt32(length);
            byte[] stringValueBytes = new byte[stringValueSize];
            for (int i = 0; i < length; i++)
            {
                stringValueBytes[i] = rawData[i + BdspConstants.NumberLength];
            }
            string value = Encoding.UTF8.GetString(stringValueBytes);

            // compare extracted and parsed string length to ensure everything is ok
            System.Diagnostics.Debug.Assert(length == Convert.ToUInt32(Encoding.UTF8.GetByteCount(value)));

            return value;
        }

    }
}
