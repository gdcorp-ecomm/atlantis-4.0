using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Persistence
{
    internal abstract class PersistenceProvider : IPersistenceProvider, ICollection
    {
        // indexer
        public object this[string key]
        {
            get
            {
                return this.Load(key);
            }

            set
            {
                this.Save(key, value);
            }
        }

        public abstract void Save(string key, object val);

        public abstract object Load(string key);

        public abstract bool Contains(string key);

        public abstract void Remove(string key);

        public abstract void Clear();

        public abstract void CopyTo(Array array, int index);

        public abstract int Count { get; }

        public abstract bool IsSynchronized { get; }

        public abstract object SyncRoot { get; }

        public abstract IEnumerator GetEnumerator();

        public abstract NameObjectCollectionBase.KeysCollection Keys { get; }
    }
}
