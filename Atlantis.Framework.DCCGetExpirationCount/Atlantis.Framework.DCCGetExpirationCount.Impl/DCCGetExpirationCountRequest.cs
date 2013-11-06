using Atlantis.Framework.DCCGetExpirationCount.Impl.DomainStatusWS;
using Atlantis.Framework.DCCGetExpirationCount.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetExpirationCount.Impl
{
  public class DCCGetExpirationCountRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      if (string.IsNullOrEmpty(requestData.ShopperID))
      {
        return DCCGetExpirationCountResponseData.None;
      }

      var request = (DCCGetExpirationCountRequestData)requestData;
      using (var service = new RegCheckDomainStatusWebSvcService())
      {
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        string responseXml = service.GetExpirationDomainCountsByShopperId(request.ToXML());

        return DCCGetExpirationCountResponseData.FromResponseXml(requestData.ShopperID, responseXml);
      }
    }
  }
}
