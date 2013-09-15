using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Preferences;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Preferences
{
  public class ShopperPreferencesProvider : ProviderBase, IShopperPreferencesProvider
  {
    private const string _DBSTATUSSETTING = "ATLANTIS_PREFERENCES_DATABASE";
    private const string _SHOPPERIDPREFKEY = "_sid";
    private readonly Lazy<PreferenceCookie> _preferencesCookie;

    public ShopperPreferencesProvider(IProviderContainer container)
      : base(container)
    {
      _preferencesCookie = new Lazy<PreferenceCookie>(() => { return new PreferenceCookie(Container); });
    }

    public void SaveAllPreferencesToDatabase()
    {
      // this has been obsoleted in the interface
      // shopperid is now ignored on the preferences
      // when the context switches shoppers, the preferences are unaffected.
      // if we want to change preferences on login based on stored data then IDP will have to 'reset' the preferences
    }

    public void UpdatePreference(string key, string value)
    {
      _preferencesCookie.Value.UpdatePreference(key, value);
    }

    public void UpdatePreferences(IDictionary<string, string> values)
    {
      if (values != null)
      {
        List<string> updatedKeys = new List<string>(values.Count);
        foreach (string key in values.Keys)
        {
          _preferencesCookie.Value.UpdatePreference(key, values[key]);
        }
      }
    }

    public string GetPreference(string key, string defaultValueIfNotFound)
    {
      string result;
      if (_preferencesCookie.Value.HasPreference(key))
      {
        result = _preferencesCookie.Value.GetPreference(key);
      }
      else
      {
        result = defaultValueIfNotFound;
      }

      return result;
    }

    public bool HasPreference(string key)
    {
      return _preferencesCookie.Value.HasPreference(key);
    }

  }
}
