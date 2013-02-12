using System.Collections.Generic;

namespace Atlantis.Framework.EcommBillingSync.Impl
{
  /// <summary>
  /// Extension methods for a Dictionary&lt;string, string&gt;
  /// </summary>
  public static class StringDictionaryExtensions
  {
    /// <summary>
    /// Returns a property in a string dictionary as an string value, or a default value.
    /// This extension method is defined in <typeparamref name="StringDictionaryExtensions"/>
    /// </summary>
    /// <param name="dictionary">Dictionary&lt;string, string&gt; to search.</param>
    /// <param name="key">key to search for.</param>
    /// <param name="defaultValue">default value to return if <paramref name="key"/> is not found.</param>
    /// <returns>String value found or <paramref name="defaultValue"/>.</returns>
    public static string GetStringProperty(this Dictionary<string, string> dictionary, string key, string defaultValue)
    {
      string result;
      if (!dictionary.TryGetValue(key, out result))
      {
        result = defaultValue;
      }
      return result;
    }
  }
}
