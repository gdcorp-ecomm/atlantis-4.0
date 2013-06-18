using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio.Packaging
{
    internal sealed class SoundPackageFacade
    {
        private SoundPackageFacade()
        {
        }

        public static byte[] GetRawSoundData(string filePath, string name)
        {
            SoundPackage package = GetSoundPackage(filePath);
            if (null == package) { return null; }

            // sound package parsing succeeded, get specified entry
            BdspDataChunk bdspDataChunk = package[name];

            byte[] buffer = null;
            // convert entry format if needed
            if (SoundFormat.WavPcm16bit8kHzMono != bdspDataChunk.Format)
            {
                IFormatConverter converter = FormatConverterFactory.CreateConverter(bdspDataChunk.Format);
                buffer = converter.ConvertToWavPcm16bit8kHzMono(bdspDataChunk.Data);
            }
            else
            {
                // the formats match, we can use the data as is
                buffer = bdspDataChunk.Data;
            }
            return buffer;
        }

        // we keep a static cache of already processed SoundPackages,
        // so we don't always re-parse previously parsed sound package files
        private static readonly Dictionary<string, SoundPackage> _soundPackageCache = new Dictionary<string, SoundPackage>();
        private static readonly object _cacheLock = new object();

        public static SoundPackage GetSoundPackage(string filePath)
        {
            if (!_soundPackageCache.ContainsKey(filePath))
            {
                // not in cache, we have to initialize the SoundPackage from the given file
                lock (_cacheLock)
                {
                    // re-check in case the lock was not acquired immediately
                    if (!_soundPackageCache.ContainsKey(filePath))
                    {
                        // delegate file parsing to reader object
                        BdspFileReader bdspReader = new BdspFileReader();
                        SoundPackage soundPackage = bdspReader.ParseSoundPackageFile(filePath);
                        if (null == soundPackage) { return null; }

                        // add the parsed package to cache for re-use
                        _soundPackageCache.Add(filePath, soundPackage);
                    }
                }
            }

            return _soundPackageCache[filePath];
        }

        public static void WriteSoundPackageFile(string folderPath, SoundPackage package, SoundPackageFormat format)
        {
            BdspFileWriter bdspWriter = new BdspFileWriter(package, format);
            bdspWriter.WriteSoundPackageFile(folderPath);
        }
    }
}
