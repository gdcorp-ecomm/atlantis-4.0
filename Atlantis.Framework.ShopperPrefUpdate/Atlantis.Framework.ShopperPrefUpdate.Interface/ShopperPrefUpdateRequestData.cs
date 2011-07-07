using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperPrefUpdate.Interface
{
  public class ShopperPrefUpdateRequestData : RequestData
  {

    private Dictionary<string, string> _preferences = new Dictionary<string,string>();

    public ShopperPrefUpdateRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public Dictionary<string, string>.KeyCollection GetPreferenceKeys()
    {
      return _preferences.Keys;
    }

    public void AddPreference(string name, string value)
    {
      _preferences[name] = value;
    }

    public string GetPreference(string name)
    {
      string value;
      if (!_preferences.TryGetValue(name, out value))
      {
        value = null;
      }
      return value;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ShopperPrefUpdateRequestData");     
    }


  }
}
