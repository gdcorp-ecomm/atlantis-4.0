using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SsoServiceProvider.Interface;

namespace Atlantis.Framework.SsoServiceProvider.Impl
{
  public class SsoServiceProviderRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoServiceProviderResponseData result;

      try
      {
        SsoServiceProviderRequestData ssoRequest = (SsoServiceProviderRequestData)requestData;

        string cacheDataRequest = new XElement("ssoGetServiceProviderByName",
          new XElement("param",
            new XAttribute("name", "serviceProviderName"),
            new XAttribute("value", ssoRequest.ServiceProviderKey)
          )
        ).ToString(SaveOptions.DisableFormatting);
          
        string cacheXml = DataCache.DataCache.GetCacheData(cacheDataRequest);
        result = new SsoServiceProviderResponseData(ssoRequest.ServiceProviderKey, cacheXml);
      }
      catch (Exception ex)
      {
        result = new SsoServiceProviderResponseData(requestData, ex);
      }

      return result;
    }
  }
}
