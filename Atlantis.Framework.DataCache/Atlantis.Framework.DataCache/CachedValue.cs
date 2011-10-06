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
    SlimLock _statusLock;

    public CachedValue(string key, object cacheValue, long finalTicks, int privateLabelId)
    {
      _key = key;
      _cacheValue = cacheValue;
      _finalTicks = finalTicks;
      _isActive = true;
      _privateLabelId = privateLabelId;
      _status = CachedValueStatus.Valid;
      _statusLock = new SlimLock();
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
        CachedValueStatus tempStatus;
        using (SlimRead read = _statusLock.GetReadLock())
        {
          tempStatus = _status;
        }
        
        if ((tempStatus == CachedValueStatus.Valid) && ((DateTime.UtcNow.Ticks > _finalTicks)))
        {
          using (SlimWrite write = _statusLock.GetWriteLock())
          {
            if (_status == CachedValueStatus.Valid)
            {
              _status = CachedValueStatus.Invalid;
            }
          }
        }
        return _status;
      }
    }

    public void MarkInProgress()
    {
      using (SlimWrite write = _statusLock.GetWriteLock())
      {
        _status = CachedValueStatus.RefreshInProgress;
      }
    }

    public void MarkInactive()
    {
      _isActive = false;
    }

  }
}
