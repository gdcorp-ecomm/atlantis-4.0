using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Impl
{
  public class PromoPriceRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = PromoPriceResponseData.NoPriceFoundResponse;
      int price;
      bool isEstimate;

      var promoPriceRequest = (PromoPriceRequestData)requestData;

      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        comCache.WithOptionsGetPromoPrice(promoPriceRequest.UnifiedProductId, promoPriceRequest.PrivateLabelId, promoPriceRequest.Quantity, promoPriceRequest.Options, out price, out isEstimate);
      }

      //  returned price < 0 means it's not found for the options passed in
      if (price > -1)
      {
        result = PromoPriceResponseData.FromPrice(price, isEstimate);
      }
      return result;
    }
  }
}
