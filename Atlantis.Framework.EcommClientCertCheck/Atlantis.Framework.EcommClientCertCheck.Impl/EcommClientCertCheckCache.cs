using System;
using System.Collections.Generic;

namespace Atlantis.Framework.EcommClientCertCheck.Impl
{
  internal class EcommClientCertCheckCache
  {
    private readonly IDictionary<string, EcommClientCertCheckCacheItem> _cache;
    private readonly TimeSpan _cacheItemDuration;
    private readonly SlimLock _cacheLock;

    internal delegate bool IsClientCertAuthorizedRequestDelegate(out bool isAuthorized, out string errorMessage);

    public EcommClientCertCheckCache()
    {
      _cache = new Dictionary<string, EcommClientCertCheckCacheItem>(32);
      _cacheItemDuration = TimeSpan.FromMinutes(10);
      _cacheLock = new SlimLock();
    }

    ~EcommClientCertCheckCache()
    {
      _cacheLock.Dispose();
    }

    public bool TryGetValue(string cacheKey, IsClientCertAuthorizedRequestDelegate isClientCertAuthorized, out bool isAuthorized, out string errorMessage)
    {
      bool cacheItemExists;
      isAuthorized = false;
      errorMessage = string.Empty;

      bool cacheItemExpired = false;

      EcommClientCertCheckCacheItem cacheItem;

      using(_cacheLock.GetReadLock())
      {
        cacheItemExists = _cache.TryGetValue(cacheKey, out cacheItem);
        if (cacheItemExists)
        {
          isAuthorized = cacheItem.IsAuthorized;
          cacheItemExpired = DateTime.UtcNow.Ticks > cacheItem.ExpirationTicks;
        }
      }

      if (!cacheItemExists || cacheItemExpired)
      {
        using (_cacheLock.GetWriteLock())
        {
          if(!_cache.ContainsKey(cacheKey) || DateTime.UtcNow.Ticks > cacheItem.ExpirationTicks)
          {
            bool isAuthorizedFromRequest;
            if(isClientCertAuthorized(out isAuthorizedFromRequest, out errorMessage))
            {
              isAuthorized = isAuthorizedFromRequest;
              cacheItemExists = true;
              _cache[cacheKey] = new EcommClientCertCheckCacheItem { IsAuthorized = isAuthorized, ExpirationTicks = DateTime.UtcNow.Add(_cacheItemDuration).Ticks };
            }
          }
        }
      }

      return cacheItemExists;
    }
  }
}
