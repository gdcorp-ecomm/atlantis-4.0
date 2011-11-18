using System;
using System.Collections.Generic;

namespace Atlantis.Framework.EcommClientCertCheck.Impl
{
  internal class EcommClientCertCheckCache
  {
    private readonly IDictionary<string, bool> _cache;
    private readonly TimeSpan _cacheDuration;
    private readonly SlimLock _cacheLock;

    private long _cacheExpirationTicks;
    private bool _isCacheExpired;

    public EcommClientCertCheckCache()
    {
      _cache = new Dictionary<string, bool>(32);
      _cacheDuration = TimeSpan.FromMinutes(10);
      _cacheLock = new SlimLock();

      _cacheExpirationTicks = DateTime.UtcNow.Add(_cacheDuration).Ticks;
    }

    ~EcommClientCertCheckCache()
    {
      _cacheLock.Dispose();
    }

    public void Insert(string cacheKey, bool isAuthorized)
    {
      using(_cacheLock.GetWriteLock())
      {
        _cache[cacheKey] = isAuthorized;
      }
    }

    public bool TryGetValue(string cacheKey, out bool isAuthorized)
    {
      bool found;

      using(_cacheLock.GetReadLock())
      {
        if(DateTime.UtcNow.Ticks > _cacheExpirationTicks)
        {
          _isCacheExpired = true;
        }

        // Even if the cache is expired, it should be ok to grab an expired value if it exists
        found = _cache.TryGetValue(cacheKey, out isAuthorized);
      }

      if (_isCacheExpired)
      {
        using (_cacheLock.GetWriteLock())
        {
          if(_isCacheExpired)
          {
            _isCacheExpired = false;
            _cacheExpirationTicks = DateTime.UtcNow.Add(_cacheDuration).Ticks;
            _cache.Clear();
          }
        }
      }

      return found;
    }
  }
}
