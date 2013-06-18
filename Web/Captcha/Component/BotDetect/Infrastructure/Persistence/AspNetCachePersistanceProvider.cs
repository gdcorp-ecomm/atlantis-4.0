using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Diagnostics;

namespace BotDetect.Persistence
{
    /// <summary>
    /// Summary description for WebCachePersistanceProvider.
    /// </summary>
    internal class AspNetCachePersistenceProvider : PersistenceProvider, IPersistenceProvider, ICollection
    {
        private readonly string _keyNameSpace = "CachePersistanceProvider_";

        // singleton
        private static readonly AspNetCachePersistenceProvider _instance = new AspNetCachePersistenceProvider();
        public static AspNetCachePersistenceProvider Persistence
        {
            get
            {
                return _instance;
            }
        }

        private AspNetCachePersistenceProvider()
        {
        }

        private string GetWebCacheKey(string key)
        {
            return _keyNameSpace + key;
        }

        public override void Save(string key, object val)
        {
			try
			{
			    HttpRuntime.Cache[GetWebCacheKey(key)] = val;
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
				return HttpRuntime.Cache[GetWebCacheKey(key)];
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
				if(null != HttpRuntime.Cache[GetWebCacheKey(key)])
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
				if(null != HttpRuntime.Cache[GetWebCacheKey(key)])
				{
					HttpRuntime.Cache.Remove(GetWebCacheKey(key));
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
            try
            {
                foreach (DictionaryEntry de in HttpRuntime.Cache)
                {
                    HttpRuntime.Cache.Remove(de.Key as string);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                throw;
            }
		}

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override int Count
        {
            get 
            {
                return HttpRuntime.Cache.Count;
            }
        }

        public override bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public override object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                throw new NotImplementedException("TODO: ASP.NET Cache key collection");
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return HttpRuntime.Cache.GetEnumerator();
        }

    }
}
