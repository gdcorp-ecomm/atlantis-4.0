using System;
using Atlantis.Framework.ActivateAdCredit.Impl.WSCgdAdWordCoupons;
using Atlantis.Framework.ActivateAdCredit.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ActivateAdCredit.Impl
{
  public class ActivateAdCreditRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ActivateAdCreditResponseData responseData = null;

      try
      {
        ActivateAdCreditRequestData request = (ActivateAdCreditRequestData)requestData;

        using (WSCgdAdWordCouponsService ws = new WSCgdAdWordCouponsService())
        {
          ws.Url = (((WsConfigElement)config).WSURL);
          ws.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          string responseXml = ws.ProvisionCoupon(request.ShopperID, request.CouponKey);

          responseData = new ActivateAdCreditResponseData(responseXml);
        }
      }

      catch (Exception ex)
      {
        responseData = new ActivateAdCreditResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
