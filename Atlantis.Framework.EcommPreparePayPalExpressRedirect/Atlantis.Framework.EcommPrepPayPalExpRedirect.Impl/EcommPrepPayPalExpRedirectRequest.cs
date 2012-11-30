using System;
using Atlantis.Framework.EcommPrepPayPalExpRedirect.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPrepPayPalExpRedirect.Impl
{
  public class EcommPrepPayPalExpRedirectRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommPrepPayPalExpRedirectResponseData responseData;
      try
      {
        var ecomRequest = (EcommPrepPayPalExpRedirectRequestData)requestData;
        using (var oSvc = new BasketService.WscgdBasketService())
        {
          oSvc.Url = ((WsConfigElement)config).WSURL;
          oSvc.Timeout = (int)ecomRequest.RequestTimeout.TotalMilliseconds;

          string token;
          string errorDesc;
          var redirectUrl = oSvc.PreparePayPalExpressRedirect(ecomRequest.ShopperID, ecomRequest.ToXML(), out token, out errorDesc);

          responseData = new EcommPrepPayPalExpRedirectResponseData(redirectUrl, errorDesc, token);
        }
      }
      catch (Exception ex)
      {
        responseData = new EcommPrepPayPalExpRedirectResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
