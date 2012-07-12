using System;
using Atlantis.Framework.EcommLOCAccountDetails.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommLOCAccountDetails.Impl
{
  public class EcommLOCAccountDetailsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommLOCAccountDetailsResponseData response = null;

      try
      {
        var request = (EcommLOCAccountDetailsRequestData)requestData;

        using (LocSvc.Service getAccounts = new LocSvc.Service())
        {
          getAccounts.Url = ((WsConfigElement)config).WSURL;
          getAccounts.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

          string responseXML;

          string status = getAccounts.GetDetailsForAccount(requestData.ShopperID, request.AccountId, request.StartDate.ToShortDateString(), request.EndDate.ToShortDateString(), out responseXML);

          bool success = status.ToLowerInvariant().Contains("success");

          if (success)
          {
            response = new EcommLOCAccountDetailsResponseData(success, responseXML);
          }
          else
          {
            var ex = new AtlantisException(requestData, "EcommLOCAccountDetailsRequest", status, string.Empty);
            response = new EcommLOCAccountDetailsResponseData(ex);
          }
        }
      }
      catch (AtlantisException aex)
      {
        response = new EcommLOCAccountDetailsResponseData(aex);
      }
      catch (Exception ex)
      {
        response = new EcommLOCAccountDetailsResponseData(requestData, ex);
      }

      return response;
    }
  }
}
