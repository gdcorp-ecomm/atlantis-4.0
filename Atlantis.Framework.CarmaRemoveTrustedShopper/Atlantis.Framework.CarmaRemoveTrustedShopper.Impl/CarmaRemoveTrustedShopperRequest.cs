using System;
using Atlantis.Framework.CarmaRemoveTrustedShopper.Impl.CarmaWs;
using Atlantis.Framework.CarmaRemoveTrustedShopper.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaRemoveTrustedShopper.Impl
{
  public class CarmaRemoveTrustedShopperRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CarmaRemoveTrustedShopperResponseData responseData = null;
      string errors = string.Empty;

      try
      {
        CarmaRemoveTrustedShopperRequestData request = (CarmaRemoveTrustedShopperRequestData)requestData;

        using (WSgdCarmaService carmaWs = new WSgdCarmaService())
        {
          carmaWs.Url = ((WsConfigElement)config).WSURL;
          carmaWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          int response = carmaWs.RemoveTrustedShopper(request.ShopperID, request.SecondaryShopperId, out errors);

          if (response != 0)
          {
            responseData = new CarmaRemoveTrustedShopperResponseData();
          }
          else
          {
            AtlantisException aex = new AtlantisException(requestData, "CarmaRemoveTrustedShoppersRequest::RequestHandler", "Error removing trusted shopper", errors);
            responseData = new CarmaRemoveTrustedShopperResponseData(aex);
          }
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new CarmaRemoveTrustedShopperResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new CarmaRemoveTrustedShopperResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
