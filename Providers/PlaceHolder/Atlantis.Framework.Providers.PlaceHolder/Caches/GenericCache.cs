using System.Collections.Generic;
using Atlantis.Framework.DataCache;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class GenericCache<T>
  {
    private volatile IDictionary<string, T> _cache = new Dictionary<string, T>(1024);
    private readonly SlimLock _cacheLock;

    internal GenericCache()
    {
      _cacheLock = new SlimLock();
    }

    ~GenericCache()
    {
      if (_cacheLock != null)
      {
        _cacheLock.Dispose();
      }
    }

    internal void Set(string key, T value)
    {
      using(_cacheLock.GetWriteLock())
      {
        _cache[key] = value;
      }
    }

    internal bool TryGet(string key, out T value)
    {
      using(_cacheLock.GetReadLock())
      {
        return _cache.TryGetValue(key, out value);
      }
    }
  }
}