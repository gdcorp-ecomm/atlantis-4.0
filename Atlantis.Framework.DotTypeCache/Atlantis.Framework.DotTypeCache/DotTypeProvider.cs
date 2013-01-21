using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache
{
  // Goal is to provide a per-request class to access dottype information
  // May require that the app register the ProviderContainer to use by default
  // default calls would use the default providercontainer
  // allow override to use another provider container?  maybe not initially

  public class DotTypeProvider : ProviderBase, IDotTypeProvider
  {
    private Dictionary<string, IDotTypeInfo> _dotTypesRequestCache;

    public DotTypeProvider(IProviderContainer container)
      : base(container)
    {
      _dotTypesRequestCache = new Dictionary<string, IDotTypeInfo>(100, StringComparer.OrdinalIgnoreCase);
    }

    public IDotTypeInfo InvalidDotType
    {
      get { return StaticDotTypes.InvalidDotType; }
    }

    public IDotTypeInfo GetDotTypeInfo(string dotType)
    {
      if (string.IsNullOrEmpty(dotType))
      {
        return StaticDotTypes.InvalidDotType;
      }

      IDotTypeInfo result;
      if (!_dotTypesRequestCache.TryGetValue(dotType, out result))
      {
        result = LoadDotTypeInfo(dotType);
        _dotTypesRequestCache[dotType] = result;
      }

      return result;
    }

    private IDotTypeInfo LoadDotTypeInfo(string dotType)
    {
      IDotTypeInfo result = TLDMLDotTypes.CreateTLDMLDotTypeIfAvailable(dotType);
      if (result == null)
      {
        result = StaticDotTypes.GetDotType(dotType);
        if (result != StaticDotTypes.InvalidDotType)
        {
          IDotTypeInfo multiRegistryDotType = MultiRegistryStaticDotTypes.GetMultiRegistryDotTypeIfAvailable(dotType);
          if (multiRegistryDotType != null)
          {
            result = multiRegistryDotType;
          }
        }
      }

      return result;
    }

    public bool HasDotTypeInfo(string dotType)
    {
      return TLDMLDotTypes.TLDMLIsAvailable(dotType) || StaticDotTypes.HasDotType(dotType);
    }

    /// Expose TLDDataCache lists
    /// Expose ActiveTLD functionality
  }
}
