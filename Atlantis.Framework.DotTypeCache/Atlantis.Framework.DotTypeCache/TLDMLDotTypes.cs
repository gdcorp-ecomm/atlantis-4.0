using Atlantis.Framework.DotTypeCache.Interface;
using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Static;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  public static class TLDMLDotTypes
  {
    public const string TLDMLSupportedFlag = "tldml_supported";

    internal static bool TLDMLIsAvailable(string dotType)
    {
      bool result = false;

      if (!string.IsNullOrEmpty(dotType))
      {
        try
        {
          var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
          var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ActiveTlds);

          if (response != null)
          {
            result = response.IsTLDActive(dotType, TLDMLSupportedFlag);
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

      if (TLDMLIsAvailable(dotType))
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
