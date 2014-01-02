using System;
using System.Collections.Generic;
using System.Globalization;

namespace Atlantis.Framework.Providers.Localization
{
  public static class SupportedCultures
  {
    private static Dictionary<string, CultureInfo> _cultures;

    static SupportedCultures()
    {
      _cultures = new Dictionary<string, CultureInfo>(StringComparer.OrdinalIgnoreCase);
      foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
      {
        _cultures[culture.Name] = culture;
      }
    }

    public static CultureInfo GetByName(string cultureName)
    {
      CultureInfo result;
      _cultures.TryGetValue(cultureName, out result);
      return result;
    }

    public static bool IsSupportedCulture(string cultureName)
    {
      return _cultures.ContainsKey(cultureName);
    }
  }
}
