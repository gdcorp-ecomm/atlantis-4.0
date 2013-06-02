using System;

namespace Atlantis.Framework.DataCache
{
  class CachedValue<T>
  {
    long _finalTicks;
    T _cacheValue;
    string _key;
    bool _isActive = true;
    volatile bool _refreshInProgress = false;

    public CachedValue(string key, T cacheValue, long finalTicks)
    {
      _key = key;
      _cacheValue = cacheValue;
      _finalTicks = finalTicks;
    }

    public T Value
    {
      get { return _cacheValue; }
    }

    public long FinalTicks
    {
      get { return _finalTicks; }
    }

    public string Key
    {
      get { return _key; }
    }

    public bool IsActive
    {
      get { return _isActive; }
    }

    public bool IsExpired
    {
      get { return DateTime.UtcNow.Ticks > _finalTicks; }
    }

    public bool RefreshInProgress
    {
      get { return _refreshInProgress; }
    }

    public void MarkInProgress()
    {
      _refreshInProgress = true;
    }

    public void MarkInactive()
    {
      _isActive = false;
    }

  }
}
