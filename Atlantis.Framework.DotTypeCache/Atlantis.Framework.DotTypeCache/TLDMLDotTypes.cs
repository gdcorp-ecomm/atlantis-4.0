using Atlantis.Framework.DotTypeCache.Interface;
using System;
using Atlantis.Framework.DotTypeCache.Static;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  public static class TLDMLDotTypes
  {
    public const string TLDMLSupportedFlag = "tldml_supported";

    internal static bool TLDMLIsAvailable(string dotType, IProviderContainer container)
    {
      bool result = false;

      if (!string.IsNullOrEmpty(dotType))
      {
        try
        {
          var provider = container.Resolve<ITLDDataCacheProvider>();
          var activeTlds = provider.GetActiveTlds();

          if (activeTlds != null)
          {
            result = activeTlds.IsTLDActive(dotType, TLDMLSupportedFlag);
          }
        }
        catch (Exception ex)
        {
          string message = ex.Message + Environment.NewLine + ex.StackTrace;
          Logging.LogException("TLDMLDotTypes.TLDMLIsAvailable", message, dotType);
        }
      }

      return result;
    }

    internal static IDotTypeInfo CreateTLDMLDotTypeIfAvailable(string dotType, IProviderContainer container)
    {
      IDotTypeInfo result = null;

      if (TLDMLIsAvailable(dotType, container))
      {
        try
        {
          result = TLDMLDotTypeInfo.FromDotType(dotType, container);
        }
        catch (Exception ex)
        {
          string message = ex.Message + Environment.NewLine + ex.StackTrace;
          Logging.LogException("TLDMLDottypes.GetTLDMLDotTypeIfAvailable", message, dotType);
        }
      }

      return result;
    }
  }
}
