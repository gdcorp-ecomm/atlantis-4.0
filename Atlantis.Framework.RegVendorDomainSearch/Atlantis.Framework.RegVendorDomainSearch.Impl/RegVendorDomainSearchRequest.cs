using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RegVendorDomainSearch.Impl.gdDPPSearch;
using Atlantis.Framework.RegVendorDomainSearch.Interface;

namespace Atlantis.Framework.RegVendorDomainSearch.Impl
{
  public class RegVendorDomainSearchRequest : IRequest
  {
    private const int _MAX_SERVICETIMEOUT_MILLISECONDS = 300000;// in miliseconds

    public IResponseData RequestHandler(RequestData requestData, ConfigElement configElement)
    {
      IResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        RegVendorDomainSearchRequestData regVendorDomainSearchRequestData = (RegVendorDomainSearchRequestData)requestData;
        WsConfigElement wsConfig = ((WsConfigElement)configElement);

        using (gdDppSearchWS service = new gdDppSearchWS())
        {
          service.Url = wsConfig.WSURL;
          service.Timeout = (int)regVendorDomainSearchRequestData.RequestTimeout.TotalMilliseconds;

          if (regVendorDomainSearchRequestData._requestTimeout.Milliseconds > _MAX_SERVICETIMEOUT_MILLISECONDS)
          {
            service.Timeout = _MAX_SERVICETIMEOUT_MILLISECONDS;
          }

          string rqXml = requestData.ToXML();
          responseXml = service.dppDomainSearch(rqXml);
          responseData = new RegVendorDomainSearchResponseData(responseXml);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RegVendorDomainSearchResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RegVendorDomainSearchResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
