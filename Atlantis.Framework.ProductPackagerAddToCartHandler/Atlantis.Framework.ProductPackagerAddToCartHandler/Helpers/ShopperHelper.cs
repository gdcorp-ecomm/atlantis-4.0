using System.Web;
using Atlantis.Framework.CreateShopper.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductPackagerAddToCartHandler
{
  internal static class ShopperHelper
  {
    internal static void CreateTemporaryShopper(ISiteContext siteContext, IShopperContext shopperContext)
    {
      CreateShopperRequestData request = new CreateShopperRequestData(HttpContext.Current.Request.Url.ToString(),
                                                                      string.Empty,
                                                                      siteContext.Pathway,
                                                                      siteContext.PageCount,
                                                                      siteContext.PrivateLabelId);

      CreateShopperResponseData shopperResponseData = (CreateShopperResponseData)Engine.Engine.ProcessRequest(request, 28);
      shopperContext.SetNewShopper(shopperResponseData.GetShopperId());
    }
  }
}
