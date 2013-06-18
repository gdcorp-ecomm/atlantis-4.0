using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio.Packaging
{
    internal class SoundPackage
    {
        public SoundPackage()
        {
            // default empty constructor
        }

        // name of the sound package, e.g. "Pronunciation_English_US"
        private string _name;

        /// <summary>
        /// name of the sound package, e.g. "Pronunciation_English_US"
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        // name of the sound package format, i.e. "Lanapsoft BotDetect™ CAPTCHA Sound Package Format"
        private string _formatName;

        /// <summary>
        /// name of the sound package format, i.e. "Lanapsoft BotDetect™ CAPTCHA Sound Package Format"
        /// </summary>
        public string FormatName
        {
            get
            {
                return _formatName;
            }
            set
            {
                _formatName = value;
            }
        }

        // sound package format version, e.g. "3.0.0.0"
        private string _formatVersion;

        /// <summary>
        /// sound package format version, e.g. "3.0.0.0"
        /// </summary>
        public string FormatVersion
        {
            get
            {
                return _formatVersion;
            }
            set
            {
                _formatVersion = value;
            }
        }

        // sound package description, e.g. "US English character pronunciation data"
        private string _description;

        /// <summary>
        /// sound package description, e.g. "US English character pronunciation data"
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        // legal information, e.g. "Copyright Lanapsoft Inc. 2009-2010. All rights reserved."
        private string _legalInfo;

        /// <summary>
        /// legal information, e.g. "Copyright Lanapsoft Inc. 2009-2010. All rights reserved."
        /// </summary>
        public string LegalInfo
        {
            get
            {
                return _legalInfo;
            }
            set
            {
                _legalInfo = value;
            }
        }

        // multi-level name-indexed collection of entries,
        // each entry can contain either binary sound data 
        // (e.g., Entries["a"] contains binary data,) 
        // or a collection of sub-entries 
        // (e.g. Entries["c"] contains two sub-entries: SubEntries["voice1"] & SubEntries["voice2"])
        private Dictionary<string, SoundPackageEntry> _entries = new Dictionary<string,SoundPackageEntry>();

        /// <summary>
        /// multi-level name-indexed collection of entries,
        /// each entry can contain either binary sound data 
        /// (e.g., Entries["a"] contains binary data,) 
        /// or a collection of sub-entries 
        /// (e.g. Entries["c"] contains two sub-entries: SubEntries["voice1"] & SubEntries["voice2"])
        /// </summary>
        public Dictionary<string, SoundPackageEntry> Entries
        {
            get
            {
                return _entries;
            }
        }

        // SoundPackage exposes method to access entry binary sound data
        public BdspDataChunk this[string identifier]
        {
            get
            {
                BdspDataChunk bdspDataChunk = null;
                if (_entries.ContainsKey(identifier))
                {
                    SoundPackageEntry entry = _entries[identifier];
                    if (null != entry && !entry.IsEmpty)
                    {
                        bdspDataChunk = entry.BdspDataChunk;
                    }
                }
                return bdspDataChunk;
            }
        }

        // used for pupulating a SoundPackage from individual sound files
        public void AddBdspDataChunk(string identifier, string ancestorString, BdspDataChunk bdspDataChunk)
        {
            SoundPackageEntry entry = new SoundPackageEntry();
            entry.BdspDataChunk = bdspDataChunk;

            if (null == ancestorString || 0 == ancestorString.Length)
            {
                // top-level entry
                _entries.Add(identifier, entry);
            }
            else
            {
                // sub-entry somewhere in the package
                string[] ancestors = ancestorString.Split('.');
                int ancestorCount = 0;
                string parentIdentifier = ancestors[0];
                Dictionary<string, SoundPackageEntry> location = _entries;

                while (ancestorCount < ancestors.Length - 1)
                {
                    ancestorCount++;
                    
                    if (!location.ContainsKey(parentIdentifier))
                    {
                        location[parentIdentifier] = new SoundPackageEntry();
                    }
                    location = location[parentIdentifier].SubEntries;

                    parentIdentifier = ancestors[ancestorCount];
                }

                location[parentIdentifier].SubEntries.Add(identifier, entry);
            }
        }

        // used for pupulating a SoundPackage from a .bdsp file
        public void AddBdspDataChunk(string identifier, BdspDataChunk bdspDataChunk)
        {
            SoundPackageEntry entry = new SoundPackageEntry();
            entry.BdspDataChunk = bdspDataChunk;

            string[] identifierParts = identifier.Split('.');
            Dictionary<string, SoundPackageEntry> entryCollection = _entries;
            int pos = 0;
            string identifierPart = identifierParts[pos];
            while (pos < identifierParts.Length - 1)
            {
                if (!entryCollection.ContainsKey(identifierPart))
                {
                    entryCollection.Add(identifierPart, new SoundPackageEntry());
                }

                entryCollection = entryCollection[identifierPart].SubEntries;
                pos++;
                identifierPart = identifierParts[pos];
            }
            entryCollection.Add(identifierPart, entry);
        }

        /// <summary>
        /// total number of entries contained in a SounPackage
        /// </summary>
        public int TotalEntryCount
        {
            get
            {
                int count = 0;
                foreach (string key in _entries.Keys)
                {
                    SoundPackageEntry entry = _entries[key];
                    count++;
                    count += CountSubEntries(entry);
                }
                return count;
            }
        }

        // recursive sub-entry counter
        private int CountSubEntries(SoundPackageEntry entry)
        {
            int count = 0;
            if (entry.HasSubEntries)
            {
                foreach (string key in entry.SubEntries.Keys)
                {
                    SoundPackageEntry subentry = entry.SubEntries[key];
                    count++;
                    count += CountSubEntries(subentry);
                }
            }
            return count;
        }

        /// <summary>
        /// number of entries with binary sound data contained in a SounPackage
        /// </summary>
        public int DataEntryCount
        {
            get
            {
                int count = 0;
                foreach (string key in _entries.Keys)
                {
                    SoundPackageEntry entry = _entries[key];
                    if (entry.HasData) { count++; }
                    count += CountDataSubEntries(entry);
                }
                return count;
            }
        }

        // recursive sub-entry counter
        private int CountDataSubEntries(SoundPackageEntry entry)
        {
            int count = 0;
            if (entry.HasSubEntries)
            {
                foreach (string key in entry.SubEntries.Keys)
                {
                    SoundPackageEntry subentry = entry.SubEntries[key];
                    if (subentry.HasData) { count++; }
                    count += CountDataSubEntries(subentry);
                }
            }
            return count;
        }

        /// <summary>
        /// number of entries with sub-entries contained in a SounPackage
        /// </summary>
        public int ContainerEntryCount
        {
            get
            {
                int count = 0;
                foreach (string key in _entries.Keys)
                {
                    SoundPackageEntry entry = _entries[key];
                    if (entry.HasSubEntries) { count++; }
                    count += CountDataSubEntries(entry);
                }
                return count;
            }
        }

        // recursive sub-entry counter
        private int CountContainerSubEntries(SoundPackageEntry entry)
        {
            int count = 0;
            if (entry.HasSubEntries)
            {
                foreach (string key in entry.SubEntries.Keys)
                {
                    SoundPackageEntry subentry = entry.SubEntries[key];
                    if (subentry.HasSubEntries) { count++; }
                    count += CountDataSubEntries(subentry);
                }
            }
            return count;
        }
 
    }
}
