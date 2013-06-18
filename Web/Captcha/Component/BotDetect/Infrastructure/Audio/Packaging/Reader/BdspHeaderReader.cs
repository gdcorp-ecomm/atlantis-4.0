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
    internal class BdspHeaderReader
    {
        Stream _input;
        UInt32 _readHeaderBytes;

        public BdspHeaderReader(Stream input)
        {
            _input = input;
        }

        public string ReadFormatName()
        {
            // read format name
            byte[] bFormatName = new byte[BdspConstants.FormatNameLength];
            _input.Read(bFormatName, 0, BdspConstants.FormatNameLength);
            _readHeaderBytes += BdspConstants.FormatNameLength;
            string formatName = Encoding.UTF8.GetString(bFormatName).Trim();

            // validate format name
            if (formatName != "Lanapsoft BotDetect™ CAPTCHA Sound Package Format")
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid format name");
            }

            return formatName;
        }
				
        public string ReadFormatVersion()
        {
            // read format version
            byte[] bFormatVersion = new byte[BdspConstants.FormatVersionLength];
            _input.Read(bFormatVersion, 0, BdspConstants.FormatVersionLength);
            _readHeaderBytes += BdspConstants.FormatVersionLength;
            string formatVersion = Encoding.UTF8.GetString(bFormatVersion).Trim();

            // validate format version
            if (!versionMatch.IsMatch(formatVersion))
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid format version");
            }

            return formatVersion;
        }
				
		internal static readonly Regex versionMatch = new Regex(@"^v(\d+)\.(\d+)\.(\d+)\.(\d+)$");

        public UInt32 Read_v3_0_0_0_Header(SoundPackage soundPackage)
        {
            soundPackage.Description = ReadDescription();
            soundPackage.LegalInfo = ReadLegalInfo();

            return _readHeaderBytes;
        }

        public BdspToc ReadToc()
        {
            BdspToc toc;

            // read lookup offset
            byte[] bIntValue = new byte[BdspConstants.NumberLength];
            _input.Read(bIntValue, 0, BdspConstants.NumberLength);
            _readHeaderBytes += BdspConstants.NumberLength;
            toc.LookupSectionStart = BitConverter.ToUInt32(bIntValue, 0);

            // validate lookup offset
            if (toc.LookupSectionStart < BdspConstants.MinHeaderLength) // minimal theoretical offset
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid toc - lookup offset not valid");
            }

            // read lookup length
            _input.Read(bIntValue, 0, BdspConstants.NumberLength);
            _readHeaderBytes += BdspConstants.NumberLength;
            toc.LookupSectionLength = BitConverter.ToUInt32(bIntValue, 0);

            // validate lookup length
            if (toc.LookupSectionLength < BdspConstants.MinLookupLength) // minimal theoretical length
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid toc - lookup length not valid");
            }

            // read data offset
            _input.Read(bIntValue, 0, BdspConstants.NumberLength);
            _readHeaderBytes += BdspConstants.NumberLength;
            toc.DataSectionStart = BitConverter.ToUInt32(bIntValue, 0);

            // validate data offset
            if (toc.DataSectionStart != toc.LookupSectionStart + toc.LookupSectionLength)
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid toc - data section offset not valid");
            }

            // read data length
            _input.Read(bIntValue, 0, BdspConstants.NumberLength);
            _readHeaderBytes += BdspConstants.NumberLength;
            toc.DataSectionLength = BitConverter.ToUInt32(bIntValue, 0);

            // validate data length
            if (_input.Length != toc.DataSectionStart + toc.DataSectionLength)
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid toc - data section length not valid");
            }

            return toc;
        }

        private string ReadDescription()
        {
            // read package description
            byte[] bIntValue = new byte[BdspConstants.NumberLength];
            _input.Read(bIntValue, 0, BdspConstants.NumberLength);
            _readHeaderBytes += BdspConstants.NumberLength;
            UInt32 descriptionLength = BitConverter.ToUInt32(bIntValue, 0);

            byte[] bDescription = new byte[descriptionLength + BdspConstants.NumberLength];
            bIntValue.CopyTo(bDescription, 0);
            _input.Read(bDescription, BdspConstants.NumberLength, Convert.ToInt32(descriptionLength));
            _readHeaderBytes += descriptionLength;
            string description = BdspString.GetString(bDescription);

            // validate package description
            if (0 > description.Trim().Length)
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid package description");
            }

            return description;
        }

        private string ReadLegalInfo()
        {
            // read package legal info
            byte[] bIntValue = new byte[BdspConstants.NumberLength];
            _input.Read(bIntValue, 0, BdspConstants.NumberLength);
            _readHeaderBytes += BdspConstants.NumberLength;
            UInt32 legalLength = BitConverter.ToUInt32(bIntValue, 0);

            byte[] bLegal = new byte[legalLength + BdspConstants.NumberLength];
            bIntValue.CopyTo(bLegal, 0);
            _input.Read(bLegal, BdspConstants.NumberLength, Convert.ToInt32(legalLength));
            _readHeaderBytes += legalLength;
            string legalInfo = BdspString.GetString(bLegal);

            // validate package legal info
            if (!legalInfo.Contains("Copyright Lanapsoft Inc."))
            //if ("Copyright Lanapsoft Inc. 2009-2010. All rights reserved." != legalInfo)
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid package legal info");
            }

            return legalInfo;
        }
    }
}
