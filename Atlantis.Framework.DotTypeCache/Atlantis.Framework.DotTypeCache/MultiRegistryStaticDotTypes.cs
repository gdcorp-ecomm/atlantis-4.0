using Atlantis.Framework.DotTypeCache.Interface;
using System;
using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache
{
  internal static class MultiRegistryStaticDotTypes
  {
    static DateTime _getMultiRegCallFailedOn = DateTime.MinValue;

    internal static IDotTypeInfo GetMultiRegistryDotTypeIfAvailable(string dotType)
    {
      IDotTypeInfo result = null;

      if ((CheckForMultiRegDotType) && (StaticDotTypes.IsDotTypeMultiRegistry(dotType)))
      {
        try
        {
          IDotTypeInfo multiRegistryDotType = DataCache.DataCache.GetCustomCacheData<MultiRegDotTypeInfo>(dotType, MultiRegDotTypeInfo.GetMultiRegDotTypeInfo);
          result = multiRegistryDotType;
        }
        catch (Exception ex)
        {
          string message = ex.Message + Environment.NewLine + ex.StackTrace;
          Logging.LogException("MultiRegistryStaticDotTypes.GetMultiRegistryDotTypeIfAvailable", message, dotType);
          _getMultiRegCallFailedOn = DateTime.Now;
        }
      }

      return result;
    }

    private static bool CheckForMultiRegDotType
    {
      get
      {
        bool result = MultiRegCheckIsOn;

        if (result && _getMultiRegCallFailedOn != DateTime.MinValue)
        {
          TimeSpan span = DateTime.Now.Subtract(_getMultiRegCallFailedOn);
          result = span.TotalSeconds > 60;
        }

        return result;
      }
    }

    private static bool MultiRegCheckIsOn
    {
      get
      {
        return DataCache.DataCache.GetAppSetting("ATLANTIS_DOTTYPECACHE_MULTIREG_ON").Equals("true", StringComparison.OrdinalIgnoreCase);
      }
    }

  }
}
