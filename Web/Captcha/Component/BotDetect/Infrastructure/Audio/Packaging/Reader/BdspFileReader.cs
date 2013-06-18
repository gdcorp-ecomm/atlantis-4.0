using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Security.Permissions;

namespace BotDetect.Audio.Packaging
{
    internal class BdspFileReader
    {
        SoundPackage _soundPackage;

        BdspToc _toc;

        BdspHeaderReader _headerReader;
        BdspLookupReader _lookupReader;
        BdspDataReader _dataReader;

        public BdspFileReader()
        {
        }

        private static bool IsDefaultLocale(string filePath)
        {
            return (0 == String.CompareOrdinal(
                Path.GetFileName(filePath), 
                CaptchaDefaults.Localization.PronunciationFilename)
            );
        }

        public SoundPackage ParseSoundPackageFile(string filePath)
        {
            #if (NET20)
            try
            {
                // neccessary for GAC + Medium/Minimal trust scenarios
                FileIOPermission fileReadPermission = new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath(filePath));
                fileReadPermission.Assert();
            }
            catch (System.Security.SecurityException)
            {
                // insufficient permissions, abort
                return null;
            }
            #endif

            // basic checks
            bool fileExists = File.Exists(filePath);
            bool isDefaultLocale = IsDefaultLocale(filePath);

            // the sound package file doesn't exist, we can't do anything but report it
            if (!fileExists && !isDefaultLocale)
            {
                return null;
            }

            // try to access sound package data
            try
            {
                _soundPackage = new SoundPackage();

                if (fileExists)
                {
                    // try to parse the specified file if it exists
                    _soundPackage.Name = Path.GetFileNameWithoutExtension(filePath);
                    using (Stream input = File.OpenRead(filePath))
                    {
                        ParseInputStream(input);
                    }
                }
                else
                {
                    // fall back to the default locale embedded resource 
                    _soundPackage.Name = Path.GetFileNameWithoutExtension(
                        CaptchaDefaults.Localization.PronunciationFilename
                     );

                    using (Stream input = 
                        ResourceHelper.GetResourceStream(
                            ".Infrastructure.Audio.Pronunciation.Resources.Pronunciation_English_US.bdsp"
                        )
                    )
                    {
                        ParseInputStream(input);
                    }
                }
            }
            catch
            {
                // prevent returning an invalid SoundPackage in case of any errors
                _soundPackage = null;
                throw;
            }
            
            return _soundPackage;
        }

        protected void ParseInputStream(Stream input)
        {
            _headerReader = new BdspHeaderReader(input);

            // format name, version and the Toc are in the same positions 
            // in all Bdsp format versions
            _soundPackage.FormatName = _headerReader.ReadFormatName();
            _soundPackage.FormatVersion = _headerReader.ReadFormatVersion();
            _toc = _headerReader.ReadToc();

            // all other fields can change with the version
            switch (_soundPackage.FormatVersion)
            {
                case "v3.0.0.0":
                    ParseSoundPackage_v3_0_0_0(input);
                    break;

                default:
                    throw new NotImplementedException("Unknown SoundPackageFormat");
            }
        }

        private void ParseSoundPackage_v3_0_0_0(Stream input)
        {
            UInt32 readHeader = _headerReader.Read_v3_0_0_0_Header(_soundPackage);

            _lookupReader = new BdspLookupReader(input, _toc);
            BdspLookup lookup = _lookupReader.Read_v3_0_0_0_Lookup();
            UInt32 readLookup = _lookupReader.ReadLookupBytes;

            _dataReader = new BdspDataReader(input, _toc, lookup);
            UInt32 readData = _dataReader.Read_v3_0_0_0_Entries(_soundPackage);

            if (input.Length != readHeader + readLookup + readData)
            {
                throw new InvalidDataException("Not a valid .bdsp file - invalid toc - total length not valid");
            }
        }

    }

}
