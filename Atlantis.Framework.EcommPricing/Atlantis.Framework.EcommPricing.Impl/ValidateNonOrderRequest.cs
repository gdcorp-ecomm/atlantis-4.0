using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Impl
{
  public class ValidateNonOrderRequest : IRequest
  {
    const string _REQUESTFORMAT = "<PromoValidateNonOrder><param name=\"promoCode\" value=\"{0}\"/><param name=\"privateLabelID\" value=\"{1}\"/></PromoValidateNonOrder>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ValidateNonOrderRequestData validateRequest = (ValidateNonOrderRequestData)requestData;
      string cacheRequest = string.Format(_REQUESTFORMAT, validateRequest.PromoCode, validateRequest.PrivateLabelId);

      string cacheResponse;
      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        cacheResponse = comCache.GetCacheData(cacheRequest);
      }

      return ValidateNonOrderResponseData.FromCacheDataXml(cacheResponse);
    }
  }
}
