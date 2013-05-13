using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ShopperSegment.Interface;
using Atlantis.Framework.ShopperSegment.Interface;

namespace Atlantis.Framework.Providers.ShopperSegment
{
  public class ShopperSegmentProvider : ProviderBase, IShopperSegmentProvider
  {
    private readonly ISiteContext _siteContext;
    private readonly IShopperContext _shopperContext;

    public ShopperSegmentProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = Container.Resolve<ISiteContext>();
      _shopperContext = Container.Resolve<IShopperContext>();
    }

    private IShopperContext ShopperContext
    {
      get { return _shopperContext; }
    }

    public ISiteContext SiteContext
    {
      get { return _siteContext; }
    }

    public int GetShopperSegmentId()
    {
      int returnValue = 0;
      
      returnValue = GetSpoofValue(SiteContext);

      if (0 >= returnValue)
      {
        RequestData request = new ShopperSegmentRequestData(ShopperContext.ShopperId, string.Empty, string.Empty, SiteContext.Pathway, SiteContext.PageCount);
        IResponseData response = Engine.Engine.ProcessRequest(request, ShopperSegmentEngineRequests.ShopperSegmentId);

        ShopperSegmentResponseData converted = response as ShopperSegmentResponseData;
        if (!ReferenceEquals(null, converted) && converted.IsSuccess)
        {
          returnValue = converted.SegmentId;
        } 
      }

      return returnValue;
    }

    private static int GetSpoofValue(ISiteContext siteContext)
    {
      int returnValue = 0;

      if (siteContext.IsRequestInternal)
      {
        string spoofValue = HttpContext.Current.Request["QA--SEGMENTATION_VALUE"];
        if (!string.IsNullOrEmpty(spoofValue))
        {
          int.TryParse(spoofValue, out returnValue);
        }
      }

      return returnValue;
    }
  }
}
