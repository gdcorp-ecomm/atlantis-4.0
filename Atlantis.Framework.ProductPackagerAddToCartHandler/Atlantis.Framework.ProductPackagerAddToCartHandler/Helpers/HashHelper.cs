using System.Web;
using Atlantis.Framework.GetDurationHash.Interface;
using Atlantis.Framework.GetOverrideHash.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.ProductPackagerAddToCartHandler
{
  internal static class HashHelper
  {
    internal static string GetDurationHash(int productId, double duration)
    {
      string hash = string.Empty;

      if(duration > 0)
      {
        ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();

        GetDurationHashRequestData request = new GetDurationHashRequestData(shopperContext.ShopperId,
                                                                            HttpContext.Current.Request.Url.ToString(),
                                                                            string.Empty,
                                                                            siteContext.Pathway,
                                                                            siteContext.PageCount,
                                                                            productId,
                                                                            siteContext.PrivateLabelId,
                                                                            duration);

        GetDurationHashResponseData response = (GetDurationHashResponseData) Engine.Engine.ProcessRequest(request, 27);
        hash = response.Hash;
      }

      return hash;
    }

    internal static string GetOverrideHash(int productId, int overrideListPrice, int overrideCurrentPrice)
    {
      string hash = string.Empty;

      if (productId > 0 && overrideListPrice >= 0 && overrideCurrentPrice >= 0)
      {
        ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();

        GetOverrideHashRequestData request = new GetOverrideHashRequestData(shopperContext.ShopperId,
                                                                            HttpContext.Current.Request.Url.ToString(),
                                                                            string.Empty,
                                                                            siteContext.Pathway,
                                                                            siteContext.PageCount,
                                                                            productId,
                                                                            siteContext.PrivateLabelId,
                                                                            overrideListPrice,
                                                                            overrideCurrentPrice);

        GetOverrideHashResponseData response = (GetOverrideHashResponseData) Engine.Engine.ProcessRequest(request, 26);
        hash = response.Hash;
      }

      return hash;
    }
  }
}
