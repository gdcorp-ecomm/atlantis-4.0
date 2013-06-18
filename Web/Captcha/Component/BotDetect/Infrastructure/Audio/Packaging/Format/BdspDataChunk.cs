using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace BotDetect.Audio.Packaging
{
    internal class BdspDataChunk 
    {
        // sound data bytes
        private byte[] _data;

        /// <summary>
        /// sound data bytes
        /// </summary>
        public byte[] Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        // binary sound data format mark / enum value for the given data chunk
        private SoundFormat _format = SoundFormat.WavPcm16bit8kHzMono;

        /// <summary>
        /// binary sound data format mark / enum value for the given data chunk
        /// </summary>
        public SoundFormat Format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
            }
        }

        // SHA1 hash of the headerless sound data, used to recognize duplicate entries
        private byte[] _hash; 

        // constructor used for constructing from code
        public BdspDataChunk(IAudio sound)
        {
            _data = sound.HeaderlessData;

            _format = sound.Format;

            _hash = Crypto.SHA1.Hash(_data);
            Debug.Assert(_hash.Length == BdspConstants.DataHashSize);
        }

        // constructor used for constructing from binary file
        public BdspDataChunk(byte[] rawData)
        {
            // read the format mark
            byte[] bFormatMark = new byte[BdspConstants.FormatMarkSize];
            bFormatMark[0] = rawData[0];
            bFormatMark[1] = rawData[1];
            UInt16 formatMark = BitConverter.ToUInt16(bFormatMark, 0);
            _format = (SoundFormat)formatMark;

            // read the data hash
            _hash = new byte[BdspConstants.DataHashSize];
            for (int i = 0; i < BdspConstants.DataHashSize; i++)
            {
                _hash[i] = rawData[i + BdspConstants.FormatMarkSize];
            }

            // read the binary data
            _data = new byte[rawData.Length - BdspConstants.FormatMarkSize - BdspConstants.DataHashSize];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = rawData[i + BdspConstants.FormatMarkSize + BdspConstants.DataHashSize];
            }

            // check that the data matches the hash value
            byte[] dataHash = Crypto.SHA1.Hash(_data);
            Debug.Assert(_hash.Length == BdspConstants.DataHashSize);
            if (!Crypto.SHA1.IsHashMatch(dataHash, _hash))
            {
                throw new InvalidDataException("Data and hash mismatch");
            }
        }

        // get the sound stream from the entry
        public byte[] SoundStream
        {
            get
            {
                IAudio sound = Audio.GetAudio(_data, _format);
                return sound.Bytes;
            }
        }

        public byte[] Bytes
        {
            get
            {
                // headerless binary data prepended by 2 bytes of format mark and 20 bytes of hash
                byte[] serialized = new byte[_data.Length + BdspConstants.FormatMarkSize + BdspConstants.DataHashSize];

                // write the format mark
                UInt16 formatMark = (UInt16)_format;
                byte[] bFormatMark = BitConverter.GetBytes(formatMark);
                serialized[0] = bFormatMark[0];
                serialized[1] = bFormatMark[1];

                // write the data hash
                for (int i = 0; i < BdspConstants.DataHashSize; i++)
                {
                    serialized[i + BdspConstants.FormatMarkSize] = _hash[i];
                }

                // write the binary data
                for (int i = 0; i < _data.Length; i++)
                {
                    serialized[i + BdspConstants.FormatMarkSize + BdspConstants.DataHashSize] = _data[i];
                }

                return serialized;
            }
        }
    }
}
