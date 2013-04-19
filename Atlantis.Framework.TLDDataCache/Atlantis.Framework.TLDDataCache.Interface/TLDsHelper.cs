using System;
using System.Collections.Generic;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public static class TLDsHelper
  {
    public static HashSet<string> OverrideTlds()
    {
      var overrideTlds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      string stagingServers = DataCache.DataCache.GetAppSetting("DOTTYPECACHE_STAGING_SERVERS");
      string stagingTlds = DataCache.DataCache.GetAppSetting("DOTTYPECACHE_STAGING_TLDS");

      if (!string.IsNullOrEmpty(stagingServers) && !string.IsNullOrEmpty(stagingTlds))
      {
        var serverArr = stagingServers.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var serverName in serverArr)
        {
          if (Environment.MachineName.Equals(serverName, StringComparison.OrdinalIgnoreCase))
          {
            var stagingTldList = stagingTlds.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            overrideTlds = new HashSet<string>(stagingTldList, StringComparer.OrdinalIgnoreCase);
          }
        }
      }

      return overrideTlds;
    }
  }
}
