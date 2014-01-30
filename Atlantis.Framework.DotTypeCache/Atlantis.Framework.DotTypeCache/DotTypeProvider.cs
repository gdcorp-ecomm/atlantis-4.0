using System.Globalization;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeCache.Static;
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
    private readonly ITLDDataImpl _tldDataForInvalid;
    private readonly ITLDDataImpl _tldDataForRegistration;
    private readonly ITLDDataImpl _tldDataForTransfer;
    private readonly ITLDDataImpl _tldDataForBulk;
    private readonly ITLDDataImpl _tldDataForBulkTransfer;
    private readonly IProviderContainer _container;

    public DotTypeProvider(IProviderContainer container) : base(container)
    {
      _dotTypesRequestCache = new Dictionary<string, IDotTypeInfo>(100, StringComparer.OrdinalIgnoreCase);

      _tldDataForInvalid = new TLDDataImpl(container, SiteContext.PrivateLabelId, OfferedTLDProductTypes.Invalid);
      _tldDataForRegistration = new TLDDataImpl(container, SiteContext.PrivateLabelId, OfferedTLDProductTypes.Registration);
      _tldDataForTransfer = new TLDDataImpl(container, SiteContext.PrivateLabelId, OfferedTLDProductTypes.Transfer);
      _tldDataForBulk = new TLDDataImpl(container, SiteContext.PrivateLabelId, OfferedTLDProductTypes.Bulk);
      _tldDataForBulkTransfer = new TLDDataImpl(container, SiteContext.PrivateLabelId, OfferedTLDProductTypes.BulkTransfer);

      _container = container;
    }

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get { return _siteContext ?? (_siteContext = Container.Resolve<ISiteContext>()); }
    }

    public IDotTypeInfo InvalidDotType
    {
      get { return Interface.InvalidDotType.Instance; }
    }

    public IDotTypeInfo GetDotTypeInfo(string dotType)
    {
      if (string.IsNullOrEmpty(dotType))
      {
        return Interface.InvalidDotType.Instance;
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
      IDotTypeInfo result = TLDMLDotTypes.CreateTLDMLDotTypeIfAvailable(dotType, _container);
      if (result == null)
      {
        result = StaticDotTypes.GetDotType(dotType);
        if (result != Interface.InvalidDotType.Instance)
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
      return TLDMLDotTypes.TLDMLIsAvailable(dotType, _container) || StaticDotTypes.HasDotType(dotType);
    }

    public ITLDDataImpl GetTLDDataForInvalid
    {
      get { return _tldDataForInvalid; }
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
