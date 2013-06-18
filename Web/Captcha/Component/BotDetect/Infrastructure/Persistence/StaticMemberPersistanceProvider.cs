using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

namespace BotDetect.Persistence
{
    /// <summary>
    /// Summary description for SimplePersistenceProvider.
    /// </summary>
    internal class StaticMemberPersistenceProvider : PersistenceProvider, IPersistenceProvider, ICollection
    {
        private readonly string _keyNameSpace = "SimplePersistenceProvider_";

		private Hashtable _container;
        private readonly object locker = new object();

        // singleton
        private static readonly StaticMemberPersistenceProvider _instance = new StaticMemberPersistenceProvider();
        public static StaticMemberPersistenceProvider Persistence
        {
            get
            {
                return _instance;
            }
        }

        private StaticMemberPersistenceProvider()
        {
            _container = new Hashtable();
        }

        private string GetHashtableKey(string key)
        {
            return _keyNameSpace + key;
        }

        public override void Save(string key, object val)
        {
			try
			{
                lock (locker)
                {
                    _container[GetHashtableKey(key)] = val;
                }
			}
			catch (Exception ex)
			{
				Debug.Assert(false, ex.Message);
				throw;
			}
        }

        public override object Load(string key)
        {
			try
			{
                return _container[GetHashtableKey(key)];
			}
			catch (Exception ex)
			{
				Debug.Assert(false, ex.Message);
				throw;
			}
        }

        public override bool Contains(string key)
        {
			try
			{
				if(null != _container[GetHashtableKey(key)])
				{
					return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				Debug.Assert(false, ex.Message);
				throw;
			}
        }

        public override void Remove(string key)
		{
			try
			{
                if (null != _container[GetHashtableKey(key)])
                {
                    lock (locker)
                    {
                        _container.Remove(GetHashtableKey(key));
                    }
                }
			}
			catch (Exception ex)
			{
				Debug.Assert(false, ex.Message);
				throw;
			}
		}

		public override void Clear()
		{
            lock (locker)
            {
                _container.Clear();
            }
		}

        public override void CopyTo(Array array, int index)
        {
            _container.CopyTo(array, index);
        }

        public override int Count
        {
            get 
            {
                return _container.Count;
            }
        }

        public override bool IsSynchronized
        {
            get 
            {
                return _container.IsSynchronized;
            }
        }

        public override object SyncRoot
        {
            get 
            {
                return _container.SyncRoot;
            }
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return _container.Keys as NameObjectCollectionBase.KeysCollection;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return _container.GetEnumerator();
        }

    }
}
