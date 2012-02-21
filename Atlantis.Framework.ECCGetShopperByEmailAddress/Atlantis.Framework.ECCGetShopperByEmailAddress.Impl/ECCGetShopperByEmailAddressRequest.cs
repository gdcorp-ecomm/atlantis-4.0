using System;
using Atlantis.Framework.ECCGetShopperByEmailAddress.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetShopperByEmailAddress.Impl
{
  public class ECCGetShopperByEmailAddressRequest: IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ECCGetShopperByEmailAddressResponseData response = null;

      try
      {
        var request = (ECCGetShopperByEmailAddressRequestData)requestData;

        using (eccWs.Item emailItem = new eccWs.Item())
        {
          emailItem.Url = ((WsConfigElement)config).WSURL;
          emailItem.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          var shopperId = emailItem.getShopperIdByEmailAddress(request.EmailAddress);
          response = new ECCGetShopperByEmailAddressResponseData(shopperId); 
        }
      }
      catch (AtlantisException ex)
      {
        response = new ECCGetShopperByEmailAddressResponseData(ex);
      }
      catch (Exception ex)
      {
        response = new ECCGetShopperByEmailAddressResponseData(requestData, ex);
      }

      return response;
    }
  }
}
