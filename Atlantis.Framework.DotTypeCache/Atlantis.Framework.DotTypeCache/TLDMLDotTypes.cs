using Atlantis.Framework.DotTypeCache.Interface;
using System;

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
          var dotTypeAttributesDictionary = DataCache.DataCache.GetExtendedTLDData(dotType);
          var dotTypeAttributes = dotTypeAttributesDictionary[dotType];
          if ((dotTypeAttributes != null) && (dotTypeAttributes.ContainsKey(TLDMLSupportedFlag)))
          {
            result = dotTypeAttributes[TLDMLSupportedFlag] != "0";
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

    internal static IDotTypeInfo CreateTLDMLDotTypeIfAvailable(string dotType)
    {
      IDotTypeInfo result = null;

      if (TLDMLIsAvailable(dotType))
      {
        try
        {
          result = TLDMLDotTypeInfo.FromDotType(dotType);
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
