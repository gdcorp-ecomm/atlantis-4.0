using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Atlantis.Framework.DataCache
{
  class Cache<T>
  {
    static TimeSpan _DEFAULTITEMCACHETIME = TimeSpan.FromMinutes(10);
    const int MAX_CLEAN = 100;

    Dictionary<string, CachedValue<T>> _cachedValuesDictionary;
    LinkedList<object> _cachedValuesLinkList;

    SlimLock _cacheLock;
    string _cacheName;
    TimeSpan _itemCacheTime;
    DateTime _cacheCreateTime;

    private TimeSpan DefaultItemCacheTime
    {
      get { return _DEFAULTITEMCACHETIME; }
    }

    internal Cache(string cacheName)
    {
      _cachedValuesDictionary = new Dictionary<string, CachedValue<T>>();
      _cachedValuesLinkList = new LinkedList<object>();

      _cacheName = cacheName;
      _cacheLock = new SlimLock();
      _itemCacheTime = DefaultItemCacheTime;
      _cacheCreateTime = DateTime.UtcNow;
    }

    public bool TryGetValue(string key, out CachedValue<T> cachedValue)
    {
      cachedValue = null;
      bool isFresh = false;
      bool foundValue = false;

      try
      {
        using (SlimRead read = _cacheLock.GetReadLock())
        {
          foundValue = _cachedValuesDictionary.TryGetValue(key, out cachedValue);
        }

        if (foundValue && cachedValue != null)
        {
          isFresh = !cachedValue.IsExpired || cachedValue.RefreshInProgress;
        }
      }
      catch (Exception ex)
      {
        LogError(_cacheName, key, ex);
        cachedValue = null;
        isFresh = false;
      }

      return isFresh;
    }

    public void RenewValue(CachedValue<T> cachedValue)
    {
      try
      {
        using (SlimWrite write = _cacheLock.GetWriteLock())
        {
          cachedValue.MarkInactive();

          DateTime finalExpiration = DateTime.UtcNow + _itemCacheTime;
          CachedValue<T> renewedCacheValue = new CachedValue<T>(cachedValue.Key, cachedValue.Value, finalExpiration.Ticks);
          _cachedValuesDictionary[renewedCacheValue.Key] = renewedCacheValue;
          _cachedValuesLinkList.AddLast(new WeakReference(renewedCacheValue));
        }
      }
      catch (Exception ex)
      {
        LogError(_cacheName, cachedValue.Key, ex);
      }
    }

    private void QuickClean(CachedValue<T> oldCachedValue)
    {
      try
      {
        int maxCheck = System.Math.Min(MAX_CLEAN, (int)((_cachedValuesLinkList.Count * .05) + 1));
        long timeKeyFiveHoursAgo = (DateTime.UtcNow.AddHours(-5)).Ticks;

        if (oldCachedValue != null)
        {
          oldCachedValue.MarkInactive();
        }

        LinkedListNode<object> oNode = _cachedValuesLinkList.First;
        int i = 0;
        bool bExit = false;
        while (i < maxCheck && oNode != null && !bExit)
        {
          WeakReference oWeakRef = (WeakReference)oNode.Value;
          LinkedListNode<object> oNextNode = oNode.Next;

          CachedValue<T> targetCachedValue = oWeakRef.Target as CachedValue<T>;
          if ((targetCachedValue == null) || (!targetCachedValue.IsActive))
          {
            _cachedValuesLinkList.Remove(oNode);
          }
          else if (targetCachedValue.FinalTicks < timeKeyFiveHoursAgo)
          {
            _cachedValuesLinkList.Remove(oNode);
            _cachedValuesDictionary.Remove(targetCachedValue.Key);
          }
          else
          {
            bExit = true;
          }

          oNode = oNextNode;
          i++;
        }
      }
      catch (Exception ex)
      {
        LogError(_cacheName, "QuickClean", ex);
      }
    }

    // WeakReference - http://msdn2.microsoft.com/en-us/library/ms404247.aspx
    public void AddValue(string key, T cacheValue, CachedValue<T> oldCachedValue)
    {
      DateTime finalExpiration = DateTime.UtcNow + _itemCacheTime;
      CachedValue<T> newCachedValue = new CachedValue<T>(key, cacheValue, finalExpiration.Ticks);

      using (SlimWrite write = _cacheLock.GetWriteLock())
      {
        QuickClean(oldCachedValue);
        _cachedValuesDictionary[newCachedValue.Key] = newCachedValue;
        _cachedValuesLinkList.AddLast(new WeakReference(newCachedValue));
      }
    }    

    public void Clear()
    {
      using (SlimWrite write = _cacheLock.GetWriteLock())
      {
        _cachedValuesLinkList.Clear();
        _cachedValuesDictionary.Clear();
      }
    }

    public void Clear(string key)
    {
      CachedValue<T> cachedValue;

      using (SlimWrite write = _cacheLock.GetWriteLock())
      {
        if (_cachedValuesDictionary.TryGetValue(key, out cachedValue))
        {
          QuickClean(cachedValue);
          _cachedValuesDictionary.Remove(key);
        }
      }
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
