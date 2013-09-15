using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Interface.Preferences
{
  /// <summary>
  /// Provides various lightweight shopper preferences
  /// </summary>
  public interface IShopperPreferencesProvider
  {
    /// <summary>
    /// Returns true if a preference value exists
    /// </summary>
    /// <param name="key">preference key</param>
    /// <returns></returns>
    bool HasPreference(string key);

    /// <summary>
    /// Attempts to get a preference based on key.
    /// </summary>
    /// <param name="key">preference key</param>
    /// <param name="defaultValueIfNotFound">default value to return if preference does not exist</param>
    /// <returns>the preference value or the default value if the preference does not exist.</returns>
    string GetPreference(string key, string defaultValueIfNotFound);

    /// <summary>
    /// Updates a preference
    /// </summary>
    /// <param name="key">preference key</param>
    /// <param name="value">value to update the preference to</param>
    void UpdatePreference(string key, string value);

    /// <summary>
    /// Updates multiple preferences
    /// </summary>
    /// <param name="values">Dictionary of preference key value pairs</param>
    void UpdatePreferences(IDictionary<string, string> values);

    [Obsolete("This method no longer does anything. Preferences are autosaved appropriately when UpdatePreference or UpdatePreferences is called.")]
    void SaveAllPreferencesToDatabase();
  }
}
