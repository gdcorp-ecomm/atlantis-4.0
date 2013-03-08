using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public static class TLDsHelper
  {
    private static HashSet<string> _overrideTlds;
    public static HashSet<string> OverrideTlds
    {
      get
      {
        if (_overrideTlds == null)
        {
          _overrideTlds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

          string stagingServers = DataCache.DataCache.GetAppSetting("DOTTYPECACHE_STAGING_SERVERS");
          string stagingTlds = DataCache.DataCache.GetAppSetting("DOTTYPECACHE_STAGING_TLDS");

          if (!string.IsNullOrEmpty(stagingServers) && !string.IsNullOrEmpty(stagingTlds))
          {
            var serverArr = stagingServers.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (serverArr.Any(s => s.Trim().ToLowerInvariant() == Environment.MachineName.ToLowerInvariant()))
            {
              var stagingTldList = stagingTlds.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

              foreach (var tld in stagingTldList)
              {
                _overrideTlds.Add(tld);
              }
            }
          }
        }
        return _overrideTlds;
      }
    }
  }
}
