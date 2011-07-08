using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Preferences
{
  public class ValidPreferenceKeys
  {
    private HashSet<string> _validPreferenceKeys = new HashSet<string>();

    private ValidPreferenceKeys()
    {
      string delimitedKeys = DataCache.DataCache.GetAppSetting("ATLANTIS_PREFERENCES_VALID_KEYS");
      if (!string.IsNullOrEmpty(delimitedKeys))
      {
        string[] keyArray = delimitedKeys.Split(',');
        foreach (string key in keyArray)
        {
          _validPreferenceKeys.Add(key);
        }
      }
    }

    public bool IsPreferenceKeyValid(string preferenceKey)
    {
      return _validPreferenceKeys.Contains(preferenceKey);
    }

    private static ValidPreferenceKeys GetValidPreferenceKeys(string key)
    {
      return new ValidPreferenceKeys();
    }

    public static ValidPreferenceKeys LoadValidPreferenceKeysFromCache()
    {
      return DataCache.DataCache.GetCustomCacheData<ValidPreferenceKeys>("ValidPreferenceKeys", GetValidPreferenceKeys);
    }
  }
}
