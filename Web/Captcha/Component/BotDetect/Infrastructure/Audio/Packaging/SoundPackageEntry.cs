using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio.Packaging
{
    internal class SoundPackageEntry
    {
        // binary sound data stored in this entry, for example SoundPackageInstance["a"]
        private BdspDataChunk _bdspDataChunk;

        // sub-entries, for example SoundPackageInstance["a.voice1"] and SoundPackageInstance["a.voice2"]
        private Dictionary<string, SoundPackageEntry> _subEntries = new Dictionary<string, SoundPackageEntry>();

        /// <summary>
        /// sub-entries, for example SoundPackageInstance["a.voice1"] and SoundPackageInstance["a.voice2"]
        /// </summary>
        internal Dictionary<string, SoundPackageEntry> SubEntries
        {
            get
            {
                return _subEntries;
            }
        }

        /// <summary>
        /// a SoundPackageEntry is empty if it contains no binary data and no sub-entries
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return !HasData && !HasSubEntries;
            }
        }

        /// <summary>
        /// since BinarySoundDataChunk accessor returns a randomly selected subentry 
        /// value if the entry's own data is emtpy, we need this separate method 
        /// </summary>
        public bool HasData
        {
            get
            {
                return !(null == _bdspDataChunk ||
                    null == _bdspDataChunk.Data ||
                    0 == _bdspDataChunk.Data.Length);
            }
        }

        // does the entry contain subentries?
        public bool HasSubEntries
        {
            get
            {
                return !(null == _subEntries || 0 == _subEntries.Count);
            }
        }

        // entry data access
        public BdspDataChunk BdspDataChunk
        {
            get
            {
                if (this.HasData)
                {
                    // use the current node sound data if it exists
                    return _bdspDataChunk;
                }
                else
                {
                    if (this.HasSubEntries)
                    {
                        // use a randomly selected sub-entry data otherwise
                        string[] childNodeKeys = new string[_subEntries.Count];
                        _subEntries.Keys.CopyTo(childNodeKeys, 0);

                        int selectedIndex = RandomGenerator.Next(childNodeKeys.Length - 1);
                        string selectedKey = childNodeKeys[selectedIndex];

                        return _subEntries[selectedKey].BdspDataChunk;
                    }

                    return null;
                }
            }
            set
            {
                _bdspDataChunk = value;
            }
        }

        // sub-entry data access
        public BdspDataChunk this[string identifier]
        {
            get
            {
                BdspDataChunk bdspDataChunk = null;
                int dotIndex = identifier.IndexOf('.');
                if (-1 == dotIndex)
                {
                    // no sub-entry selection
                    if (null != _subEntries[identifier] && !_subEntries[identifier].IsEmpty)
                    {
                        bdspDataChunk = _subEntries[identifier].BdspDataChunk;
                    }
                }
                else
                {
                    // sub-entry selection, navigate to the right sub-entry
                    string parentIdentifier = identifier.Substring(0, dotIndex);
                    string descendantIdentfier = identifier.Substring(dotIndex);
                    if (null != _subEntries[parentIdentifier] && !_subEntries[descendantIdentfier].IsEmpty)
                    {
                        bdspDataChunk = _subEntries[parentIdentifier][descendantIdentfier];
                    }
                }

                return bdspDataChunk;
            }
        }
    }
}
