using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace BotDetect.Audio.Packaging
{
    internal class BdspDataReader
    {
        Stream _input;
        BdspToc _toc;
        BdspLookup _lookup;

        UInt32 _readDataBytes; // count data read bytes

        // we keep a separate dictionary of data hashes for uniqueness checks
        Dictionary<string, bool> _dataHashes;

        public BdspDataReader(Stream input, BdspToc toc, BdspLookup lookup)
        {
            _input = input;
            _toc = toc;
            _lookup = lookup;
            _dataHashes = new Dictionary<string, bool>();
        }

        public UInt32 Read_v3_0_0_0_Entries(SoundPackage soundPackage)
        {
            _readDataBytes = 0;
            foreach (string identifier in _lookup.LookupEntries.Keys)
            {
                UInt32[] position = _lookup.LookupEntries[identifier];
                BdspDataChunk bdspDataChunk = ReadEntryData(position);

                soundPackage.AddBdspDataChunk(identifier, bdspDataChunk);
            }

            // read data length validation
            if (_readDataBytes != _toc.DataSectionLength)
            {
                throw new InvalidDataException("Not a valid .bdsp file - data read error");
            }

            return _readDataBytes;
        }

        BdspDataChunk ReadEntryData(UInt32[] position)
        {
            // read binary data
            UInt32 offset = position[0];
            UInt32 size = position[1];

            byte[] bDataChunk = new byte[size];
            long positionBackup = _input.Position;

            _input.Seek(_toc.DataSectionStart + offset, SeekOrigin.Begin);
            _input.Read(bDataChunk, 0, Convert.ToInt32(size));

            // we only count length of duplicate data entries once
            string dataHashHex = Crypto.SHA1.HashBase64(bDataChunk);
            if (!_dataHashes.ContainsKey(dataHashHex))
            {
                _readDataBytes += size;
                _dataHashes.Add(dataHashHex, true);
            }

            _input.Seek(positionBackup, SeekOrigin.Begin);

            // validate binary data - in the BinarySoundDataChunk constructor
            BdspDataChunk dataChunk = new BdspDataChunk(bDataChunk);

            return dataChunk;
        } 
    }
}
