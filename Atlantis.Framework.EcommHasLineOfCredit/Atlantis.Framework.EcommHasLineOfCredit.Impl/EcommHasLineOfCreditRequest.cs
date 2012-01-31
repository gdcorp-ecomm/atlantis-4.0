using System;
using Atlantis.Framework.EcommHasLineOfCredit.Impl.locWS;
using Atlantis.Framework.EcommHasLineOfCredit.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommHasLineOfCredit.Impl
{
  public class EcommHasLineOfCreditRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommHasLineOfCreditResponseData response = null;

      try
      {
        var request = (EcommHasLineOfCreditRequestData)requestData;

        using (Service locWs = new Service())
        {
          locWs.Url = ((WsConfigElement)config).WSURL;
          locWs.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

          bool hasLoc;
          string status = locWs.ShopperHasLOC(requestData.ShopperID, out hasLoc);
          
          bool success = status.ToLowerInvariant().Contains("success");

          if (success)
          {
            response = new EcommHasLineOfCreditResponseData(hasLoc, success);
          }
          else
          {
            var ex = new AtlantisException(requestData, "EcommHasLineOfCreditRequest", status, string.Empty);
            response = new EcommHasLineOfCreditResponseData(ex);
          }          
        }
      }
      catch (AtlantisException aex)
      {
        response = new EcommHasLineOfCreditResponseData(aex);
      } 
      catch (Exception ex)
      {
        response = new EcommHasLineOfCreditResponseData(requestData, ex);
      }

      return response;
    }
  }
}
