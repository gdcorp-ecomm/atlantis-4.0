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

    public CachedValue(string key, object cacheValue, long finalTicks, int privateLabelId)
    {
      _key = key;
      _cacheValue = cacheValue;
      _finalTicks = finalTicks;
      _isActive = true;
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

    public bool IsValidValue
    {
      get
      {
        bool result = false;
        if (DateTime.UtcNow.Ticks <= _finalTicks)
        {
          result = true;
        }
        return result;
      }
    }

    public void MarkInvalid()
    {
      _isActive = false;
    }

  }
}
