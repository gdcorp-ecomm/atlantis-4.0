using System;

namespace Atlantis.Framework.DataCache
{
  class CachedValue
  {
    long _finalTicks;
    object _cacheValue;
    string _key;
    bool _isActive = true;
    int _privateLabelId;
    volatile bool _refreshInProgress = false;

    public CachedValue(string key, object cacheValue, long finalTicks, int privateLabelId)
    {
      _key = key;
      _cacheValue = cacheValue;
      _finalTicks = finalTicks;
      _privateLabelId = privateLabelId;
    }

    public object Value
    {
      get { return _cacheValue; }
    }

    public long FinalTicks
    {
      get { return _finalTicks; }
    }

    public int PrivateLabelId
    {
      get { return _privateLabelId; }
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
