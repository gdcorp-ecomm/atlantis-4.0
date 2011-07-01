using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataCache
{
  class Cache
  {
    const int MAX_CLEAN = 100;
    Dictionary<string, CachedValue> _cachedValuesDictionary;
    LinkedList<object> _cachedValuesLinkList;

    CacheLock _cacheLock;
    string _cacheName;
    TimeSpan _itemCacheTime;
    int _iHit = 0;
    int _iMiss = 0;
    DateTime _cacheCreateTime;
    string _privateLabelIdName;
    bool _isBasedOnPrivateLabelId;

    private TimeSpan DefaultItemCacheTime
    {
      get
      {
        TimeSpan result;
#if DEBUG
        result = TimeSpan.FromMinutes(5);
#else
        result = TimeSpan.FromMinutes(10);
#endif
        return result;
      }
    }

    public Cache(string cacheName, string privateLabelIdName)
    {
      _cachedValuesDictionary = new Dictionary<string, CachedValue>();
      _cachedValuesLinkList = new LinkedList<object>();

      _cacheName = cacheName;
      _privateLabelIdName = privateLabelIdName;
      _isBasedOnPrivateLabelId = true;
      _cacheLock = new CacheLock();
      _itemCacheTime = DefaultItemCacheTime;
      _cacheCreateTime = DateTime.UtcNow;
    }

    public Cache(string cacheName, string privateLabelIdName, TimeSpan itemCacheTime)
      : this(cacheName, privateLabelIdName)
    {
      _itemCacheTime = itemCacheTime;
    }

    public Cache(string cacheName, bool isBasedOnPrivateLabelId)
      : this(cacheName, null)
    {
      _isBasedOnPrivateLabelId = isBasedOnPrivateLabelId;
    }

    public Cache(string cacheName, bool isBasedOnPrivateLabelId, TimeSpan itemCacheTime)
      : this(cacheName, null)
    {
      _isBasedOnPrivateLabelId = isBasedOnPrivateLabelId;
      _itemCacheTime = itemCacheTime;
    }

    public string PrivateLabelIdName
    {
      get { return _privateLabelIdName; }
    }

    public bool IsBasedOnPrivateLabelId
    {
      get { return _isBasedOnPrivateLabelId; }
    }

    public string GetStats()
    {
      TimeSpan ts = DateTime.UtcNow - _cacheCreateTime;
      int seconds = (int)ts.TotalSeconds + 1;

      int iHit = Interlocked.CompareExchange(ref _iHit, 0, 0);
      int iMiss = Interlocked.CompareExchange(ref _iMiss, 0, 0);

      StringBuilder sb = new StringBuilder();
      sb.Append("<Stats><CacheName>");
      sb.Append(_cacheName);
      sb.Append("</CacheName><CacheCreate>");
      sb.Append(_cacheCreateTime.ToLongTimeString());
      sb.Append("</CacheCreate><HitCount>");
      sb.Append(iHit.ToString());
      sb.Append("</HitCount><MissCount>");
      sb.Append(iMiss.ToString());
      sb.Append("</MissCount><HitPerSecond>");
      sb.Append(((iHit / seconds)).ToString());
      sb.Append("</HitPerSecond><MissPerSecond>");
      sb.Append(((iMiss / seconds)).ToString());
      sb.Append("</MissPerSecond></Stats>");
      return sb.ToString();
    }

    public bool TryGetValue(string key, out object cachedValue)
    {
      CachedValue tempCachedValue = null;
      bool isValid = false;
      bool foundValue = false;

      try
      {
        _cacheLock.GetReaderLock();
        foundValue = _cachedValuesDictionary.TryGetValue(key, out tempCachedValue);
        _cacheLock.ReleaseReaderLock();

        if (foundValue && tempCachedValue != null)
          isValid = tempCachedValue.IsValidValue;

        if (isValid && foundValue)
          Interlocked.Increment(ref _iHit);
        else
          Interlocked.Increment(ref _iMiss);

        cachedValue = tempCachedValue;
      }
      catch (Exception ex)
      {
        _cacheLock.ReleaseReaderLock();
        LogError(_cacheName, key, ex);
        cachedValue = null;
        isValid = false;
      }

      return isValid && foundValue;
    }

    public void RenewValue(CachedValue cachedValue)
    {
      try
      {
        _cacheLock.GetWriterLock();

        cachedValue.MarkInvalid();
        _cachedValuesDictionary.Remove(cachedValue.Key);

        if (cachedValue.PrivateLabelId != 0)
          AddValue(cachedValue.Key, cachedValue.Value, cachedValue.PrivateLabelId);
        else
          AddValue(cachedValue.Key, cachedValue.Value, 0);
      }
      catch (Exception ex)
      {
        LogError(_cacheName, cachedValue.Key, ex);
      }
      finally
      {
        _cacheLock.ReleaseWriterLock(); // Clean-up lock
      }
    }

    public void QuickClean(CachedValue cachedValue)
    {
      long timeKey = 0;
      int maxCheck = 0;

      try
      {
        timeKey = DateTime.UtcNow.Ticks;

        _cacheLock.GetWriterLock();

        maxCheck = System.Math.Min(MAX_CLEAN, (int)((_cachedValuesLinkList.Count * .05) + 1));

        if (cachedValue.IsActive)
        {
          cachedValue.MarkInvalid();
          _cachedValuesDictionary.Remove(cachedValue.Key);
        }

        LinkedListNode<object> oNode = _cachedValuesLinkList.First;
        int i = 0;
        bool bExit = false;
        while (i < maxCheck && oNode != null && !bExit)
        {
          WeakReference oWeakRef = (WeakReference)oNode.Value;
          LinkedListNode<object> oNextNode = oNode.Next;

          if (oWeakRef.Target == null || !(((CachedValue)oWeakRef.Target).IsActive))
          {
            _cachedValuesLinkList.Remove(oNode);
          }
          else if (((CachedValue)oWeakRef.Target).FinalTicks < timeKey)
          {
            ((CachedValue)oWeakRef.Target).MarkInvalid();
            _cachedValuesDictionary.Remove(((CachedValue)oWeakRef.Target).Key);
            _cachedValuesLinkList.Remove(oNode);
          }
          else
            bExit = true;

          oNode = oNextNode;
          i++;
        }
      }
      catch (Exception ex)
      {
        LogError(_cacheName, cachedValue.Key, ex);
      }
      finally
      {
        _cacheLock.ReleaseWriterLock(); // Clean-up lock
      }
    }

    // WeakReference - http://msdn2.microsoft.com/en-us/library/ms404247.aspx
    public void AddValue(string key, object cacheValue, int privateLabelId)
    {
      DateTime finalExpiration = DateTime.UtcNow + _itemCacheTime;
      CachedValue oCachedValue = new CachedValue(key, cacheValue, finalExpiration.Ticks, privateLabelId);

      try
      {
        _cacheLock.GetWriterLock();
        if (!_cachedValuesDictionary.ContainsKey(key))
        {
          _cachedValuesDictionary.Add(key, oCachedValue);
          _cachedValuesLinkList.AddLast(new WeakReference(oCachedValue));
        }
      }
      finally
      {
        _cacheLock.ReleaseWriterLock();
      }
    }

    public void AddValue(string key, object cacheValue)
    {
      AddValue(key, cacheValue, 0);
    }

    public void ClearByPLID(HashSet<int> privateLabelIds)
    {
      try
      {
        _cacheLock.GetWriterLock();
        if (privateLabelIds.Count > 0)
        {
          long timeKey = DateTime.UtcNow.Ticks;
          LinkedListNode<object> oNode = _cachedValuesLinkList.First;

          while (oNode != null)
          {
            WeakReference oWeakRef = (WeakReference)oNode.Value;
            LinkedListNode<object> oNextNode = oNode.Next;

            if (oWeakRef.Target == null || !(((CachedValue)oWeakRef.Target).IsActive))
            {
              _cachedValuesLinkList.Remove(oNode);
            }
            else if (((CachedValue)oWeakRef.Target).FinalTicks < timeKey ||
                     ((CachedValue)oWeakRef.Target).PrivateLabelId == 0 ||
                     privateLabelIds.Contains(((CachedValue)oWeakRef.Target).PrivateLabelId))
            {
              ((CachedValue)oWeakRef.Target).MarkInvalid();
              _cachedValuesDictionary.Remove(((CachedValue)oWeakRef.Target).Key);
              _cachedValuesLinkList.Remove(oNode);
            }

            oNode = oNextNode;
          }
        }
        else
        {
          _cachedValuesDictionary.Clear();
          _cachedValuesLinkList.Clear();
        }
      }
      catch (Exception ex)
      {
        LogError("Cache.ClearByPLID(): " + _cacheName, "", ex);
      }
      finally
      {
        _cacheLock.ReleaseWriterLock();
      }
    }

    public void Clear()
    {
      try
      {
        _cacheLock.GetWriterLock();
        _cachedValuesLinkList.Clear();
        _cachedValuesDictionary.Clear();
      }
      finally
      {
        _cacheLock.ReleaseWriterLock();
      }
    }

    virtual public string Display()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));
      xtwRequest.WriteStartElement("Cache");
      xtwRequest.WriteAttributeString("MethodName", _cacheName);

      try
      {
        _cacheLock.GetReaderLock();
        foreach (KeyValuePair<string, CachedValue> oPair in _cachedValuesDictionary)
        {
          xtwRequest.WriteStartElement("Data");
          xtwRequest.WriteAttributeString("Key", oPair.Key);
          xtwRequest.WriteAttributeString("Value", ((CachedValue)oPair.Value).Value.ToString());
          xtwRequest.WriteEndElement();
        }
      }
      finally
      {
        _cacheLock.ReleaseReaderLock();
      }

      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

    private void LogError(string cacheName, string key, Exception ex)
    {
      if (typeof(Exception) != typeof(ThreadAbortException))
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        string source = cacheName + ":" + key;
        AtlantisException aex = new AtlantisException(source, "0", message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }
    }

  }
}
