using System;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Segmentation.Interface;
using Atlantis.Framework.Segmentation.Interface;

namespace Atlantis.Framework.Providers.Segmentation
{
  public class SegmentationProvider : ProviderBase, ISegmentationProvider
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;
    private Lazy<int> _shopperSegmentId;

    public SegmentationProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
    }

    private IShopperContext ShopperContext
    {
      get { return _shopperContext.Value; }
    }

    public ISiteContext SiteContext
    {
      get { return _siteContext.Value; }
    }

    public int GetShopperSegmentId()
    {
      if (ReferenceEquals(null, _shopperSegmentId))
      {
        _shopperSegmentId = new Lazy<int>(() =>
          {
            int returnValue = 0;

            RequestData request = new ShopperSegmentRequestData(ShopperContext.ShopperId, string.Empty, string.Empty, SiteContext.Pathway, SiteContext.PageCount);
            IResponseData response = SessionCache.SessionCache.GetProcessRequest<ShopperSegmentResponseData>(request, SegmentationEngineRequests.ShopperSegmentId);

            ShopperSegmentResponseData converted = response as ShopperSegmentResponseData;
            if (!ReferenceEquals(null, converted) && converted.IsSuccess)
            {
              returnValue = converted.SegmentId;
            }

            return returnValue;
          }); 
      }

      return _shopperSegmentId.Value;
    }
  }
}
