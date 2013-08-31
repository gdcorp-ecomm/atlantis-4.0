using System;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Segmentation.Interface;
using Atlantis.Framework.Segmentation.Interface;

namespace Atlantis.Framework.Providers.Segmentation
{
  public class ShopperSegmentationProvider : ProviderBase, IShopperSegmentationProvider
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;
    private Lazy<string> _shopperSegmentId;

    public ShopperSegmentationProvider(IProviderContainer container)
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

    public string GetShopperSegmentId()
    {
      if (ReferenceEquals(null, _shopperSegmentId))
      {
        _shopperSegmentId = new Lazy<string>(() =>
          {
            string returnValue = GetSpoofShopperSegment();
            try
            {
              if (!String.IsNullOrEmpty(ShopperContext.ShopperId))
              {
                if (String.IsNullOrEmpty(returnValue))
                {
                  RequestData request = new ShopperSegmentRequestData(ShopperContext.ShopperId, string.Empty, string.Empty, SiteContext.Pathway, SiteContext.PageCount);
                  IResponseData response = SessionCache.SessionCache.GetProcessRequest<ShopperSegmentResponseData>(request, SegmentationEngineRequests.ShopperSegmentId);
                  ShopperSegmentResponseData converted = response as ShopperSegmentResponseData;
                  if (!ReferenceEquals(null, converted) && converted.IsSuccess)
                  {
                    returnValue = converted.SegmentId.SegmentationName();
                  }
                }
              }
            }
            catch (Exception ex)
            {
              string message = ex.Message + ex.StackTrace;
              AtlantisException exception = new AtlantisException("GetShopperSegmentId", "0", message, string.Empty, SiteContext, ShopperContext);
              Engine.Engine.LogAtlantisException(exception);
            }

            return returnValue ?? ShopperSegmentations.Nascent;
          });
      }

      return _shopperSegmentId.Value;
    }

    private string GetSpoofShopperSegment()
    {
      string result = null;
      if (SiteContext.IsRequestInternal && HttpContext.Current != null)
      {
        var segment = HttpContext.Current.Request.QueryString["qaspoofsegment"];

        if (!string.IsNullOrEmpty(segment))
        {
          result = segment;
        }
      }

      return result;
    }



  }
}
