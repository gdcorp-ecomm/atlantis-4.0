using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Diagnostics;

namespace BotDetect.Persistence
{
	/// <summary>
	/// Summary description for ApplicationPersistanceProvider.
	/// </summary>
    internal class AspNetApplicationPersistenceProvider : PersistenceProvider, IPersistenceProvider, ICollection
	{
        private readonly string _keyNameSpace = "ApplicationPersistanceProvider_";

        // singleton
        private static readonly AspNetApplicationPersistenceProvider _instance = new AspNetApplicationPersistenceProvider();
        public static AspNetApplicationPersistenceProvider Persistence
        {
            get
            {
                return _instance;
            }
        }

        private AspNetApplicationPersistenceProvider()
        {
        }

        private string GetApplicationKey(string key)
        {
            return _keyNameSpace + key;
        }

        public override void Save(string key, object val)
        {
			try
			{
				HttpContext.Current.Application.Lock();
				HttpContext.Current.Application[GetApplicationKey(key)] = val;
				HttpContext.Current.Application.UnLock();
				
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
				return HttpContext.Current.Application[GetApplicationKey(key)];
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
				if(null != HttpContext.Current.Application[GetApplicationKey(key)])
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
				if(null != HttpContext.Current.Application[GetApplicationKey(key)])
				{
                    HttpContext.Current.Application.Lock();
					HttpContext.Current.Application.Remove(GetApplicationKey(key));
                    HttpContext.Current.Application.UnLock();
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
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application.Clear();
                HttpContext.Current.Application.UnLock();
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
                return HttpContext.Current.Application.Count;
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
                return HttpContext.Current.Application.Keys;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return HttpContext.Current.Application.GetEnumerator();
        }

    }
}
