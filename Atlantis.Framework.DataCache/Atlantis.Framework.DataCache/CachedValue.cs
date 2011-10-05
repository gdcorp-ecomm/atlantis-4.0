using System;

namespace Atlantis.Framework.DataCache
{
  class CachedValue
  {
    long _finalTicks;
    object _cacheValue;
    string _key;
    bool _isActive;
    int _privateLabelId;
    CachedValueStatus _status;

    public CachedValue(string key, object cacheValue, long finalTicks, int privateLabelId)
    {
      _key = key;
      _cacheValue = cacheValue;
      _finalTicks = finalTicks;
      _isActive = true;
      _privateLabelId = privateLabelId;
      _status = CachedValueStatus.Valid;
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

    public CachedValueStatus Status
    {
      get
      {
        if ((_status == CachedValueStatus.Valid) && ((DateTime.UtcNow.Ticks > _finalTicks)))
        {
          _status = CachedValueStatus.Invalid;
        }
        return _status;
      }
    }

    public void MarkInProgress()
    {
      _status = CachedValueStatus.RefreshInProgress;
    }

    public void MarkInactive()
    {
      _isActive = false;
    }

  }
}
