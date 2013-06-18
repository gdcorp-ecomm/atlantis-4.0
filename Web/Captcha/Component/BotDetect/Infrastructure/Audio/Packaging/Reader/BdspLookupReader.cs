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
    internal class BdspLookupReader
    {
        Stream _input;
        BdspToc _toc;

        UInt32 _readLookupBytes; // count lookup read bytes
        public UInt32 ReadLookupBytes
        {
            get
            {
                return _readLookupBytes;
            }
        }

        public BdspLookupReader(Stream input, BdspToc toc)
        {
            _input = input;
            _toc = toc;
        }

        public BdspLookup Read_v3_0_0_0_Lookup()
        {
            BdspLookup lookup = new BdspLookup();
            _readLookupBytes = 0;
            while (_readLookupBytes < _toc.LookupSectionLength)
            {
                string identifier = ReadEntryIdentifier();
                UInt32[] position = ReadEntryPosition();
                lookup.LookupEntries.Add(identifier, position);
            }

            // read lookup length validation
            if (_readLookupBytes != _toc.LookupSectionLength)
            {
                throw new InvalidDataException("Not a valid .bdsp file - lookup read error");
            }

            return lookup;
        }

        private string ReadEntryIdentifier()
        {
            // read identifier
            byte[] bIntValue = new byte[BdspConstants.NumberLength];
            _input.Read(bIntValue, 0, BdspConstants.NumberLength);
            _readLookupBytes += BdspConstants.NumberLength;
            UInt32 identifierLength = BitConverter.ToUInt32(bIntValue, 0);

            byte[] bIdentifier = new byte[Convert.ToInt32(identifierLength) + BdspConstants.NumberLength];
            bIntValue.CopyTo(bIdentifier, 0);
            _input.Read(bIdentifier, BdspConstants.NumberLength, Convert.ToInt32(identifierLength));
            _readLookupBytes += identifierLength;
            string identifier = BdspString.GetString(bIdentifier);

            // validate identifier
            if (!validIdentifier.IsMatch(identifier))
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid entry identifier");
            }

            return identifier;
        }

        // alphanumeric unicode chars + dots, underscores, dashes
		internal static readonly  Regex validIdentifier = new Regex(@"^[\p{N}\p{L}\._-]+$");

        private UInt32[] ReadEntryPosition()
        {
            UInt32[] position = new UInt32[2];

            // read entry data position
            byte[] bIntValue = new byte[BdspConstants.NumberLength];
            _input.Read(bIntValue, 0, BdspConstants.NumberLength);
            _readLookupBytes += BdspConstants.NumberLength;
            position[0] = BitConverter.ToUInt32(bIntValue, 0);

            // read data entry size
            _input.Read(bIntValue, 0, BdspConstants.NumberLength);
            _readLookupBytes += BdspConstants.NumberLength;
            position[1] = BitConverter.ToUInt32(bIntValue, 0);

            // validate data position
            if (0 == position[1])
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid entry length");
            }

            return position;
        }
    }
}
