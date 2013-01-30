using System.Globalization;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using Atlantis.Framework.TLDDataCache.Interface;
using System.Linq;

namespace Atlantis.Framework.DotTypeCache
{
  // Goal is to provide a per-request class to access dottype information
  // May require that the app register the ProviderContainer to use by default
  // default calls would use the default providercontainer
  // allow override to use another provider container?  maybe not initially

  public class DotTypeProvider : ProviderBase, IDotTypeProvider
  {
    const int _OFFEREDTLDREQUEST = 637;
    const int _ACTIVETLDREQUEST = 635;

    private Dictionary<string, IDotTypeInfo> _dotTypesRequestCache;

    public DotTypeProvider(IProviderContainer container)
      : base(container)
    {
      _dotTypesRequestCache = new Dictionary<string, IDotTypeInfo>(100, StringComparer.OrdinalIgnoreCase);
    }

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get { return _siteContext ?? (_siteContext = Container.Resolve<ISiteContext>()); }
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

    public Dictionary<string, Dictionary<string, bool>> GetOfferedTLDFlags(OfferedTLDProductTypes tldProductType, string[] tldNames = null)
    {
      Dictionary<string, Dictionary<string, bool>> tldInfo = new Dictionary<string, Dictionary<string, bool>>(1);

      ActiveTLDsRequestData aRequest = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ActiveTLDsResponseData aResponse = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(aRequest, _ACTIVETLDREQUEST);
      var allFlags = aResponse.AllFlagNames;

      OfferedTLDsRequestData oRequest = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, SiteContext.PrivateLabelId, tldProductType);
      OfferedTLDsResponseData oResponse = (OfferedTLDsResponseData)DataCache.DataCache.GetProcessRequest(oRequest, _OFFEREDTLDREQUEST);

      tldNames = tldNames ?? new string[0];

      List<string> offeredTlds = new List<string>();
      if (tldNames.Length > 0)
      {
        foreach (var offeredTld in tldNames)
        {
          if (oResponse.OfferedTLDs.Contains(offeredTld, StringComparer.OrdinalIgnoreCase))
          {
            offeredTlds.Add(offeredTld);
          }
        }
      }
      else
      {
        offeredTlds = oResponse.OfferedTLDs.ToList();
      }

      foreach (var tld in offeredTlds)
      {
        var flagSets = new Dictionary<string, bool>();
        if (allFlags != null && allFlags.Any())
        {
          foreach (var flag in allFlags)
          {
            flagSets.Add(flag, aResponse.IsTLDActive(tld, flag));
          }
        }
        if (!tldInfo.ContainsKey(tld))
        {
          tldInfo.Add(tld, flagSets);
        }
      }

      return tldInfo;
    }

    /// Expose TLDDataCache lists
    /// Expose ActiveTLD functionality
  }
}
