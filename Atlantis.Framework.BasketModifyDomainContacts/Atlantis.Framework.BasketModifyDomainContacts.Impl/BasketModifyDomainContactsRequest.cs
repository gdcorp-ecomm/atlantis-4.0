using System;
using Atlantis.Framework.BasketModifyDomainContacts.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BasketModifyDomainContacts.Impl
{
  public class BasketModifyDomainContactsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      try
      {
        var request = (BasketModifyDomainContactsRequestData) requestData;
        using (var basketService = new BasketService.WscgdBasketService())
        {
          var responseXml = string.Empty;
          basketService.Url = ((WsConfigElement) config).WSURL;
          basketService.Timeout = (int) request.RequestTimeout.TotalMilliseconds;
          responseXml = basketService.ModifyDomainContacts(request.ShopperID, request.BasketType, String.Join(",", request.DomainNames), request.ContactXml);
          responseData = new BasketModifyDomainContactsResponseData(responseXml);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new BasketModifyDomainContactsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new BasketModifyDomainContactsResponseData(requestData, ex);
      }
      return responseData;
    }
  }
}
