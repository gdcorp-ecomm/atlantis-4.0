using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoValidate.Interface;
using System;

namespace Atlantis.Framework.PromoValidate.Impl
{
  public class ValidateNonOrderRequest : IRequest
  {
    const string _REQUESTFORMAT = "<PromoValidateNonOrder><param name=\"promoCode\" value=\"{0}\"/><param name=\"privateLabelID\" value=\"{1}\"/></PromoValidateNonOrder>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result;

      try
      {
        ValidateNonOrderRequestData validateRequest = (ValidateNonOrderRequestData)requestData;
        string cacheRequest = string.Format(_REQUESTFORMAT, validateRequest.PromoCode, validateRequest.PrivateLabelId);
        string cacheResponse = DataCache.DataCache.GetCacheData(cacheRequest);
        result = ValidateNonOrderResponseData.FromCacheDataXml(cacheResponse);
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        AtlantisException aex = new AtlantisException(requestData, "RequestHandler", message, requestData.ToXML());
        result = ValidateNonOrderResponseData.FromException(aex);
      }

      return result;
    }
  }
}
