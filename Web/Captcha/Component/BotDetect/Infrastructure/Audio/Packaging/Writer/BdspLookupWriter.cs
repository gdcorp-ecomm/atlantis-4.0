using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace BotDetect.Audio.Packaging
{
    internal class BdspLookupWriter
    {
        private SoundPackageFormat _format;

        public BdspLookupWriter(SoundPackageFormat format)
        {
            _format = format;
        }

        public byte[] CreateLookupSection(Dictionary<string, UInt32[]> input)
        {
            byte[] lookupSection = null;

            switch (_format)
            {
                case SoundPackageFormat.V300000000:
                    lookupSection = CreateLookupSection_v3_0_0_0(input);
                    break;

                default:
                    throw new NotImplementedException("Unknown SoundPackageFormat");
            }

            return lookupSection;
        }

        private static byte[] CreateLookupSection_v3_0_0_0(Dictionary<string, UInt32[]> input)
        {
            // cosntruct lookup section of the .bdsp file
            UInt32 lookupSectionSize = CalculateTotalLookupSize(input.Keys);
            byte[] lookupSection = new byte[lookupSectionSize];

            UInt32 writtenDataSize = 0;
            foreach (string identifier in input.Keys)
            {
                // write identifier string
                byte[] bStringData = BdspString.GetBytes(identifier);
                bStringData.CopyTo(lookupSection, writtenDataSize);
                writtenDataSize += Convert.ToUInt32(bStringData.Length);

                // write data section offset for the indentifier
                byte[] bOffset = BitConverter.GetBytes(input[identifier][0]);
                bOffset.CopyTo(lookupSection, writtenDataSize);
                writtenDataSize += Convert.ToUInt32(bOffset.Length);

                // write data section length for the identifier
                byte[] bLength = BitConverter.GetBytes(input[identifier][1]);
                bLength.CopyTo(lookupSection, writtenDataSize);
                writtenDataSize += Convert.ToUInt32(bLength.Length);
            }

            Debug.Assert(writtenDataSize == lookupSectionSize);

            return lookupSection;
        }

        private static UInt32 CalculateTotalLookupSize(Dictionary<string, UInt32[]>.KeyCollection input)
        {
            UInt32 totalDataSize = 0;
            foreach (string identifier in input)
            {
                // UTF8 string size + 4 bytes for string size + 4 bytes for data offset + 4 bytes for data length
                totalDataSize += Convert.ToUInt32(Encoding.UTF8.GetByteCount(identifier)) + 3 * BdspConstants.NumberLength;
            }
            return totalDataSize;
        }
    }
}
