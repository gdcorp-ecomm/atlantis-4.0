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
    private readonly Dictionary<string, IDotTypeInfo> _dotTypesRequestCache;
    private readonly ITLDDataImpl _tldDataForRegistration;
    private readonly ITLDDataImpl _tldDataForTransfer;
    private readonly ITLDDataImpl _tldDataForBulk;
    private readonly ITLDDataImpl _tldDataForBulkTransfer;

    public DotTypeProvider(IProviderContainer container) : base(container)
    {
      _dotTypesRequestCache = new Dictionary<string, IDotTypeInfo>(100, StringComparer.OrdinalIgnoreCase);

      _tldDataForRegistration = new TLDDataImpl(SiteContext.PrivateLabelId, OfferedTLDProductTypes.Registration);
      _tldDataForTransfer = new TLDDataImpl(SiteContext.PrivateLabelId, OfferedTLDProductTypes.Transfer);
      _tldDataForBulk = new TLDDataImpl(SiteContext.PrivateLabelId, OfferedTLDProductTypes.Bulk);
      _tldDataForBulkTransfer = new TLDDataImpl(SiteContext.PrivateLabelId, OfferedTLDProductTypes.BulkTransfer);
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

    public ITLDDataImpl GetTLDDataForRegistration
    {
      get { return _tldDataForRegistration; }
    }

    public ITLDDataImpl GetTLDDataForTransfer
    {
      get { return _tldDataForTransfer; }
    }

    public ITLDDataImpl GetTLDDataForBulk
    {
      get { return _tldDataForBulk; }
    }

    public ITLDDataImpl GetTLDDataForBulkTransfer
    {
      get { return _tldDataForBulkTransfer; }
    }
  }
}
