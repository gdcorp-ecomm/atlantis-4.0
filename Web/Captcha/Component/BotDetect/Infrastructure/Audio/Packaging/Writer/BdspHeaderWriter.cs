using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace BotDetect.Audio.Packaging
{
    internal class BdspHeaderWriter
    {
        SoundPackageFormat _format;

        public BdspHeaderWriter(SoundPackageFormat format)
        {
            _format = format;
        }

        public byte[] CreateHeaderSection(SoundPackage soundPackage, byte[] dataSection,
            byte[] lookupSection)
        {
            byte[] headerSection = null;

            switch (_format)
            {
                case SoundPackageFormat.V300000000:
                    headerSection = CreateHeaderSection_v3_0_0_0(soundPackage, dataSection, lookupSection);
                    break;

                default:
                    throw new NotImplementedException("Unknown SoundPackageFormat");
            }

            return headerSection;
        }

        private static byte[] CreateHeaderSection_v3_0_0_0(SoundPackage soundPackage, byte[] dataSection,
            byte[] lookupSection)
        {
            HeaderInfo info = GetHeaderInfo(SoundPackageFormat.V300000000);

            // description
            byte[] bDescription = BdspString.GetBytes(soundPackage.Description);

            // legal info
            byte[] bLegal = BdspString.GetBytes(info.LegalInfo);

            // calculate header section size
            UInt32 headerSectionSize =
                BdspConstants.FormatNameLength + 
                BdspConstants.FormatVersionLength + 
                BdspConstants.TocLength +
                Convert.ToUInt32(bDescription.Length) +
                Convert.ToUInt32(bLegal.Length);

            // construct header section buffer
            byte[] headerSection = new byte[headerSectionSize];
            UInt32 lookupSectionSize = Convert.ToUInt32(lookupSection.Length);
            UInt32 dataSectionSize = Convert.ToUInt32(dataSection.Length);
            UInt32 writtenDataSize = 0;

            // format name - bytes 0...63
            byte[] bFormatName = Encoding.UTF8.GetBytes(info.FormatName);
            bFormatName.CopyTo(headerSection, 0);
            for (int i = bFormatName.Length; i < BdspConstants.FormatNameLength; i++)
            {
                headerSection[i] = 0x20;
            }
            writtenDataSize += BdspConstants.FormatNameLength;

            // format version - bytes 64...79
            byte[] bFormatVersion = Encoding.UTF8.GetBytes(info.FormatVersion);
            bFormatVersion.CopyTo(headerSection, writtenDataSize);
            for (int i = bFormatVersion.Length; i < BdspConstants.FormatVersionLength; i++)
            {
                headerSection[writtenDataSize + i] = 0x20;
            }
            writtenDataSize += BdspConstants.FormatVersionLength;

            // toc - bytes 80-95
            BitConverter.GetBytes(headerSectionSize).CopyTo(headerSection, writtenDataSize);
            writtenDataSize += BdspConstants.NumberLength;
            BitConverter.GetBytes(lookupSectionSize).CopyTo(headerSection, writtenDataSize);
            writtenDataSize += BdspConstants.NumberLength;
            BitConverter.GetBytes(headerSectionSize + lookupSectionSize).CopyTo(headerSection, writtenDataSize);
            writtenDataSize += BdspConstants.NumberLength;
            BitConverter.GetBytes(dataSectionSize).CopyTo(headerSection, writtenDataSize);
            writtenDataSize += BdspConstants.NumberLength;

            // description
            bDescription.CopyTo(headerSection, writtenDataSize);
            writtenDataSize += Convert.ToUInt32(bDescription.Length);

            // legal info
            bLegal.CopyTo(headerSection, writtenDataSize);
            writtenDataSize += Convert.ToUInt32(bLegal.Length);

            Debug.Assert(writtenDataSize == headerSectionSize);

            return headerSection;
        }

        // format version -specific constants
        public static HeaderInfo GetHeaderInfo(SoundPackageFormat format)
        {
            HeaderInfo info = new HeaderInfo();

            switch(format)
            {
                case SoundPackageFormat.V300000000:
                    info.FormatName = "Lanapsoft BotDetect™ CAPTCHA Sound Package Format";
                    info.FormatVersion = "v3.0.0.0";
                    info.LegalInfo = "Copyright Lanapsoft Inc. 2009-2012. All rights reserved.";
                    break;

                default:
                    throw new NotImplementedException("Unknown SoundPackageFormat");
            }

            return info;
        }
    }

    internal struct HeaderInfo
    {
        public string FormatName;
        public string FormatVersion;
        public string LegalInfo;
    }
}
