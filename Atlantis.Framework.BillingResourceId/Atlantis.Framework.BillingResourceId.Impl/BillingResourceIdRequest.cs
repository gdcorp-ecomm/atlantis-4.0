using System;
using Atlantis.Framework.BillingResourceId.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BillingResourceId.Impl
{
  public class BillingResourceIdRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      BillingResourceIdResponseData responseData;

      try
      {
        BillingResourceIdRequestData billingResourceIdRequestData = (BillingResourceIdRequestData)requestData;
        GDBillingData.WSgdBillingDataService billingDataWS = new GDBillingData.WSgdBillingDataService();
        billingDataWS.Url = ((WsConfigElement)config).WSURL;
        billingDataWS.Timeout = (int)billingResourceIdRequestData.RequestTimeout.TotalMilliseconds;

        string error;
        string result = billingDataWS.GetResourceIDForShopperID(billingResourceIdRequestData.ShopperID, billingResourceIdRequestData.ProductId, out error);

        if (!string.IsNullOrEmpty(error))
        {
          AtlantisException exception = new AtlantisException(requestData, "BillingResourceIdRequest", error, billingResourceIdRequestData.ProductId.ToString());
          responseData = new BillingResourceIdResponseData(exception);
        }
        else
        {
          responseData = new BillingResourceIdResponseData(result);
        }
      }
      catch (AtlantisException atlantisException)
      {
        responseData = new BillingResourceIdResponseData(atlantisException);
      }
      catch (Exception ex)
      {
        responseData = new BillingResourceIdResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
