using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.DataCache
{
  class CacheManager
  {
    Dictionary<string, Cache> _cacheMap;
    SlimLock _cacheLock;

    Dictionary<string, Cache> _genericCacheDataMap;
    Dictionary<string, Cache> _genericCacheRsMap;
    Dictionary<string, string> _genericCacheNames;
    SlimLock _genericCachesLock;

    public CacheManager()
    {
      _cacheMap = new Dictionary<string, Cache>();
      _cacheLock = new SlimLock();

      _genericCacheDataMap = new Dictionary<string, Cache>();
      _genericCacheRsMap = new Dictionary<string, Cache>();
      _genericCacheNames = new Dictionary<string, string>();
      _genericCachesLock = new SlimLock();

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

      bool cacheFound;
      using (SlimRead read = _cacheLock.GetReadLock())
      {
        cacheFound = genericCacheMap.TryGetValue(cacheName, out result);
      }

      if (!cacheFound)
      {
        using (SlimWrite write = _cacheLock.GetWriteLock())
        {
          if (!genericCacheMap.TryGetValue(cacheName, out result))
          {
            string privateLabelIdName = string.Empty;

            using (SlimRead genericRead = _genericCachesLock.GetReadLock())
            {
              if (_genericCacheNames.TryGetValue(cacheName, out privateLabelIdName))
                result = new Cache(cacheName, privateLabelIdName);
              else
                result = new Cache(cacheName, false);
            }

            genericCacheMap.Add(cacheName, result);
          }
        }
      }

      return result;
    }

    public Cache GetCache(string cacheName, bool isBasedOnPrivateLabelId)
    {
      Cache result = null;

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
            result = new Cache(cacheName, isBasedOnPrivateLabelId);
            _cacheMap.Add(cacheName, result);
          }
        }
      }

      return result;
    }

    public void ReloadGenericCaches()
    {
      string genericCachesXML = string.Empty;

      using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
      {
        genericCachesXML = oCacheWrapper.COMAccessClass.GetGenericCaches();
      }

      XmlDocument xdDoc = new XmlDocument();

      xdDoc.LoadXml(genericCachesXML);

      XmlNodeList xnlCaches = xdDoc.SelectNodes("/GenericCaches/Cache");

      using (SlimWrite write = _genericCachesLock.GetWriteLock())
      {
        _genericCacheNames.Clear();
        foreach (XmlElement xlCache in xnlCaches)
        {
          _genericCacheNames.Add(xlCache.GetAttribute("name"),
                                xlCache.GetAttribute("plid_name"));
        }
      }
    }

    public void ClearCacheDataByPLID(string cacheName, HashSet<int> privateLabelIds)
    {
      Cache oCache = null;

      using (SlimRead read = _cacheLock.GetReadLock())
      {
        if (_cacheMap.TryGetValue(cacheName, out oCache))
          oCache.ClearByPLID(privateLabelIds);
        if (_genericCacheDataMap.TryGetValue(cacheName, out oCache))
          oCache.ClearByPLID(privateLabelIds);
        if (_genericCacheRsMap.TryGetValue(cacheName, out oCache))
          oCache.ClearByPLID(privateLabelIds);
      }
    }

    public void ClearCacheAllCachesByPLID(HashSet<int> privateLabelIds)
    {
      using (SlimRead read = _cacheLock.GetReadLock())
      {
        foreach (KeyValuePair<string, Cache> oPair in _cacheMap)
          oPair.Value.ClearByPLID(privateLabelIds);
        foreach (KeyValuePair<string, Cache> oPair in _genericCacheDataMap)
          oPair.Value.ClearByPLID(privateLabelIds);
        foreach (KeyValuePair<string, Cache> oPair in _genericCacheRsMap)
          oPair.Value.ClearByPLID(privateLabelIds);
      }
    }

    public void ClearCacheData(string cacheName)
    {
      Cache oCache = null;
      using (SlimRead read = _cacheLock.GetReadLock())
      {
        if (_cacheMap.TryGetValue(cacheName, out oCache))
          oCache.Clear();
        if (_genericCacheDataMap.TryGetValue(cacheName, out oCache))
          oCache.Clear();
        if (_genericCacheRsMap.TryGetValue(cacheName, out oCache))
          oCache.Clear();
      }
    }

    public string DisplayCache(string cacheName)
    {
      Cache oCache = null;
      StringBuilder sb = new StringBuilder();
      sb.Append("<Caches>");

      using (SlimRead read = _cacheLock.GetReadLock())
      {
        if (_cacheMap.TryGetValue(cacheName, out oCache))
          sb.Append(oCache.Display());
        if (_genericCacheDataMap.TryGetValue(cacheName, out oCache))
          sb.Append(oCache.Display());
        if (_genericCacheRsMap.TryGetValue(cacheName, out oCache))
          sb.Append(oCache.Display());
      }

      sb.Append("</Caches>");
      return sb.ToString();
    }

    public string GetStats()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("<ManagerStats>");

      using (SlimRead read = _cacheLock.GetReadLock())
      {
        foreach (KeyValuePair<string, Cache> oPair in _cacheMap)
          sb.Append(oPair.Value.GetStats());
        foreach (KeyValuePair<string, Cache> oPair in _genericCacheDataMap)
          sb.Append(oPair.Value.GetStats());
        foreach (KeyValuePair<string, Cache> oPair in _genericCacheRsMap)
          sb.Append(oPair.Value.GetStats());
      }

      sb.Append("</ManagerStats>");
      return sb.ToString();
    }

  }
}
