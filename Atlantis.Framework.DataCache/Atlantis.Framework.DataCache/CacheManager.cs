using System.Collections.Generic;

namespace Atlantis.Framework.DataCache
{
  class CacheManager<T>
  {
    Dictionary<string, Cache<T>> _cacheMap;
    SlimLock _cacheLock;

    public CacheManager()
    {
      _cacheMap = new Dictionary<string, Cache<T>>();
      _cacheLock = new SlimLock();
    }

    public Cache<T> GetCache(string cacheName)
    {
      Cache<T> result = null;

      bool cacheFound;
      using (SlimRead read = _cacheLock.GetReadLock())
      {
        cacheFound = _cacheMap.TryGetValue(cacheName, out result);
      }

      if (!cacheFound)
      {
        using (SlimWrite write = _cacheLock.GetWriteLock())
        {
          if (!_cacheMap.TryGetValue(cacheName, out result))
          {
            result = new Cache<T>(cacheName);
            _cacheMap.Add(cacheName, result);
          }
        }
      }

      return result;
    }

    public void ClearCacheData(string cacheName)
    {
      Cache<T> cacheToClear = null;
      using (SlimRead read = _cacheLock.GetReadLock())
      {
        if (_cacheMap.TryGetValue(cacheName, out cacheToClear))
        {
          cacheToClear.Clear();
        }
      }
    }

    public void ClearCacheData(string cacheName, string key)
    {
      Cache<T> cacheToClear = null;
      using (SlimRead read = _cacheLock.GetReadLock())
      {
        if (_cacheMap.TryGetValue(cacheName, out cacheToClear))
        {
          cacheToClear.Clear(key);
        }
      }
    }
  }
}
