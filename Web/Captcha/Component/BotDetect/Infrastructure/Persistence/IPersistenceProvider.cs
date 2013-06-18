using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Persistence
{
    internal interface IPersistenceProvider : ICollection
    {
        object this[string key] { get; set; }
        void Save(string key, object val);
        object Load(string key);
        bool Contains(string key);
        void Remove(string key);
        void Clear();
        NameObjectCollectionBase.KeysCollection Keys { get; }
    }
}
