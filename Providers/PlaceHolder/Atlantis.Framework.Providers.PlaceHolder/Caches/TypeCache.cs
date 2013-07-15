using System;
using System.Collections.Generic;
using Atlantis.Framework.DataCache;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class TypeCache
  {
    private volatile IDictionary<string, Type> _typeCache = new Dictionary<string, Type>(1024);
    private readonly SlimLock _cacheLock;

    internal TypeCache()
    {
      _cacheLock = new SlimLock();
    }

    ~TypeCache()
    {
      if (_cacheLock != null)
      {
        _cacheLock.Dispose();
      }
    }

    internal void SetType(string key, Type value)
    {
      using(_cacheLock.GetWriteLock())
      {
        _typeCache[key] = value;
      }
    }

    internal bool TryGetType(string key, out Type value)
    {
      using(_cacheLock.GetReadLock())
      {
        return _typeCache.TryGetValue(key, out value);
      }
    }
  }
}