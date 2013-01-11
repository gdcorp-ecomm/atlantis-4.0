using Atlantis.Framework.Interface;
using Atlantis.Framework.RegDotTypeRegistry.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.RegDotTypeRegistry.Impl
{
  public class RegDotTypeRegistryRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      string responseXml = null;

      try
      {
        using (var regApiService = new RegistrationAPIWS.RegistrationApiWebSvc())
        {
          regApiService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          regApiService.Url = ((WsConfigElement)config).WSURL;
          responseXml = regApiService.GetTLDAPI(requestData.ToXML());
        }

        XElement responseElement = XElement.Parse(responseXml);
        result = RegDotTypeRegistryResponseData.FromXElement(responseElement);
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        string data = requestData.ToXML() + ":" + (responseXml ?? string.Empty);
        AtlantisException aex = new AtlantisException(requestData, "RegDotTypeRegistryRequest.RequestHandler", message, data, ex);
        result = RegDotTypeRegistryResponseData.FromException(aex);
      }

      return result;
    }
  }
}
