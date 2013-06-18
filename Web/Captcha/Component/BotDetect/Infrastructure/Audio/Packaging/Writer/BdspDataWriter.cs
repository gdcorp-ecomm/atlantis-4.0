using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace BotDetect.Audio.Packaging
{
    internal class BdspDataWriter
    {
        private SoundPackageFormat _format;

        public BdspDataWriter(SoundPackageFormat format)
        {
            _format = format;
        }

        public byte[] CreateDataSection(Dictionary<string, byte[]> input,
            Dictionary<string, UInt32[]> lookup)
        {
            byte[] dataSection = null;

            switch (_format)
            {
                case SoundPackageFormat.V300000000:
                    dataSection = CreateDataSection_v3_0_0_0(input, lookup);
                    break;

                default:
                    throw new NotImplementedException("Unknown SoundPackageFormat");
            }

            return dataSection;
        }

        private static byte[] CreateDataSection_v3_0_0_0(Dictionary<string, byte[]> input,
            Dictionary<string, UInt32[]> lookup)
        {
            // construct data section of the .bdsp file
            UInt32 dataSectionSize = CalculateTotalDataSize(input);
            byte[] dataSection = new byte[dataSectionSize];
            
            // we keep a separate dictionary of data positions by data hash
            Dictionary<string, UInt32[]> hashLookup = new Dictionary<string, UInt32[]>(lookup.Count);

            UInt32 writtenDataSize = 0;
            foreach (string flattenedKey in input.Keys)
            {
                byte[] data = input[flattenedKey];
                UInt32 dataSize = Convert.ToUInt32(data.Length);
                string dataHashHex = Crypto.SHA1.HashBase64(data);

                if (!hashLookup.ContainsKey(dataHashHex))
                {
                    // create lookup entry
                    UInt32[] dataPointer = new UInt32[2];
                    dataPointer[0] = writtenDataSize; // offset
                    dataPointer[1] = dataSize; // length
                    lookup.Add(flattenedKey, dataPointer);

                    // write unique data section entry
                    data.CopyTo(dataSection, writtenDataSize);
                    writtenDataSize += dataSize;

                    // create hash lookup entry
                    hashLookup.Add(dataHashHex, dataPointer);
                }
                else
                {
                    // data already exists, point to existing data
                    UInt32[] dataPointer = hashLookup[dataHashHex];
                    lookup.Add(flattenedKey, dataPointer);
                }
            }

            Debug.Assert(writtenDataSize == dataSectionSize);

            return dataSection;
        }

        private static UInt32 CalculateTotalDataSize(Dictionary<string, byte[]> input)
        {
            // we keep a separate dictionary of data positions by data hash
            Dictionary<string, bool> hashLookup = new Dictionary<string, bool>(input.Count);

            // add up the sizes of all data chunks in the given dictionary
            UInt32 totalDataSize = 0;
            foreach (byte[] dataChunk in input.Values)
            {
                // multiple identical entries are counted only once
                string dataHashHex = Crypto.SHA1.HashBase64(dataChunk);
                if (!hashLookup.ContainsKey(dataHashHex))
                {
                    totalDataSize += Convert.ToUInt32(dataChunk.Length);
                    hashLookup.Add(dataHashHex, true);
                }
            }

            return totalDataSize;
        }
    }
}
