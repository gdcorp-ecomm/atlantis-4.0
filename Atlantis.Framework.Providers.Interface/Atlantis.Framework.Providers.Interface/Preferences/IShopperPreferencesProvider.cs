using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Interface.Preferences
{
  public interface IShopperPreferencesProvider
  {
    bool HasPreference(string key);
    string GetPreference(string key, string defaultValueIfNotFound);
    void UpdatePreference(string key, string value);
    void UpdatePreferences(IDictionary<string, string> values);
    void SaveAllPreferencesToDatabase();
  }
}
