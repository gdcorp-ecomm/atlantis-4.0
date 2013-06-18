using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace BotDetect.Audio.Packaging
{
    internal class BdspFileWriter
    {
        private SoundPackage _package;
        private SoundPackageFormat _format;

        // we flatten multiple levels of subentry binary data into this dictionary
        Dictionary<string, byte[]> _flattenedBinaryData;

        // for each identifier, we keep the data section start offset and data length
        Dictionary<string, UInt32[]> _dataSectionLookup;

        BdspDataWriter _dataWriter;
        BdspLookupWriter _lookupWriter;
        BdspHeaderWriter _headerWriter;

        public BdspFileWriter(SoundPackage package, SoundPackageFormat format)
        {
            _package = package;
            _format = format;

            _flattenedBinaryData = new Dictionary<string, byte[]>(_package.Entries.Count);
            _dataSectionLookup = new Dictionary<string, uint[]>(_flattenedBinaryData.Count);

            _dataWriter = new BdspDataWriter(_format);
            _lookupWriter = new BdspLookupWriter(_format);
            _headerWriter = new BdspHeaderWriter(_format);
        }

        public void WriteSoundPackageFile(string folderPath)
        {
            switch (_format)
            {
                case SoundPackageFormat.V300000000:
                    WriteSoundPackageFile_v3_0_0_0(folderPath);
                    break;

                default:
                    throw new NotImplementedException("Unknown SoundPackageFormat");
            }
        }

        private void WriteSoundPackageFile_v3_0_0_0(string folderPath)
        {
            // create a .bdsp file from a given SoundPackage object
            string fileName = _package.Name + ".bdsp";

            ProcessEntries(_package.Entries, null, _flattenedBinaryData);

            byte[] dataSection = _dataWriter.CreateDataSection(_flattenedBinaryData, _dataSectionLookup);
            byte[] lookupSection = _lookupWriter.CreateLookupSection(_dataSectionLookup);
            byte[] headerSection = _headerWriter.CreateHeaderSection(_package, dataSection, lookupSection);

            // file creation
            string filePath = Path.Combine(folderPath, fileName);
            using (FileStream outputStream = new FileStream(filePath, FileMode.Create))
            {
                outputStream.Write(headerSection, 0, headerSection.Length);
                outputStream.Write(lookupSection, 0, lookupSection.Length);
                outputStream.Write(dataSection, 0, dataSection.Length);;
            }
        }

        private void ProcessEntries(Dictionary<string, SoundPackageEntry> input,
            string parentIdentifier, Dictionary<string, byte[]> output)
        {
            // recursively copy all SoundPackage data into a flat access structure
            foreach (string entryIdentifier in input.Keys)
            {
                SoundPackageEntry entry = input[entryIdentifier];

                // construct flattened entry identifier
                string outputIdentifier = entryIdentifier;
                if (!String.IsNullOrEmpty(parentIdentifier))
                {
                    outputIdentifier = parentIdentifier + "." + outputIdentifier;
                }

                // add entry data to the flattenedBinaryData dictionary
                if (entry.HasData)
                {
                    output.Add(outputIdentifier, entry.BdspDataChunk.Bytes);
                }

                // process sub-entries
                if (entry.HasSubEntries)
                {
                    ProcessEntries(entry.SubEntries, outputIdentifier, output);
                }
            }
        } 
    } 
}
