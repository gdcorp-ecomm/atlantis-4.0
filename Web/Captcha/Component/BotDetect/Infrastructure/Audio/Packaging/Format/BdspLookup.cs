using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Audio.Packaging
{
    internal class BdspLookup
    {
        private Dictionary<string, UInt32[]> _lookupEntries;
        public Dictionary<string, UInt32[]> LookupEntries
        {
            get
            {
                return _lookupEntries;
            }
        }

        public BdspLookup()
        {
            _lookupEntries = new Dictionary<string, uint[]>();
        }

    }
}
