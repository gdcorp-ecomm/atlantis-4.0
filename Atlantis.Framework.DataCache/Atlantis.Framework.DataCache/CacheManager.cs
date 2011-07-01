using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.DataCache
{
  class CacheManager
  {
    Dictionary<string, Cache> _cacheMap;
    CacheLock _cacheLock;

    Dictionary<string, Cache> _genericCacheDataMap;
    Dictionary<string, Cache> _genericCacheRsMap;
    Dictionary<string, string> _genericCacheNames;
    CacheLock _genericCachesLock;

    public CacheManager()
    {
      _cacheMap = new Dictionary<string, Cache>();
      _cacheLock = new CacheLock();

      _genericCacheDataMap = new Dictionary<string, Cache>();
      _genericCacheRsMap = new Dictionary<string, Cache>();
      _genericCacheNames = new Dictionary<string, string>();
      _genericCachesLock = new CacheLock();

      ReloadGenericCaches();
    }

    public Cache GetGenericDataCache(string cacheName)
    {
      return GetGenericCache(cacheName, _genericCacheDataMap);
    }

    public Cache GetGenericRsCache(string cacheName)
    {
      return GetGenericCache(cacheName, _genericCacheRsMap);
    }

    private Cache GetGenericCache(string cacheName, Dictionary<string, Cache> genericCacheMap)
    {
      Cache result = null;

      try
      {
        _cacheLock.GetReaderLock();
        if (!genericCacheMap.TryGetValue(cacheName, out result))
        {
          try
          {
            _cacheLock.GetWriterLock();
            if (!genericCacheMap.TryGetValue(cacheName, out result))
            {
              string privateLabelIdName = string.Empty;

              try
              {
                _genericCachesLock.GetReaderLock();
                if (_genericCacheNames.TryGetValue(cacheName, out privateLabelIdName))
                  result = new Cache(cacheName, privateLabelIdName);
                else
                  result = new Cache(cacheName, false);
              }
              finally
              {
                _genericCachesLock.ReleaseReaderLock();
              }

              genericCacheMap.Add(cacheName, result);
            }
          }
          finally
          {
            _cacheLock.ReleaseWriterLock();
          }

        }
      }
      finally
      {
        _cacheLock.ReleaseReaderLock();
      }

      return result;
    }

    public Cache GetCache(string cacheName, bool isBasedOnPrivateLabelId)
    {
      Cache result = null;

      try
      {
        _cacheLock.GetReaderLock();
        if (!_cacheMap.TryGetValue(cacheName, out result))
        {
          try
          {
            _cacheLock.GetWriterLock();
            if (!_cacheMap.TryGetValue(cacheName, out result))
            {
              result = new Cache(cacheName, isBasedOnPrivateLabelId);
              _cacheMap.Add(cacheName, result);
            }
          }
          finally
          {
            _cacheLock.ReleaseWriterLock();
          }
        }
      }
      finally
      {
        _cacheLock.ReleaseReaderLock();
      }

      return result;
    }

    public void ReloadGenericCaches()
    {
      string genericCachesXML = string.Empty;

      try
      {
        using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
        {
          genericCachesXML = oCacheWrapper.COMAccessClass.GetGenericCaches();
        }

        XmlDocument xdDoc = new XmlDocument();

        xdDoc.LoadXml(genericCachesXML);

        XmlNodeList xnlCaches = xdDoc.SelectNodes("/GenericCaches/Cache");

        _genericCachesLock.GetWriterLock();
        _genericCacheNames.Clear();
        foreach (XmlElement xlCache in xnlCaches)
        {
          _genericCacheNames.Add(xlCache.GetAttribute("name"),
                               xlCache.GetAttribute("plid_name"));
        }
      }
      finally
      {
        _genericCachesLock.ReleaseWriterLock();
      }
    }

    public void ClearCacheDataByPLID(string cacheName, HashSet<int> privateLabelIds)
    {
      Cache oCache = null;

      try
      {
        _cacheLock.GetReaderLock();

        if (_cacheMap.TryGetValue(cacheName, out oCache))
          oCache.ClearByPLID(privateLabelIds);
        if (_genericCacheDataMap.TryGetValue(cacheName, out oCache))
          oCache.ClearByPLID(privateLabelIds);
        if (_genericCacheRsMap.TryGetValue(cacheName, out oCache))
          oCache.ClearByPLID(privateLabelIds);
      }
      finally
      {
        _cacheLock.ReleaseReaderLock();
      }

    }

    public void ClearCacheAllCachesByPLID(HashSet<int> privateLabelIds)
    {
      try
      {
        _cacheLock.GetWriterLock();

        foreach (KeyValuePair<string, Cache> oPair in _cacheMap)
          oPair.Value.ClearByPLID(privateLabelIds);
        foreach (KeyValuePair<string, Cache> oPair in _genericCacheDataMap)
          oPair.Value.ClearByPLID(privateLabelIds);
        foreach (KeyValuePair<string, Cache> oPair in _genericCacheRsMap)
          oPair.Value.ClearByPLID(privateLabelIds);
      }
      finally
      {
        _cacheLock.ReleaseWriterLock();
      }

    }

    public void ClearCacheData(string cacheName)
    {
      Cache oCache = null;
      try
      {
        _cacheLock.GetWriterLock();

        if (_cacheMap.TryGetValue(cacheName, out oCache))
          oCache.Clear();
        if (_genericCacheDataMap.TryGetValue(cacheName, out oCache))
          oCache.Clear();
        if (_genericCacheRsMap.TryGetValue(cacheName, out oCache))
          oCache.Clear();
      }
      finally
      {
        _cacheLock.ReleaseWriterLock();
      }
    }

    public void ClearAllCaches()
    {
      try
      {
        _cacheLock.GetWriterLock();

        foreach (KeyValuePair<string, Cache> oPair in _cacheMap)
          oPair.Value.Clear();
        foreach (KeyValuePair<string, Cache> oPair in _genericCacheDataMap)
          oPair.Value.Clear();
        foreach (KeyValuePair<string, Cache> oPair in _genericCacheRsMap)
          oPair.Value.Clear();
      }
      finally
      {
        _cacheLock.ReleaseWriterLock();
      }

    }

    public string DisplayCache(string cacheName)
    {
      Cache oCache = null;
      StringBuilder sb = new StringBuilder();
      sb.Append("<Caches>");

      try
      {
        _cacheLock.GetReaderLock();

        if (_cacheMap.TryGetValue(cacheName, out oCache))
          sb.Append(oCache.Display());
        if (_genericCacheDataMap.TryGetValue(cacheName, out oCache))
          sb.Append(oCache.Display());
        if (_genericCacheRsMap.TryGetValue(cacheName, out oCache))
          sb.Append(oCache.Display());
      }
      finally
      {
        _cacheLock.ReleaseReaderLock();
      }

      sb.Append("</Caches>");
      return sb.ToString();
    }

    public string GetStats()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("<ManagerStats>");

      try
      {
        _cacheLock.GetReaderLock();

        foreach (KeyValuePair<string, Cache> oPair in _cacheMap)
          sb.Append(oPair.Value.GetStats());
        foreach (KeyValuePair<string, Cache> oPair in _genericCacheDataMap)
          sb.Append(oPair.Value.GetStats());
        foreach (KeyValuePair<string, Cache> oPair in _genericCacheRsMap)
          sb.Append(oPair.Value.GetStats());

      }
      finally
      {
        _cacheLock.ReleaseReaderLock();
      }

      sb.Append("</ManagerStats>");
      return sb.ToString();
    }

  }
}
