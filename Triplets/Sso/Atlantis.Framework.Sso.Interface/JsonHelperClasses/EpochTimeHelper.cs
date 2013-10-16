using System;

namespace Atlantis.Framework.Sso.Interface.JsonHelperClasses
{
  public static class EpochTimeHelper
  {
    public static DateTime FromUnixEpoch(string ticks)
    {
      long convertedTicks = 0;

      long.TryParse(ticks, out convertedTicks);

      if (convertedTicks < 0)
      {
        convertedTicks = 0;
      }

      var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      var epochDateTime = epoch.AddSeconds(convertedTicks);

      return epochDateTime;
    }
  }
}
