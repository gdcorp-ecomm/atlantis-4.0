using System;
using Atlantis.Framework.CarmaTrustedShoppers.Impl.CarmaWs;
using Atlantis.Framework.CarmaTrustedShoppers.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaTrustedShoppers.Impl
{
  public class CarmaTrustedShoppersRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CarmaTrustedShoppersResponseData responseData = null;
      string errors = string.Empty;

      try
      {
        CarmaTrustedShoppersRequestData request = (CarmaTrustedShoppersRequestData)requestData;

        using (WSgdCarmaService carmaWs = new WSgdCarmaService())
        {
          carmaWs.Url = ((WsConfigElement)config).WSURL;
          carmaWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          string responseXml = carmaWs.GetTrustedShoppers(request.ShopperID, out errors);

          if (!string.IsNullOrWhiteSpace(responseXml) && string.IsNullOrEmpty(errors))
          {
            responseData = new CarmaTrustedShoppersResponseData(responseXml);
          }
          else
          {
            AtlantisException aex = new AtlantisException(requestData, "CarmaTrustedShoppersRequest::RequestHandler", "Error calling GetTrustedShoppers", errors);
            responseData = new CarmaTrustedShoppersResponseData(aex);
          }
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new CarmaTrustedShoppersResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new CarmaTrustedShoppersResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
