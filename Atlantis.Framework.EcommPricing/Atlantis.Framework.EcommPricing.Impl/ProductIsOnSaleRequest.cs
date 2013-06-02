using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Impl
{
  public class ProductIsOnSaleRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = ProductIsOnSaleResponseData.NotOnSale;
      bool isOnSale;

      var productIsOnSaleRequest = (ProductIsOnSaleRequestData)requestData;

      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        isOnSale = comCache.WithOptionsIsProductOnSale(productIsOnSaleRequest.UnifiedProductId, productIsOnSaleRequest.PrivateLabelId, productIsOnSaleRequest.Options);
      }

      if (isOnSale)
      {
        result = ProductIsOnSaleResponseData.OnSale;
      }

      return result;
    }
  }
}
