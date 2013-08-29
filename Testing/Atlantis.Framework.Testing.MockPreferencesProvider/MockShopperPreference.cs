using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Preferences;
using System.Collections.Generic;

namespace Atlantis.Framework.Testing.MockPreferencesProvider
{
  public class MockShopperPreference : ProviderBase, IShopperPreferencesProvider
  {
    public MockShopperPreference(IProviderContainer container)
      : base(container)
    {
    }

    Dictionary<string, string> _mockPreferences = new Dictionary<string, string>();

    public bool HasPreference(string key)
    {
      return _mockPreferences.ContainsKey(key);
    }

    public string GetPreference(string key, string defaultValueIfNotFound)
    {
      string result;
      if (!_mockPreferences.TryGetValue(key, out result))
      {
        result = defaultValueIfNotFound;
      }
      return result;
    }

    public void UpdatePreference(string key, string value)
    {
      _mockPreferences[key] = value;
    }

    public void UpdatePreferences(IDictionary<string, string> values)
    {
      foreach (string key in values.Keys)
      {
        _mockPreferences[key] = values[key];
      }
    }

    public void SaveAllPreferencesToDatabase()
    {
      return;
    }
  }
}
