using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SsoServiceProvider.Interface;

namespace Atlantis.Framework.SsoServiceProvider.Impl
{
  public class SsoServiceProviderRequest : IRequest
  {
    const string _cacheDataRequestTemplate = "<ssoGetServiceProviderByName><param name=\"serviceProviderName\" value=\"{0}\" /></ssoGetServiceProviderByName>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoServiceProviderResponseData result;

      try
      {
        SsoServiceProviderRequestData ssoRequest = (SsoServiceProviderRequestData)requestData;
        string cacheDataRequest = string.Format(_cacheDataRequestTemplate, ssoRequest.ServiceProviderKey);
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
