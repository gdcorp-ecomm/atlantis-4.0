using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Impl
{
  public class ListPriceRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = ListPriceResponseData.NoPriceFoundResponse;
      int price;
      bool isEstimate;

      var listPriceRequest = (ListPriceRequestData)requestData;

      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        comCache.WithOptionsGetListPrice(listPriceRequest.UnifiedProductId, listPriceRequest.PrivateLabelId, listPriceRequest.Options, out price, out isEstimate);
      }

      //  returned price < 0 means it's not found for the options passed in
      if (price > -1)
      {
        result = ListPriceResponseData.FromPrice(price, isEstimate);
      }

      return result;
    }
  }
}
