using System;
using Atlantis.Framework.EcommLOCAccounts.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommLOCAccounts.Impl
{
  public class EcommLOCAccountsRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommLOCAccountsResponseData response = null;

      try
      {
        var request = (EcommLOCAccountsRequestData)requestData;

        using (LOCSvc.Service getAccounts = new LOCSvc.Service())
        {
          getAccounts.Url = ((WsConfigElement)config).WSURL;
          getAccounts.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

          string responseXML;

          string status = getAccounts.GetAccountsForShopper(requestData.ShopperID, out responseXML);

          bool success = status.ToLowerInvariant().Contains("success");

          if (success)
          {
            response = new EcommLOCAccountsResponseData(success, responseXML);
          }
          else
          {
            var ex = new AtlantisException(requestData, "EcommHasLineOfCreditRequest", status, string.Empty);
            response = new EcommLOCAccountsResponseData(ex);
          }
        }
      }
      catch (AtlantisException aex)
      {
        response = new EcommLOCAccountsResponseData(aex);
      }
      catch (Exception ex)
      {
        response = new EcommLOCAccountsResponseData(requestData, ex);
      }

      return response;
    }

  }
}
